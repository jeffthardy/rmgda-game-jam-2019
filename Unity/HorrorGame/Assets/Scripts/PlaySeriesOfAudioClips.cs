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

        private enum State
        {
            Stopped,
            Playing,
            Paused,
        }
        private State state = State.Stopped;

        // Start is called before the first frame update
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {

        }
        private void OnEnable()
        {
            audioSource = GetComponent<AudioSource>();
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
            state = State.Playing;

            for (int i = 0; i < clipSeries.Length; i++)
            {
                Debug.Log(Time.time + " : playing clip " + i);
                var remainingTime = clipSeries[i].length;
                audioSource.PlayOneShot(clipSeries[i]);
                while (remainingTime > 0)
                {
                    if (state == State.Stopped)
                    {
                        yield break;
                    }
                    if (state == State.Playing)
                    {
                        remainingTime -= Time.deltaTime;
                    }
                    yield return null;
                }
                yield return new WaitForSeconds(seriesDelay[i]);
            }

            state = State.Stopped;
        }

        private void Pause()
        {
            if (state == State.Playing)
            {
                audioSource.Pause();
                state = State.Paused;
            }
        }

        private void Unpause()
        {
            if (state == State.Paused)
            {
                audioSource.UnPause();
                state = State.Playing;
            }
        }

        private void Stop()
        {
            audioSource.Stop();
            state = State.Stopped;
        }

        public static void PauseAll()
        {
            foreach (var series in FindObjectsOfType<PlaySeriesOfAudioClips>())
            {
                series.Pause();
            }
        }

        public static void UnpauseAll()
        {
            foreach (var series in FindObjectsOfType<PlaySeriesOfAudioClips>())
            {
                series.Unpause();
            }
        }

        public static void StopAll()
        {
            foreach (var series in FindObjectsOfType<PlaySeriesOfAudioClips>())
            {
                series.Stop();
            }
        }
    }
}
