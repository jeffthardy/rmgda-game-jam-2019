﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TopZombies
{
    public class FPSController : MonoBehaviour
    {
        // How do we treat the cursor in game mode?  Locked == invisible and stuck in game
        public CursorLockMode cursorLockedMode = CursorLockMode.Locked;

        public GameObject pauseMenu;
        public GameObject flashlight;

        // How sensitive do we want turning?  Menu controlled?
        public float mouseSensitivityX = 8;
        public float mouseSensitivityY = 8;

        // Controls for general speed of motion
        public float horizontalMoveRate = 100;
        public float forwardMoveRate = 100;
        public float duckSpeed = 2;
        public float walkSpeed = 3;
        public float sprintSpeed = 6;
        public float jumpMoveRate = 100;
        public bool  enableInput = true;


        // Control the size of the player
        public float standingHeight = 2.0f;
        public float duckingHeight = 1.0f;
        public float cameraOffsetFromHead = 0.2f;



        // These could be modified to have different range of turning, but these settings feel typical
        private float minimumX = -360;
        private float minimumY = -90;
        private float maximumX = 360;
        private float maximumY = 90;


        // Internal variables
        Quaternion originalCameraRotation;
        private CapsuleCollider coll;
        private Rigidbody rb;
        private PlayerDeathEffects deathEffects;
        private bool isGrounded;
        private Camera myCamera;
        private GameObject myCameraObject;
        private float horizontalInput;
        private float verticalInput;
        private Vector3 spawnPoint;
        private Quaternion spawnRotation;
        [HideInInspector]
        public bool isDucking = false;
        public bool isSprinting = false;
        public bool isTired = false;
        public float maxSprintTime = 10.0f;
        public float sprintCoolTime = 3.0f;
        private float nextSprintTime = 0.0f;

        private float sprintTime = 0.0f;
        private bool cameraUnderControl = false;
        private GameObject cameraControlTarget ;



        private ShakeObjectOnTrigger shakeObjectOnTrigger;

        // Start is called before the first frame update
        void Start()
        {
            // Remember where we started to apply changes and respawn
            spawnPoint = transform.position;
            spawnRotation = transform.rotation;

            // Grab references of gameObject components as needed.
            rb = GetComponent<Rigidbody>();
            myCamera = GetComponentInChildren<Camera>();
            myCameraObject = myCamera.gameObject;
            coll = GetComponentInChildren<CapsuleCollider>();
            shakeObjectOnTrigger = GetComponentInChildren<ShakeObjectOnTrigger>();
            deathEffects = GetComponent<PlayerDeathEffects>();

            // Setup initial conditions
            coll.height = standingHeight;
            originalCameraRotation = myCamera.transform.localRotation;
            myCameraObject.transform.localPosition = new Vector3(0, (coll.height / 2) - cameraOffsetFromHead, 0);
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            isGrounded = true;
            Cursor.lockState = cursorLockedMode;
            // Hide cursor when locking
            Cursor.visible = (CursorLockMode.Locked != cursorLockedMode);

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetButtonDown("Quit") || Input.GetButtonDown("Cancel"))
            {
                pauseMenu.GetComponent<PauseMenu>().PauseGame();
            }

            // Debug to allow getting the cursor back in game.  Remove later
            if (Input.GetAxis("MouseEjector") == 1)
            {
                Cursor.lockState = cursorLockedMode = CursorLockMode.None;
                Cursor.visible = (CursorLockMode.Locked != cursorLockedMode);
            }

            //Adjust position
            movementUpdate();

            //Adjust view
            mouseViewUpdate();

            //Handle Ducking
            handleDuck();

            //Handle Sprinting
            handleSprint();

            // Handle toggling flashlight
            HandleFlashlight();

            if (cameraUnderControl)
            {
                myCamera.transform.LookAt(cameraControlTarget.transform.position);
            }


        }

        // Currently detecting items we can use based on trigger zones
        private void OnTriggerStay(Collider other)
        {

            if (Input.GetButton("Use") && enableInput)
            {
                if (other.gameObject.GetComponent<AudioPlayerOnUse>() != null)
                    other.gameObject.GetComponent<AudioPlayerOnUse>().Use();

                if (other.gameObject.layer == LayerMask.NameToLayer("Door"))
                    other.gameObject.transform.parent.gameObject.GetComponent<DoorController>().Use();

                if (other.gameObject.GetComponent<ClueController>() != null)
                    other.gameObject.GetComponent<ClueController>().Use();

                if (other.gameObject.GetComponent<InitialFlashlightPickup>() != null)
                    other.gameObject.GetComponent<InitialFlashlightPickup>().Use();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            // Handle enemy death
            if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                Debug.Log("Player has died from  " + collision.gameObject);
                deathEffects.Death();
            }
        }

        public void Respawn()
        {
            shakeObjectOnTrigger.ResetDetector();
            gameObject.transform.position = spawnPoint;
            gameObject.transform.rotation = spawnRotation;
        }

        public void RecordNewSpawnPoint()
        {
            spawnPoint = transform.position;
            spawnRotation = transform.rotation;
            Debug.Log("Recording new spawn location " + spawnPoint);
        }


        private void mouseViewUpdate()
        {
            if (enableInput)
            {
                // Read the mouse input axis
                var rotationX = transform.localEulerAngles;
                rotationX.y += Input.GetAxis("Mouse X") * mouseSensitivityX;
                rotationX.y = ClampAngle(rotationX.y, minimumX, maximumX);
                transform.localEulerAngles = rotationX;

                var rotationY = myCamera.transform.localEulerAngles;
                rotationY.x -= Input.GetAxis("Mouse Y") * mouseSensitivityY;
                rotationY.y = ClampAngle(rotationY.y, minimumY, maximumY);
                myCamera.transform.localEulerAngles = rotationY;
            }
        }

        public void ResetMouseView()
        {
            cameraUnderControl = false;
            myCamera.transform.localRotation = originalCameraRotation;
        }

        public void CameraTarget(GameObject gameObject)
        {
            originalCameraRotation = myCamera.transform.localRotation;
            cameraUnderControl = true;
            cameraControlTarget = gameObject;
        }

        public static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360F)
                angle += 360F;
            if (angle > 360F)
                angle -= 360F;
            return Mathf.Clamp(angle, min, max);
        }

        // Handle general keyboard input for movement
        private void movementUpdate()
        {
            if (enableInput)
            {
                horizontalInput = Input.GetAxis("Horizontal");
                verticalInput = Input.GetAxis("Vertical");

                //if (Input.GetButtonDown("Jump"))
                //{
                //    pendingJumps++;
                //}
            }
            else
            {
                // clear direction so we don't move while paused
                horizontalInput = 0;
                verticalInput = 0;
            }


        }


        void FixedUpdate()
        {
            // Handle normal sideways movement
            float moveHorizontal = horizontalInput * horizontalMoveRate;
            rb.AddForce(transform.right * moveHorizontal);

            // Handle normal forward movement
            float moveForward = verticalInput * forwardMoveRate;
            rb.AddForce(transform.forward * moveForward);

            //if (pendingJumps > 0 && isGrounded)
            //{
            //    handleJump();
            //    pendingJumps = 0;
            //    isGrounded = false;
            //}


            // Clamp max speeds
            var maxSpeed = isDucking? duckSpeed: (isSprinting ? sprintSpeed : walkSpeed);
            var speed = rb.velocity.magnitude;
            if (speed > maxSpeed)
            {
                rb.velocity = rb.velocity / speed * maxSpeed;
            }

        }


        void handleJump()
        {
            rb.AddForce(transform.up * jumpMoveRate, ForceMode.Impulse);

        }


        void handleSprint()
        {
            if (Input.GetButton("Sprint") && !isDucking && enableInput)
            {
                if (Time.time > nextSprintTime)
                {
                    // Record when we start sprinting
                    if (isSprinting == false)
                        sprintTime = Time.time;

                    if (Time.time - sprintTime > maxSprintTime)
                    {
                        isSprinting = false;
                        nextSprintTime = Time.time + sprintCoolTime;
                        StartCoroutine(SignalPlayerTired());
                    } else
                    {
                        isSprinting = true;
                    }
                }
            }
            else
            {
                isSprinting = false;
            }
        }

        // Cool off time between sprints
        IEnumerator SignalPlayerTired()
        {
            isTired = true;
            yield return new WaitForSeconds(sprintCoolTime);
            isTired = false;
        }

        void handleDuck()
        {
            if (Input.GetButton("Duck") && enableInput)
            {

                // Change colider size and camera location to duck size
                if (isDucking == false)
                {
                    coll.transform.localPosition = new Vector3(coll.transform.localPosition.x, coll.transform.localPosition.y - (standingHeight - duckingHeight) / 2, coll.transform.localPosition.z);
                    isDucking = true;
                    coll.height = duckingHeight;

                }
            }
            else
            {
                // Change colider size and camera location to full size
                if (isDucking == true)
                {
                    //Check if we can stand in global space FIXME
                    if (Physics.CheckCapsule(coll.transform.position, coll.transform.TransformPoint(new Vector3(coll.transform.localPosition.x, coll.transform.localPosition.y + (standingHeight - duckingHeight), coll.transform.localPosition.z)), coll.radius))
                    {
                        coll.transform.localPosition = new Vector3(coll.transform.localPosition.x, coll.transform.localPosition.y + (standingHeight - duckingHeight) / 2, coll.transform.localPosition.z);
                        coll.height = standingHeight;
                        isDucking = false;

                    }

                }

                //, unless that would cause a collition... then stay ducked until it doesnt cause one

            }
            float cameraPosition = (coll.height / 2) - cameraOffsetFromHead;
            //Debug.Log("Camera position" + cameraPosition + "  " + coll.height + " / - " + cameraOffsetFromHead);
            myCameraObject.transform.localPosition = new Vector3(myCameraObject.transform.localPosition.x, cameraPosition, myCameraObject.transform.localPosition.z);
            //Debug.Log("Camera position read " + myCameraObject.transform.position);

        }


        void OnCollisionStay(Collision collisionInfo)
        {
            if (!isGrounded && (rb.velocity.y <= 0) && (collisionInfo.collider.gameObject.tag == "ground"))
                isGrounded = true;
        }

        public void InputControl(bool enabled)
        {
           enableInput = enabled;
        }

        void HandleFlashlight()
        {
            if (Input.GetButton("Flashlight") && enableInput)
            {
                flashlight.GetComponent<FlashlightController>().toggleFlashlight();
            }

        }
    }
}
