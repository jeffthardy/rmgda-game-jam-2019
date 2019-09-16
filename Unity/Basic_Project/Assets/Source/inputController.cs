using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class inputController : MonoBehaviour
{
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

    }
}
