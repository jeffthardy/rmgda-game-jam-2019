using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TopZombies
{
    public class PlayerDeathEffects : MonoBehaviour
    {

        public AudioClip[] deathScreams;
        public AudioClip[] spawnTalks;
        public float innerAudioGap = 0.5f;
        public float talkTimeBeforeSceenClear = 0.5f;

        private AudioSource audioSource;
        private FPSController fPSController;


        private float startTime;
        private int deathIndex;
        private int talkIndex;


        // Start is called before the first frame update
        void Start()
        {
            if (deathScreams.Length == 0)
            {
                Debug.Log("Warning there are no death screams set!");
            }
            if (spawnTalks.Length == 0)
            {
                Debug.Log("Warning there are no death talks set!");
            }
            audioSource = GetComponent<AudioSource>();

            fPSController = GetComponent<FPSController>();
            GameObject.Find("[UI]/Canvas/BlackoutPanel").GetComponent<Image>().color = new Color(0, 0, 0, 0);
        }
        
        public void Death()
        {
            //Black screen
            GameObject.Find("[UI]/Canvas/BlackoutPanel").GetComponent<Image>().color = new Color(0, 0, 0, 255);

            //Play new scream
            if (deathScreams.Length > 1)
            {
                int previousIndex = deathIndex;
                while (deathIndex == previousIndex)
                    deathIndex = Random.Range(0, deathScreams.Length - 1);
            }
            else
            {
                deathIndex = 0;
            }
            audioSource.PlayOneShot(deathScreams[deathIndex]);
            startTime = Time.time;

            StartCoroutine(DeathCleanup());

            //Clear screen and respawn
            fPSController.Respawn();
            //Debug.Log(Time.time + "Respawned");

        }
        IEnumerator DeathCleanup()
        {
            //Debug.Log(Time.time + "yielding");
            //wait for audio
            yield return new WaitForSeconds(deathScreams[deathIndex].length + innerAudioGap);

            // Play new spawn audio
            if (spawnTalks.Length > 1)
            {
                int previousIndex = talkIndex;
                while (talkIndex == previousIndex)
                    talkIndex = Random.Range(0, spawnTalks.Length - 1);
            } else
            {
                talkIndex = 0;
            }
            audioSource.PlayOneShot(spawnTalks[talkIndex]);
            //Debug.Log(Time.time + "spawnaudio");

            
            yield return new WaitForSeconds(talkTimeBeforeSceenClear);

            //Clear screen
            GameObject.Find("[UI]/Canvas/BlackoutPanel").GetComponent<Image>().color = new Color(0, 0, 0, 0); ;
            //Debug.Log(Time.time + "screen cleared");

        }

    }
}