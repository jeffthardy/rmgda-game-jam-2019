using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopZombies {
    public class ClueController : MonoBehaviour
    {
        public bool isEnabled = false;
        public AudioClip useSound;
        public int uses = 1;
        public int useCount = 0;
        public float timeDisabledAfterUse = 0.5f;

        public bool triggersNightmareMode = false;
        public bool triggersDreamMode = false;
        public GameObject blockage;
        public bool enablesClue = true;
        public GameObject nextClue;

        private NightmareController nightmareController;

        private AudioSource audioSource;
        private float timeAvailable = 0;

        // Start is called before the first frame update
        void Start()
        {
            nightmareController = GameObject.Find("NightmareController").GetComponent<NightmareController>();
            audioSource = GetComponent<AudioSource>();
            //if (isEnabled)
            //{
            //    GetComponent<FlashingIndicator>().turnOnFlasher();
            //}
            //else
            //{
            //    GetComponent<FlashingIndicator>().turnOffFlasher();
            //}

        }

        // Update is called once per frame
        void Update()
        {

        }


        public void Use()
        {
            // In case we want something you can use more than once
            if(isEnabled && (useCount < uses) && (Time.time > timeAvailable))
            {
                timeAvailable = Time.time + timeDisabledAfterUse;
                Debug.Log("using item " + gameObject);
                useCount++;
                audioSource.PlayOneShot(useSound);

                // This is the last use
                if (useCount == uses)
                {
                    GetComponent<FlashingIndicator>().DisableFlasher();
                }

                if (triggersNightmareMode)
                {
                    nightmareController.SwitchToNightmare();
                }
                if (triggersDreamMode)
                {
                    nightmareController.SwitchToDream();
                    blockage.SetActive(false);
                }

                if (enablesClue)
                    nextClue.GetComponent<ClueController>().enableClue();
            }
        }

        public void enableClue()
        {
            isEnabled = true;
            GetComponent<FlashingIndicator>().EnableFlasher();
        }
    }
}