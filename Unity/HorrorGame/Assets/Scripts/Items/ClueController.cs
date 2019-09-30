using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopZombies {
    public class ClueController : MonoBehaviour
    {
        public bool isEnabled = true;
        public AudioClip useSound;
        public int uses = 1;
        public int useCount = 0;
        public float timeDisabledAfterUse = 0.5f;


        private AudioSource audioSource;
        private float timeAvailable = 0;

        // Start is called before the first frame update
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            if (isEnabled)
            {
                GetComponent<FlashingIndicator>().turnOnFlasher();
            }
            else
            {
                GetComponent<FlashingIndicator>().turnOffFlasher();
            }

        }

        // Update is called once per frame
        void Update()
        {

        }


        public void Use()
        {
            // In case we want something you can use more than once
            if((useCount < uses) && (Time.time > timeAvailable))
            {
                timeAvailable = Time.time + timeDisabledAfterUse;
                Debug.Log("using item " + gameObject);
                useCount++;
                audioSource.PlayOneShot(useSound);

                // This is the last use
                if (useCount == uses)
                {
                    GetComponent<FlashingIndicator>().turnOffFlasher();
                }

            }
        }
    }
}