using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TopZombies
{
    public class BackgroundMusicController : MonoBehaviour
    {
        public AudioClip normalMusic;
        public AudioClip nightmareMusic;
        public AudioClip winMusic;
        public AudioClip menuMusic;

        public float maxVolume = 0.5f;

        private AudioSource audioSource;
        public enum MusicTypes { normal, nightmare, win, menu };

        public bool PlayOnStart = false;
        public MusicTypes startType;


        // Start is called before the first frame update
        void Start()
        {
            audioSource = GetComponent<AudioSource>();

            if(PlayOnStart)
            {
                PlayMusic(startType);
            }
        }


        public void PlayMusic(MusicTypes type)
        {
            audioSource.Stop();
            audioSource.loop = true;
            audioSource.volume = maxVolume;
            switch (type)
            {
                case MusicTypes.normal:
                    audioSource.clip = normalMusic;
                    break;
                case MusicTypes.nightmare:
                    audioSource.clip = nightmareMusic;
                    break;
                case MusicTypes.win:
                    audioSource.clip = winMusic;
                    break;
                case MusicTypes.menu:
                    audioSource.clip = menuMusic;
                    break;
                default:
                    audioSource.clip = normalMusic;
                    break;
            }
            audioSource.Play();
        }
    }
}