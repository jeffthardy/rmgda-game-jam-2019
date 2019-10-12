using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopZombies
{

    public class EnemyAudioGenerator : MonoBehaviour
    {
        public AudioClip[] detectClips;
        public AudioClip[] idleClips;
        public AudioClip[] walkingClips;
        public float deadTimeBetweenDetections = 1.0f;


        private AudioSource audioSource;
        private int detectIndex;
        private int idleIndex;
        private int walkingIndex;
        private float nextPlayTime;

        // Start is called before the first frame update
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            nextPlayTime = Time.time;
            detectIndex = 0;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void playDetectAudio()
        {
            //Debug.Log(Time.time);
            // Only play audio if last clip is done
            if (Time.time > nextPlayTime)
            {
                // Play new spawn audio
                if (detectClips.Length > 1)
                {
                    int previousIndex = detectIndex;
                    //Debug.Log(previousIndex + " " + detectIndex);
                    detectIndex = previousIndex + 1;
                    if (detectIndex > detectClips.Length - 1)
                        detectIndex = 0;
                }
                else
                {
                    detectIndex = 0;
                }
                //Debug.Log(Time.time + ":" + detectIndex);
                audioSource.PlayOneShot(detectClips[detectIndex]);
                nextPlayTime = Time.time + detectClips[detectIndex].length + deadTimeBetweenDetections;
            }
        }

        public void playIdleAudio()
        {
            //Debug.Log(Time.time);
            // Only play audio if last clip is done
            if (Time.time > nextPlayTime)
            {
                // Play new spawn audio
                if (idleClips.Length > 1)
                {
                    int previousIndex = detectIndex;
                    //Debug.Log(previousIndex + " " + detectIndex);
                    idleIndex = previousIndex + 1;
                    if (idleIndex > idleClips.Length - 1)
                        idleIndex = 0;
                }
                else
                {
                    detectIndex = 0;
                }
                //Debug.Log(Time.time + ":" + detectIndex);
                audioSource.PlayOneShot(idleClips[idleIndex]);
                nextPlayTime = Time.time + idleClips[detectIndex].length;
            }

        }

        public void playWalkingAudio()
        {
            //Debug.Log(Time.time);
            // Only play audio if last clip is done
            if (Time.time > nextPlayTime)
            {
                // Play new spawn audio
                if (walkingClips.Length > 1)
                {
                    int previousIndex = detectIndex;
                    walkingIndex = previousIndex + 1;
                    if (walkingIndex > walkingClips.Length - 1)
                        walkingIndex = 0;
                }
                else
                {
                    detectIndex = 0;
                }
                audioSource.PlayOneShot(walkingClips[walkingIndex]);
                nextPlayTime = Time.time + walkingClips[walkingIndex].length;
            }

        }
    }
}