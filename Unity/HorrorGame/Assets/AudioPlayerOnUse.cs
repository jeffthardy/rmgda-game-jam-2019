using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopZombies
{

    public class AudioPlayerOnUse : MonoBehaviour
    {
        public AudioClip gameAudioData;
        AudioSource Audio;

        // Start is called before the first frame update
        void Start()
        {
            Audio = this.GetComponent<AudioSource>();
            Audio.clip = gameAudioData;

        }

        // Update is called once per frame
        void Update()
        {

        }


        public void Use()
        {
            Debug.Log("Playing audio "+ gameAudioData);
            Audio.Pause();
            Audio.Play(0);
        }
    }

}