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
        public float timeBeforeSceenBlack = 1.0f;
        public float timeBeforeSceenClear = 0.5f;

        private AudioSource audioSource;
        private FPSController fPSController;


        private float startTime;
        private int deathIndex;
        private int talkIndex;
        private GameObject enemiesHolder;

        private float canDieTime;
        private float minTimeToDie = 2.0f;


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
            //GameObject.Find("[UI]/Canvas2/BlackoutPanel").GetComponent<Image>().color = new Color(0, 0, 0, 0);
            enemiesHolder = GameObject.Find("[Enemies]");

            canDieTime = Time.time + minTimeToDie;
        }

        public void Death()
        {
            //Black screen

            if (Time.time > canDieTime)
            {
                canDieTime = Time.time + minTimeToDie;
                //GameObject.Find("[UI]/Canvas2/BlackoutPanel").GetComponent<Image>().color = new Color(0, 0, 0, 255);
                GetComponent<FPSController>().InputControl(false);

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

                // Play new spawn audio
                if (spawnTalks.Length > 1)
                {
                    int previousIndex = talkIndex;
                    while (talkIndex == previousIndex)
                        talkIndex = Random.Range(0, spawnTalks.Length - 1);
                }
                else
                {
                    talkIndex = 0;
                }

                PlaySeriesOfAudioClips.RestartAll(deathScreams[deathIndex].length + timeBeforeSceenBlack * 2 + spawnTalks[talkIndex].length + 1.0f);

                StartCoroutine(DeathCleanup());

            }
        }

        IEnumerator DeathCleanup()
        {

            audioSource.PlayOneShot(deathScreams[deathIndex]);
            startTime = Time.time;

            // Go through all levels of alpha over set time
            for (int i = 0; i < 256; i+=4)
            {
                yield return new WaitForSeconds(timeBeforeSceenBlack / 64);

                //Debug.Log(Time.time + " set alpha " + (i));
                float alpha = i/255.0f ;
                GameObject.Find("[UI]/Canvas2/BlackoutPanel").GetComponent<Image>().color = new Color(0, 0, 0, alpha);
            }

            //Debug.Log(Time.time + "yielding");
            //wait for audio
            if (Time.time < startTime + deathScreams[deathIndex].length) {
                var waitTime = deathScreams[deathIndex].length + startTime - Time.time;
                yield return new WaitForSeconds(waitTime);
            }

            // Play new spawn audio
            audioSource.PlayOneShot(spawnTalks[talkIndex]);
            //Debug.Log(Time.time + "spawnaudio");

            //Respawn all enemies if alive
            for (int i = 0; i < enemiesHolder.transform.childCount; i++)
            {
                var child = enemiesHolder.transform.GetChild(i).gameObject;
                if ((child != null) && child.activeSelf)
                    if (child.GetComponentInChildren<enemyMover>() != null)
                    {
                        child.GetComponentInChildren<enemyMover>().RespawnAtStart();
                    } else
                    {
                        // Case where enemies are one layer deep under another group
                        for (int j = 0; j < child.transform.childCount; j++)
                        {
                            var childsChild = child.transform.GetChild(j).gameObject;
                            if ((childsChild != null) && childsChild.activeSelf)
                            {
                                if (childsChild.GetComponentInChildren<enemyMover>() != null)
                                {
                                    childsChild.GetComponentInChildren<enemyMover>().RespawnAtStart();
                                }
                            }
                        }

                    }
            }            

            //Respawn Character
            fPSController.Respawn();

            yield return new WaitForSeconds(spawnTalks[talkIndex].length);


            // Go through all levels of alpha over set time
            for (int i = 255; i >0; i-=4)
            {
                yield return new WaitForSeconds(timeBeforeSceenClear / 64);

                //Debug.Log(Time.time + " set alpha " + (i));
                float alpha = (i) / 255.0f;
                GameObject.Find("[UI]/Canvas2/BlackoutPanel").GetComponent<Image>().color = new Color(0, 0, 0, alpha);
            }


            //Clear screen
            GetComponent<FPSController>().InputControl(true);
            //GameObject.Find("[UI]/Canvas2/BlackoutPanel").GetComponent<Image>().color = new Color(0, 0, 0, 0); ;
            //Debug.Log(Time.time + "screen cleared");

        }

    }
}
