using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TopZombies
{
    public class Level1Intro : MonoBehaviour
    {
        public AudioClip startingAudio;
        public float percentAudioBeforemovement = 0.9f;


        private AudioSource audioSource;
        private FPSController fPSController;
        private float audioTime;

        // Start is called before the first frame update
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            audioTime = startingAudio.length;
            fPSController = GetComponent<FPSController>();
            StartCoroutine(IntroScript());

        }

        IEnumerator IntroScript()
        {
            // Disable display and control and start intro audio
            fPSController.InputControl(false);
            GameObject.Find("[UI]/Canvas/BlackoutPanel").GetComponent<Image>().color = new Color(0, 0, 0, 255);
            audioSource.PlayOneShot(startingAudio);

            yield return new WaitForSeconds(audioTime* percentAudioBeforemovement);

            // Re-enable display and control
            GameObject.Find("[UI]/Canvas/BlackoutPanel").GetComponent<Image>().color = new Color(0, 0, 0, 0);            
            fPSController.InputControl(true);

        }
        
    }
}