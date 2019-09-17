﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstPersonTeleportController : MonoBehaviour
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

    private Collider coll;

    // Start is called before the first frame update
    void Start()
    {
        originalRotation = transform.localRotation;
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

        //Adjust view
        mouseViewUpdate();

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if(hit.collider.gameObject.transform.tag != null)
                    if(hit.transform.tag == "location")
                    {
                        teleportTo(hit.transform.position, hit.transform.rotation);
                    }
            }
        }
    }


    private void teleportTo(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
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


}
