using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

namespace TopZombies
{
    public class PlaySeriesOfAudioClips : MonoBehaviour
    {


        public AudioClip[] clipSeries;
        public float[] seriesDelay;
        public UnityEvent[] clipAction;

        private AudioSource audioSource;
        private float restartDelay;

        private enum State
        {
            Stopped,
            Playing,
            Paused,
            Restarting,
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

        IEnumerator PlaySeriesWithDelays(float initialDelay = 0.0f)
        {
            state = State.Playing;

            if (initialDelay > 0.0f)
            {
                yield return new WaitForSeconds(initialDelay);
            }

            for (int i = 0; i < clipSeries.Length; i++)
            {
                Debug.Log(Time.time + " : playing clip " + i);

                audioSource.PlayOneShot(clipSeries[i]);
                if (state == State.Paused)
                {
                    audioSource.Pause();
                }

                var remainingTime = clipSeries[i].length;
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
                    if (state == State.Restarting)
                    {
                        break;
                    }
                    yield return null;
                }

                //Restart at begining of current clip
                if (state == State.Restarting)
                {
                    state = State.Playing;
                    audioSource.Stop();
                    yield return new WaitForSeconds(restartDelay);
                    --i;
                    continue;
                }

                if (clipAction.Length > i && clipAction[i] != null)
                {
                    clipAction[i].Invoke();
                }

                yield return new WaitForSeconds(seriesDelay[i]);
            }

            state = State.Stopped;
        }

        public void Pause()
        {
            if (state == State.Playing)
            {
                audioSource.Pause();
                state = State.Paused;
            }
        }

        public void Unpause()
        {
            if (state == State.Paused)
            {
                audioSource.UnPause();
                state = State.Playing;
            }
        }

        public void Stop()
        {
            audioSource.Stop();
            state = State.Stopped;
        }

        public void Restart(float delay)
        {
            if (state == State.Playing)
            {
                restartDelay = delay;
                state = State.Restarting;
            }
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

        public static void RestartAll(float delay)
        {
            foreach (var series in FindObjectsOfType<PlaySeriesOfAudioClips>())
            {
                series.Restart(delay);
            }
        }
    }
}
