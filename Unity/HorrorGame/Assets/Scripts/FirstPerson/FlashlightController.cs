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
        public AudioClip[] toggleSounds;

        private AudioSource audioSource;

        private Light flashlight;
        private float availableTime = 0;
        private int index=0;

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
            if ((Time.time > availableTime) || (Time.timeScale == 0))
            {
                availableTime = Time.time + toggleTime;

                // Play new spawn audio
                if (toggleSounds.Length > 1)
                {
                    int previousIndex = index;
                    //Debug.Log(previousIndex + " " + detectIndex);
                    index = previousIndex + 1;
                    if (index > toggleSounds.Length - 1)
                        index = 0;
                }
                else
                {
                    index = 0;
                }
                //Debug.Log(Time.time + ":" + detectIndex);
                audioSource.PlayOneShot(toggleSounds[index]);

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