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
        public GameObject postProcessingObject;

        public bool skipIntro = false;


        private AudioSource audioSource;
        private FPSController fPSController;
        private float audioTime;

        // Start is called before the first frame update
        void Start()
        {
            Time.timeScale = 1;
            audioSource = GetComponent<AudioSource>();
            audioTime = startingAudio.length;
            fPSController = GetComponent<FPSController>();
            if(!skipIntro)
                StartCoroutine(IntroScript());

        }

        IEnumerator IntroScript()
        {
            // Disable display and control and start intro audio
            fPSController.InputControl(false);
            GameObject.Find("[UI]/Canvas/BlackoutPanel").GetComponent<Image>().color = new Color(0, 0, 0, 255);
            audioSource.PlayOneShot(startingAudio);

            Debug.Log(Time.time + " waiting for " + audioTime * percentAudioBeforemovement);
            yield return new WaitForSeconds(audioTime * percentAudioBeforemovement);
            postProcessingObject.GetComponent<constantDOFChanger>().SetDOF(true, 0);
            StartCoroutine(SlowClearBlackout(audioTime * (1 - percentAudioBeforemovement) / 2));


            Debug.Log(Time.time + " waiting for " + audioTime * (1 - percentAudioBeforemovement));
            yield return new WaitForSeconds(audioTime * (1 - percentAudioBeforemovement));
            postProcessingObject.GetComponent<constantDOFChanger>().SetDOF(false, 0);

            // Re-enable display and control    
            fPSController.InputControl(true);

        }


        IEnumerator SlowClearBlackout(float time)
        {
            //Time.timeScale = 0;
            // Go through all levels of alpha over set time
            for (int i = 0; i < 256; i++)
            {
                yield return new WaitForSeconds(time / 256);

                Debug.Log(Time.time + " set alpha " + (255-i));
                float alpha = (255.0f - i)/ 255.0f;
                GameObject.Find("[UI]/Canvas/BlackoutPanel").GetComponent<Image>().color = new Color(0, 0, 0, alpha);
            }

        }
    }
}