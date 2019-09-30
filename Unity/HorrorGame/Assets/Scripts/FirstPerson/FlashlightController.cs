using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopZombies
{
    public class FlashlightController : MonoBehaviour
    {
        public float maxIntensity = 3;
        public bool isOn = true;
        public float toggleTime = 0.2f;
        public AudioClip toggleSound;

        private AudioSource audioSource;

        private Light flashlight;
        private float availableTime = 0;

        // Start is called before the first frame update
        void Start()
        {
            flashlight = GetComponent<Light>();
            audioSource = GetComponent<AudioSource>();

            if (isOn)
                flashlight.intensity = maxIntensity;
            else
                flashlight.intensity = 0;

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void toggleFlashlight()
        {
            if (Time.time > availableTime)
            {
                availableTime = Time.time + toggleTime;
                audioSource.PlayOneShot(toggleSound);

                if (isOn)
                {
                    flashlight.intensity = 0;
                    isOn = false;
                }
                else
                {
                    flashlight.intensity = maxIntensity;
                    isOn = true;
                }
            }

        }
    }
}