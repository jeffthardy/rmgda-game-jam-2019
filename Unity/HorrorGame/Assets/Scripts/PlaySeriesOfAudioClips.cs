using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopZombies
{
    public class PlaySeriesOfAudioClips : MonoBehaviour
    {


        public AudioClip[] clipSeries;
        public float[] seriesDelay;

        private AudioSource audioSource;

        // Start is called before the first frame update
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void PlaySeries()
        {
            StartCoroutine(PlaySeriesWithDelays());
        }

        public float GetClipLength(int i)
        {
            if(i >=0 && i < clipSeries.Length)
            {
                return clipSeries[i].length;
            }
            return 0;
        }
        public float GetClipsLength()
        {
            float timeTotal = 0;
            for (int i = 0; i < clipSeries.Length;i++)
            {
                timeTotal += clipSeries[i].length;
            }
            return timeTotal;
        }

        IEnumerator PlaySeriesWithDelays()
        {
            for (int i = 0; i < clipSeries.Length; i++)
            {
                audioSource.PlayOneShot(clipSeries[i]);
                yield return new WaitForSeconds(clipSeries[i].length + seriesDelay[i]);
                Debug.Log(Time.time + " : playing clip " + i);


            }
        }


    }
}