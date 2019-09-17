using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class inputController : MonoBehaviour
{
    public bool enableDebug = false;
    public bool allow3DMovement = true;
    public float zMax = 10f;
    public float xMax = 10f;
    public float playerSpeed;
    


    private float horizontalInput;
    private float verticalInput;

    // Start is called before the first frame update
    void Start()
    {

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
        handleHorizontalInput(Input.GetAxis("Horizontal"));
        
        // Only allow forward/backward movement in full 3d mode
        if (allow3DMovement)
        {
            handleVerticalInput(Input.GetAxis("Vertical"));
        }




    }


    void handleHorizontalInput(float horizontalInput)
    {

        // Handle Input
        transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * playerSpeed);

        if ((horizontalInput != 0) && enableDebug)
        {
            Debug.Log("You pressed a horizontal button!");
        }

        //Set min X range
        if (transform.position.x < -xMax)
            transform.position = new Vector3(-xMax, transform.position.y, transform.position.z);

        // Set max X range
        if (transform.position.x > xMax)
            transform.position = new Vector3(xMax, transform.position.y, transform.position.z);


    }

    void handleVerticalInput(float verticalInput)
    {
        // Handle Input
        transform.Translate(Vector3.forward * verticalInput * Time.deltaTime * playerSpeed);

        if ((verticalInput != 0) && enableDebug)
        {
            Debug.Log("You pressed a Vertical button!");
        }
        
        //Set min Z range
        if (transform.position.z < -zMax)
            transform.position = new Vector3(transform.position.x, transform.position.y, -zMax);

        // Set max Z range
        if (transform.position.z > zMax)
            transform.position = new Vector3(transform.position.x, transform.position.y, zMax);
    }
}
