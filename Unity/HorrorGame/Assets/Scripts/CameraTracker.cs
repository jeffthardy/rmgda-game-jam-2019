using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopZombies
{
    // Meant for second camera that is identical to main camera, but only displays clues so they can avoid clipping surrounding meshes
    public class CameraTracker : MonoBehaviour
    {
        public bool isEnabled;
        private Camera myCamera;
        // Start is called before the first frame update
        void Start()
        {
            myCamera = GetComponent<Camera>();
            transform.position = Camera.main.transform.position;
            transform.rotation = Camera.main.transform.rotation;
            myCamera.enabled = false;
            isEnabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = Camera.main.transform.position;
            transform.rotation = Camera.main.transform.rotation;

        }

        public void EnableCamera(bool enabled)
        {
            myCamera.enabled = enabled;
            isEnabled = enabled;
        }
    }
}