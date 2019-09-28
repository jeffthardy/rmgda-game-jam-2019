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

        private Renderer rend;
        private Material mat;
        public Color flashColor;


        // Start is called before the first frame update
        void Start()
        {
            rend = GetComponentInChildren<Renderer>();
            mat = rend.material;
            if (isFlashing)
            {
                turnOnFlasher();
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
                Debug.Log("Current: " +newColor);
                mat.SetColor("_EmissionColor", newColor);
                Debug.Log("Read: " + mat.GetColor("_EmissionColor"));
            }


        }


        public void turnOnFlasher()
        {
            isFlashing = true;
            startTime = Time.time;
            mat.EnableKeyword("_EMISSION");
            mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
            Debug.Log("Turn on lights");

        }
        public void turnOffFlasher()
        {
            isFlashing = false;
            mat.DisableKeyword("_EMISSION");
            mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
            mat.SetColor("_EMISSION", Color.black);
            Debug.Log("Turn off lights");
        }


        IEnumerator DummyChanger()
        {

            yield return new WaitForSeconds(5);

            if (isFlashing)
            {
                turnOffFlasher();
            }
            else
            {
                turnOnFlasher();
            }

            StartCoroutine(DummyChanger());
        }
    }
}