using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopZombies
{
    public class HeavyBreathingController : MonoBehaviour
    {

        public AudioClip[] breathSounds;
        public float playRate = 1.0f;

        private AudioSource audioSource;
        private int currentSound;
        private float readyTime;

        // Start is called before the first frame update
        void Start()
        {
            currentSound = 0;
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = breathSounds[currentSound];

            readyTime = Time.time + playRate;

        }

        private int lastIndex = 0;
        // Update is called once per frame
        void Update()
        {
            int clipIndex = lastIndex;

            if ((transform.gameObject.GetComponentInParent<FPSController>().isTired) && !audioSource.isPlaying && (Time.time > readyTime))
            {
                while (lastIndex == clipIndex)
                    clipIndex = Random.Range(0, breathSounds.Length - 1);
                lastIndex = clipIndex;
                audioSource.clip = breathSounds[clipIndex];
                readyTime = Time.time + playRate;
                audioSource.Play();
            }
        }
    }
}