using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopZombies
{
    public class FlashingIndicator : MonoBehaviour
    {
        public bool isFlashing = false;        

        public float pulseRate = 1.0f;
        public float startTime = 0;
        public float frequency = 0.25f;

        private Renderer[] renderers;
        private Material mat;
        public Color flashColor;
        public bool flasherEnabled = false;


        // Start is called before the first frame update
        void Start()
        {
            renderers = GetComponentsInChildren<Renderer>();
            if (isFlashing)
            {
                flasherEnabled = true;
                TurnOnFlasher();
            } else
            {
                TurnOffFlasher();
            }
            //StartCoroutine(DummyChanger());


        }

        // Update is called once per frame
        void Update()
        {
            if (isFlashing)
            {
                float scale = Mathf.Sin((Time.time - startTime)* frequency * Mathf.PI);
                scale = (scale / 2);
                //Debug.Log("Base "+flashColor);
                Color newColor = flashColor * scale + flashColor/2;
                //Debug.Log("Current: " +newColor);
                foreach (Renderer rend in renderers)
                {
                    for (int i = 0; i < rend.materials.Length; i++)
                    {
                        rend.materials[i].SetColor("_EmissionColor", newColor);
                    }
                }
                //Debug.Log("Read: " + mat.GetColor("_EmissionColor"));
            }


        }


        public void TurnOnFlasher()
        {
            if (flasherEnabled)
            {
                isFlashing = true;
                startTime = Time.time;
                foreach (Renderer rend in renderers)
                {
                    for (int i= 0; i < rend.materials.Length; i++)
                    {
                        rend.materials[i].EnableKeyword("_EMISSION");
                        rend.materials[i].globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
                    }
                }
                //Debug.Log("Turn on lights");
            }

        }
        public void TurnOffFlasher()
        {
            isFlashing = false;
            foreach (Renderer rend in renderers)
            {
                for (int i = 0; i < rend.materials.Length; i++)
                {
                    rend.materials[i].DisableKeyword("_EMISSION");
                    rend.materials[i].globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
                    rend.materials[i].SetColor("_EMISSION", Color.black);
                }
            }
            //Debug.Log("Turn off lights");
        }

        public void DisableFlasher()
        {
            flasherEnabled = false;
            TurnOffFlasher();
        }

        public void EnableFlasher()
        {
            flasherEnabled = true;
        }

        IEnumerator DummyChanger()
        {

            yield return new WaitForSeconds(5);

            if (isFlashing)
            {
                TurnOffFlasher();
            }
            else
            {
                TurnOnFlasher();
            }

            StartCoroutine(DummyChanger());
        }



        private void OnTriggerEnter(Collider other)
        {
            // Player is in range
            if ((other.gameObject.GetComponent<FPSController>() != null) && flasherEnabled)
            {
                TurnOnFlasher();
                Debug.Log("Detected player enter");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // Player is in range
            if ((other.gameObject.GetComponent<FPSController>() != null) && flasherEnabled)
            {
                TurnOffFlasher();
                Debug.Log("Detected player exit");
            }
        }
    }
}