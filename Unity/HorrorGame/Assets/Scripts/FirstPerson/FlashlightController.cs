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

        //making public for debug 
        public bool isUseable = false;

        public bool includeNightmareFlicker = true;
        public float minFlickerDelay = 3.0f;
        public float maxFlickerDelay = 12.0f;
        public Color dreamLightColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);
        public Color nightmareLightColor = new Color(1.0f, 105.0f / 255.0f, 143.0f / 255.0f, 1.0f);
        private float nightmareStartTime = 0.0f;
        private float nextNightmareTime = 0.0f;

        private NightmareController nightmareController;

        // Start is called before the first frame update
        void Start()
        {
            flashlight = GetComponent<Light>();
            audioSource = GetComponent<AudioSource>();
            flashlight.color = dreamLightColor;

            if (isOn && isUseable)
                flashlight.intensity = maxIntensity;
            else
                flashlight.intensity = 0;


            nightmareController= GameObject.Find("NightmareController").GetComponent<NightmareController>();

        }

        // Update is called once per frame
        void Update()
        {
            if(nightmareController.nightmareMode && (Time.time > nextNightmareTime) && includeNightmareFlicker)
            {
                nextNightmareTime = Time.time + Random.Range(minFlickerDelay, maxFlickerDelay);
                StartCoroutine(FlickerFlashlight());
            }
        }

        public void makeUseable()
        {
            isUseable = true;
        }

        public void toggleFlashlight()
        {
            if (((Time.time > availableTime) || (Time.timeScale == 0)) && isUseable)
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
        public void UpdateForNightmare()
        {
            if (nightmareController.nightmareMode)
            {
                flashlight.color = nightmareLightColor;
                nightmareStartTime = Time.time;
            } else
            {
                flashlight.enabled = true;
                flashlight.color = dreamLightColor;
            }
        }

        IEnumerator FlickerFlashlight()
        {
            flashlight.enabled = false;
            yield return new WaitForSeconds(0.3f);
            flashlight.enabled = true;
            yield return new WaitForSeconds(0.1f);
            flashlight.enabled = false;
            yield return new WaitForSeconds(0.2f);
            flashlight.enabled = true;
            yield return new WaitForSeconds(0.1f);
            flashlight.enabled = false;
            yield return new WaitForSeconds(0.1f);
            flashlight.enabled = true;
        }
    }
}