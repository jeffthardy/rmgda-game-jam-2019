using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FPSController : MonoBehaviour
{
    public float rotationX;
    public float rotationY;
    public float sensitivityX;
    public float sensitivityY;

    public float minimumX;
    public float minimumY;
    public float maximumX;
    public float maximumY;

    Quaternion originalRotation;
    public CursorLockMode wantedMode;

    private Collider coll;
    private Rigidbody rb;
    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        originalRotation = transform.localRotation;
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        isGrounded = true;

        Cursor.lockState = wantedMode;
        // Hide cursor when locking
        Cursor.visible = (CursorLockMode.Locked != wantedMode);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Quit") || Input.GetButtonDown("Cancel"))
        {
            Debug.Log("You have clicked the quit button!");
            Cursor.lockState = wantedMode = CursorLockMode.None;
            Cursor.visible = (CursorLockMode.Locked != wantedMode);
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBGL	
            SceneManager.LoadScene("Menu");
#else
            SceneManager.LoadScene("Menu");
#endif
        }

        if (Input.GetAxis("MouseEjector") == 1)
        {
            Cursor.lockState = wantedMode = CursorLockMode.None;
            Cursor.visible = (CursorLockMode.Locked != wantedMode);
        }

        //Adjust position
        movementUpdate();

        //Adjust view
        mouseViewUpdate();
        
    }
    

    private void mouseViewUpdate()
    {
        // Read the mouse input axis
        rotationX += Input.GetAxis("Mouse X") * sensitivityX;
        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
        rotationX = ClampAngle(rotationX, minimumX, maximumX);
        rotationY = ClampAngle(rotationY, minimumY, maximumY);
        Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
        Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);

        transform.localRotation = originalRotation * xQuaternion * yQuaternion;
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

    public float horizontalMoveRate = 10;
    public float forwardMoveRate = 10;
    public float maxSpeed = 15;
    public float jumpMoveRate = 100;

    private float horizontalInput;
    private float verticalInput;
    private float pendingJumps = 0;
    private bool jumpHeldDown;

    // Handle general keyboard input for movement
    private void movementUpdate()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Jump"))
        {
            pendingJumps++;
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

        if (pendingJumps > 0 && isGrounded)
        {
            handleJump();
            pendingJumps = 0;
            isGrounded = false;
        }


        //Bad code...diagonal is fast
        float newXSpeed = rb.velocity.x;
        float newYSpeed = rb.velocity.y;
        float newZSpeed = rb.velocity.z;

        // Fix max horzSpeed, which also comes into play when jumping
        if (Mathf.Abs(newXSpeed) > maxSpeed)
        {
            newXSpeed = Mathf.Sign(rb.velocity.x) * maxSpeed;
            rb.velocity = new Vector3(newXSpeed, newYSpeed, newZSpeed);
        }

        if (Mathf.Abs(newYSpeed) > maxSpeed)
        {
            newYSpeed = Mathf.Sign(rb.velocity.y) * maxSpeed;
            rb.velocity = new Vector3(newXSpeed, newYSpeed, newZSpeed);
        }

        if (Mathf.Abs(newZSpeed) > maxSpeed)
        {
            newZSpeed = Mathf.Sign(rb.velocity.z) * maxSpeed;
            rb.velocity = new Vector3(newXSpeed, newYSpeed, newZSpeed);
        }

    }


    void handleJump()
    {
        rb.AddForce(transform.up * jumpMoveRate, ForceMode.Impulse);

    }


    void OnCollisionStay(Collision collisionInfo)
    {
        if (!isGrounded && (rb.velocity.y <= 0) && (collisionInfo.collider.gameObject.tag == "ground"))
            isGrounded = true;
    }
}
