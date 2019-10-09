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
    }
}