﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TopZombies
{
    public class NightmareController : MonoBehaviour
    {
        public Light nightmareLight;
        public GameObject GlobalLightParent;
        public GameObject GlobalWorldParent;
        public GameObject GlobalAudioSourceGameObject;
        public AudioClip nightmareAudio;
        public AudioClip dreamAudio;

        public float nightmareBumpLevel = 5;

        public bool nightmareMode = false;


        // Set this to true and continuously toggle between modes
        public bool debugToggler = false;

        private AudioSource audioSource;


        // Start is called before the first frame update
        void Start()
        {
            audioSource = GlobalAudioSourceGameObject.GetComponent<AudioSource>();
            if (debugToggler)
                StartCoroutine(ToggleNightmare());
            if(!nightmareMode)
                nightmareLight.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SwitchToNightmare()
        {
            audioSource.PlayOneShot(nightmareAudio);
            nightmareMode = true;
            RenderSettings.ambientLight = new Color(0, 0, 0, 0);
            nightmareLight.enabled = true;

            // Disable all the lights
            setGlobalLightActive(false);
            setGlobalWorldTextureActive(false);
        }

        public void SwitchToDream()
        {
            audioSource.PlayOneShot(dreamAudio);
            nightmareMode = false;
            RenderSettings.ambientLight = new Color(0.5f, 0.5f, 0.5f, 1);
            nightmareLight.enabled = false;

            // Enable all the lights
            setGlobalLightActive(true);
            setGlobalWorldTextureActive(true);
        }


        // Assumes all lights are under an empty parent GlobalLightParent.
        private void setGlobalLightActive(bool enabled)
        {
            for (int i = 0; i < GlobalLightParent.transform.childCount; i++)
            {
                var child = GlobalLightParent.transform.GetChild(i).gameObject;
                if (child != null)
                    child.SetActive(enabled);
            }

        }


        private void setGlobalWorldTextureActive(bool enabled)
        {
            for (int i = 0; i < GlobalWorldParent.transform.childCount; i++)
            {
                var child = GlobalWorldParent.transform.GetChild(i).gameObject.GetComponent<Renderer>();
                if (child != null)
                    if (!enabled)
                    {
                        child.material.color = new Color(1, 0, 0, 1);
                        child.material.SetFloat("_BumpScale", nightmareBumpLevel);
                    }
                    else
                    {
                        child.material.color = new Color(1, 1, 1, 1);
                        child.material.SetFloat("_BumpScale", 1);
                    }
            }

        }



        public float toggleRate = 1.0f;
        IEnumerator ToggleNightmare()
        {
            yield return new WaitForSeconds(toggleRate);
            SwitchToNightmare();
            yield return new WaitForSeconds(toggleRate);
            SwitchToDream();
            StartCoroutine(ToggleNightmare());

        }

    }
}