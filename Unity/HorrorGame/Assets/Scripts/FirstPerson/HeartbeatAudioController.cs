using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TopZombies
{
    public class HeartbeatAudioController : MonoBehaviour
    {
        public AudioClip heartbeat;

        public float rateAtSafe = 1.0f;
        public float rateAtNightmare = 0.5f;
        public float rateAtEnemy = 0.25f;
        public float volumeAtSafe = 0.1f;
        public float volumeAtNightmare = 0.2f;
        public float volumeAtEnemy = 0.3f;


        private AudioSource audioSource;
        private NightmareController nightmareController;
        private ShakeObjectOnTrigger shakeObjectOnTrigger;

        private float nextBeatTime = 0;
        private float nextVolume = 0;


        // Start is called before the first frame update
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            nightmareController =  GameObject.Find("NightmareController").GetComponent<NightmareController>();
            shakeObjectOnTrigger = GameObject.Find("Player/EnemyDetector").GetComponent<ShakeObjectOnTrigger>();
            nextVolume = 0.0f;


        }

        // Update is called once per frame
        void Update()
        {
            if(Time.realtimeSinceStartup > nextBeatTime)
            {

                if (shakeObjectOnTrigger.isTriggered)
                {
                    nextBeatTime = Time.realtimeSinceStartup + rateAtEnemy;
                    nextVolume = volumeAtEnemy;
                }
                else if (nightmareController.nightmareMode)
                {
                    nextBeatTime = Time.realtimeSinceStartup + rateAtNightmare;
                    nextVolume = volumeAtNightmare;
                }
                else
                {
                    nextBeatTime = Time.realtimeSinceStartup + rateAtSafe;
                    nextVolume = volumeAtSafe;
                }

                audioSource.PlayOneShot(heartbeat, nextVolume);
            }

        }
    }
}