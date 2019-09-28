using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopZombies
{
    public class FootstepsOnMove : MonoBehaviour
    {

        public AudioClip[] walkSounds;
        public AudioClip[] sprintSounds;
        public AudioClip[] duckSounds;

        public float[] playRate;
        public float xVelocityMin = 2;
        public float zVelocityMin = 2;

        private Rigidbody rb;
        private AudioSource audioSource;
        private FPSController fPSController;
        private bool isActive;
        private int currentSound;
        private enum SoundMode { walking = 0,
            sprinting = 1,
            crawling = 2
        };
        private SoundMode soundMode;
        private float readyTime;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponentInParent<Rigidbody>(); 
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = walkSounds[currentSound];
            currentSound = 0;
            soundMode = SoundMode.walking;
            isActive = true;
            readyTime = Time.time + playRate[currentSound];
        }

        // Update is called once per frame
        void Update()
        {
            if ((Mathf.Abs(rb.velocity.x) > xVelocityMin) || (Mathf.Abs(rb.velocity.z) > zVelocityMin))
            {
                isActive = true;
                //Debug.Log(rb.velocity.magnitude);
            }
            else
            {
                isActive = false;
                audioSource.Stop();
            }

            if (transform.gameObject.GetComponentInParent<FPSController>().isSprinting)
            {
                soundMode = SoundMode.sprinting;
            }
            else
            {
                if (transform.gameObject.GetComponentInParent<FPSController>().isDucking)
                {
                    soundMode = SoundMode.crawling;
                }
                else
                {
                    soundMode = SoundMode.walking;
                }
            }



            if (isActive && (Time.time > readyTime))
            {
                //Debug.Log("Playing audio " + sounds[currentSound]);
                audioSource.clip = getAudioClip(soundMode);
                if (!audioSource.isPlaying)
                {
                    audioSource.Play(0);
                }
                else
                {
                    Debug.Log("Warning! Audio replay rate is faster than sound clip!");
                    audioSource.Stop();
                    audioSource.Play(0);
                }
                readyTime = Time.time + playRate[(int)soundMode];
            }

        }

        private int lastIndex = 0;
        private AudioClip getAudioClip(SoundMode mode)
        {
            int clipIndex= lastIndex;
            switch (soundMode)
            {
                case SoundMode.walking:
                    while (lastIndex == clipIndex)
                        clipIndex = Random.Range(0, walkSounds.Length - 1);
                    lastIndex = clipIndex;
                    return walkSounds[clipIndex];
                case SoundMode.sprinting:
                    while (lastIndex == clipIndex)
                        clipIndex = Random.Range(0, sprintSounds.Length - 1);
                    lastIndex = clipIndex;
                    return sprintSounds[clipIndex];
                case SoundMode.crawling:
                    while (lastIndex == clipIndex)
                        clipIndex = Random.Range(0, duckSounds.Length - 1);
                    lastIndex = clipIndex;
                    return duckSounds[clipIndex];
            }

            return null;
        }
    }
}