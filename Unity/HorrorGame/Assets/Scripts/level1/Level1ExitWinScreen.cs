using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

namespace TopZombies
{
    public class Level1ExitWinScreen : MonoBehaviour
    {
        public AudioClip winningNewsStory;
        public AudioClip winnersMusic;
        public GameObject enemiesHolder;
        
        private bool hasWon = false;


        private FPSController fPSController;
        
        private AudioSource audioSource;
        // Start is called before the first frame update
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            fPSController = GameObject.Find("Player").GetComponent<FPSController>();
            GameObject.Find("[UI]/Canvas/GameCredits").SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<FPSController>() != null)
            {
                if (!hasWon)
                {
                    hasWon = true;

                    audioSource.PlayOneShot(winnersMusic, 0.5f);
                    audioSource.PlayOneShot(winningNewsStory);

                    // Disable any existing enemies
                    for (int i = 0; i < enemiesHolder.transform.childCount; i++)
                    {
                        var child = enemiesHolder.transform.GetChild(i).gameObject;
                        if (child != null)
                            child.SetActive(false);
                    }
                    

                    StartCoroutine(FadeToWhite(5.0f));


                }
            }
        }

        IEnumerator FadeToWhite(float time)
        {
            fPSController.InputControl(false);
            // Give mouse back to user
            Cursor.lockState = fPSController.cursorLockedMode = CursorLockMode.None;
            Cursor.visible = (CursorLockMode.Locked != fPSController.cursorLockedMode);


            // Go through all levels of alpha over set time
            for (int i = 0; i < 256; i++)
            {
                yield return new WaitForSeconds(time / 256);

                Debug.Log(Time.time + " set alpha " + (i));
                float alpha = (i) / 255.0f;
                GameObject.Find("[UI]/Canvas/BlackoutPanel").GetComponent<Image>().color = new Color(1, 1, 1, alpha);
            }
            GameObject.Find("[UI]/Canvas/GameCredits").SetActive(true);


        }
    }
}