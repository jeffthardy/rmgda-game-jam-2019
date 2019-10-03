using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopZombies
{

    public class EnemyAudioGenerator : MonoBehaviour
    {
        public AudioClip[] detectClips;
        public float deadTimeBetweenDetections = 1.0f;


        private AudioSource audioSource;
        private int detectIndex;
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
    }
}