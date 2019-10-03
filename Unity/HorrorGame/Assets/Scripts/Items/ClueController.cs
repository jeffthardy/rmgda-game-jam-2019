using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopZombies {
    public class ClueController : MonoBehaviour
    {
        public bool isEnabled = false;
        public AudioClip useSound;
        public int uses = 1;
        public int useCount = 0;
        public float timeDisabledAfterUse = 0.5f;

        public bool triggersNightmareMode = false;
        public bool triggersDreamMode = false;
        public GameObject blockage;
        public bool enablesClue = true;
        public GameObject nextClue;
        public float minTimeToHoldItem = 1.0f;

        private FlashlightController flashlight;


        private NightmareController nightmareController;

        private FPSController fPSController;
        private bool isViewingClue = false;
        private AudioSource audioSource;
        private float timeAvailable = 0;
        private Quaternion originalRotation;
        private Vector3 originalPosition;
        private Vector3 originalScale;
        private float releaseTime=0;

        // Start is called before the first frame update
        void Start()
        {
            nightmareController = GameObject.Find("NightmareController").GetComponent<NightmareController>();
            fPSController = GameObject.Find("Player").GetComponent<FPSController>();
            flashlight = GameObject.Find("Player/MainCamera/Flashlight").GetComponent<FlashlightController>();
            audioSource = GetComponent<AudioSource>();

            originalRotation = transform.rotation;
            //if (isEnabled)
            //{
            //    GetComponent<FlashingIndicator>().turnOnFlasher();
            //}
            //else
            //{
            //    GetComponent<FlashingIndicator>().turnOffFlasher();
            //}

        }

        float rotationX = 0;
        float rotationY = 0;

        float flashlightTimeAvailable;
        // Update is called once per frame
        void Update()
        {

            // Normal input should be disabled during this...
            if (isViewingClue)
            {
                if ((Input.GetButton("Use") && (Time.realtimeSinceStartup > releaseTime)))
                    PutDownClue();

                // Still allow flashlight?
                if (Input.GetButton("Flashlight"))
                {
                    if(Time.realtimeSinceStartup > flashlightTimeAvailable)
                    {
                        flashlightTimeAvailable = Time.realtimeSinceStartup + 1.0f;
                        flashlight.toggleFlashlight();
                    }
                }

                // Read the mouse input axis
                rotationX += Input.GetAxis("Mouse X") * fPSController.mouseSensitivityX;
                rotationY += Input.GetAxis("Mouse Y") * fPSController.mouseSensitivityY;
                Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
                Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);

                transform.localRotation = originalRotation * xQuaternion * yQuaternion;
            }

        }


        public void Use()
        {
            // In case we want something you can use more than once
            if(isEnabled && (useCount < uses) && (Time.time > timeAvailable))
            {
                timeAvailable = Time.time + timeDisabledAfterUse;
                Debug.Log("using item " + gameObject);
                useCount++;
                audioSource.PlayOneShot(useSound);

                // This is the last use
                if (useCount == uses)
                {
                    GetComponent<FlashingIndicator>().DisableFlasher();
                }

                if (triggersNightmareMode)
                {
                    nightmareController.SwitchToNightmare();
                }
                if (triggersDreamMode)
                {
                    nightmareController.SwitchToDream();
                    blockage.SetActive(false);
                }

                if (enablesClue)
                    nextClue.GetComponent<ClueController>().enableClue();

                originalPosition = transform.position;
                originalScale = transform.localScale;
                ViewClue();
            }
        }

        private float targetSize = 1.0f;
        void ViewClue()
        {

            // Disable player
            fPSController.InputControl(false);
            //disable scene time
            Time.timeScale = 0;
            isViewingClue = true;
            Vector3 xyz = transform.GetComponentInChildren<MeshFilter>().mesh.bounds.size;
            float size = Mathf.Max(xyz.x, xyz.y, xyz.z);
            
            transform.position = Camera.main.transform.position + Camera.main.transform.forward * 1;
            float scaleFactor = targetSize / size;
            Vector3 newScale = new Vector3(originalScale.x * scaleFactor, originalScale.y * scaleFactor, originalScale.z * scaleFactor);
            transform.localScale = newScale;
            releaseTime = Time.realtimeSinceStartup + minTimeToHoldItem;
        }



        void PutDownClue()
        {
            transform.rotation = originalRotation;
            transform.position = originalPosition;
            transform.localScale = originalScale;

            // Enable player
            fPSController.InputControl(true);
            //enable scene time
            Time.timeScale = 1;
            isViewingClue = false;
        }

        public void enableClue()
        {
            isEnabled = true;
            GetComponent<FlashingIndicator>().EnableFlasher();
        }
    }
}