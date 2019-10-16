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
        private BackgroundMusicController backgroundMusicController;
        private BoxCollider exitCollider;
        private Renderer childRender;

        private AudioSource audioSource;
        // Start is called before the first frame update
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            fPSController = GameObject.Find("Player").GetComponent<FPSController>();
            exitCollider = GetComponent<BoxCollider>();
            childRender = GetComponentInChildren<Renderer>();
            GameObject.Find("[UI]/Canvas/GameCredits").SetActive(false);
            backgroundMusicController = GameObject.Find("Player/BGMusic").GetComponent<BackgroundMusicController>();

            childRender.enabled = false;
            exitCollider.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void EnableExit()
        {
            childRender.enabled = true;
            exitCollider.enabled = true;
        }

        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<FPSController>() != null)
            {
                if (!hasWon)
                {
                    hasWon = true;

                    backgroundMusicController.PlayMusic(BackgroundMusicController.MusicTypes.win);
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