using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace TopZombies
{
    public class Level0Intro : MonoBehaviour
    {
        public string nextScene = "JeffsScene";
        private PlaySeriesOfAudioClips playSeriesOfAudioClips;
        // Start is called before the first frame update
        void Start()
        {
            Time.timeScale = 1;
            playSeriesOfAudioClips = GetComponent<PlaySeriesOfAudioClips>();
            StartCoroutine(DoctorOfficeIntro());
        }

        IEnumerator DoctorOfficeIntro()
        {

            yield return new WaitForSeconds(0.1f);
            playSeriesOfAudioClips.PlaySeries();

            float clipsLength = playSeriesOfAudioClips.GetClipsLength();
            yield return new WaitForSeconds(clipsLength);
            SceneManager.LoadScene(nextScene);
        }


        private void Update()
        {
            // Allow user to skip intro
            if (Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Q))
            {
                SceneManager.LoadScene(nextScene);
            }
            //Debug.Log(Time.time + " , timescale " + Time.timeScale);

        }
    }
}