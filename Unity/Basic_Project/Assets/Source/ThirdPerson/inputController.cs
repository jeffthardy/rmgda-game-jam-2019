using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class inputController : MonoBehaviour
{
    public bool enableDebug = false;
    public bool allow3DMovement = true;

    public float horizontalMoveRate = 10;
    public float forwardMoveRate = 10;
    public float jumpMoveRate = 100;

    public float maxSpeed = 30;


    private Rigidbody rb;


    private float horizontalInput;
    private float verticalInput;
    private bool jumpInput;
    private float pendingJumps;
    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pendingJumps = 0;

    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetButtonDown("Quit") || Input.GetButtonDown("Cancel"))
        {
            Debug.Log("You have clicked the quit button!");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBGL	
            SceneManager.LoadScene("Menu");
#else
            SceneManager.LoadScene("Menu");
#endif
        }


        // Handle Input
        horizontalInput = Input.GetAxis("Horizontal");

        // Only allow forward/backward movement in full 3d mode
        if (allow3DMovement)
        {
            verticalInput = Input.GetAxis("Horizontal");
        }

        if (Input.GetButtonDown("Jump"))
        {
            pendingJumps++;
        }
            


    }


    private void FixedUpdate()
    {
        if(horizontalInput != 0)
            handleHorizontalInput(horizontalInput);

        if (allow3DMovement)
        {
            if (verticalInput != 0)
                handleVerticalInput(verticalInput);
        }

        if (pendingJumps>0 && isGrounded)
        {
            handleJump();
            pendingJumps = 0;
            isGrounded = false;
        }



        // Fix max horzSpeed, which also comes into play when jumping
        if (Mathf.Abs(rb.velocity.x) > maxSpeed)
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);

    }

    void handleJump()
    {
        rb.AddForce(transform.up * jumpMoveRate, ForceMode.Impulse);

    }

        void handleHorizontalInput(float horizontalInput)
    {
        

        // Handle normal sideways movement
        float moveHorizontal = horizontalInput * horizontalMoveRate;
        rb.AddForce(transform.right * moveHorizontal);

        if ((horizontalInput != 0) && enableDebug)
        {
            Debug.Log("You pressed a horizontal button!");
        }


        

    }

    void handleVerticalInput(float verticalInput)
    {

        // Handle normal forward movement
        float moveForward = verticalInput * forwardMoveRate;
        rb.AddForce(transform.forward * moveForward);


        if ((verticalInput != 0) && enableDebug)
        {
            Debug.Log("You pressed a Vertical button!");
        }
        
    }


    void OnCollisionStay(Collision collisionInfo)
    {
        if (!isGrounded && (rb.velocity.y <= 0) && (collisionInfo.collider.gameObject.tag == "ground"))
            isGrounded = true;
    }
}
