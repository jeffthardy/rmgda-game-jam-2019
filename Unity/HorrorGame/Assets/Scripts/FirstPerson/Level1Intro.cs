using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TopZombies
{
    public class Level1Intro : MonoBehaviour
    {
        public float percentAudioBeforemovement = 0.9f;
        public GameObject postProcessingObject;
        public Animator viewAnimator;
        public GameObject animatedTarget;

        public GameObject firstClue;

        public bool skipIntro = false;


        private AudioSource audioSource;
        private FPSController fPSController;
        private float audioTime;
        private PlaySeriesOfAudioClips playSeriesOfAudioClips;
        private NightmareController nightmareController;

        // Start is called before the first frame update
        void Start()
        {
            Time.timeScale = 1;
            audioSource = GetComponent<AudioSource>();
            fPSController = GetComponent<FPSController>();
            playSeriesOfAudioClips = GetComponent<PlaySeriesOfAudioClips>();
            nightmareController = GameObject.Find("NightmareController").GetComponent<NightmareController>();
            if (!skipIntro)
                StartCoroutine(Level1IntroScript());


        }


        IEnumerator Level1IntroScript()
        {
            // Disable display and control and start intro audio
            fPSController.InputControl(false);
            fPSController.CameraTarget(animatedTarget);
            GameObject.Find("[UI]/Canvas/BlackoutPanel").GetComponent<Image>().color = new Color(0, 0, 0, 255);
            //Wait a tiny bit to let things get out of start
            yield return new WaitForSeconds(0.1f);

            // Disable first clue
            firstClue.SetActive(false);

            // Line 7-19
            playSeriesOfAudioClips.PlaySeries();
            
            float clipLength = playSeriesOfAudioClips.GetClipLength(0);

            Debug.Log(Time.time + " waiting for " + clipLength * percentAudioBeforemovement);
            yield return new WaitForSeconds(clipLength * percentAudioBeforemovement);
            postProcessingObject.GetComponent<constantDOFChanger>().SetDOF(true, 0);
            StartCoroutine(SlowClearBlackout(clipLength * (1 - percentAudioBeforemovement) / 2));
            viewAnimator.SetTrigger("PlayView");


            Debug.Log(Time.time + " waiting for " + clipLength * (1 - percentAudioBeforemovement));
            yield return new WaitForSeconds(clipLength * (1 - percentAudioBeforemovement));
            postProcessingObject.GetComponent<constantDOFChanger>().SetDOF(false, 0);

            // Disable input until 2nd clip is done
            // Line 8
            clipLength = playSeriesOfAudioClips.GetClipLength(1);
            yield return new WaitForSeconds(clipLength);

            // Re-enable display and control    
            fPSController.ResetMouseView();
            fPSController.InputControl(true);

            // Line 9
            clipLength = playSeriesOfAudioClips.GetClipLength(2);
            yield return new WaitForSeconds(clipLength);
            
            // Line 10
            clipLength = playSeriesOfAudioClips.GetClipLength(3);
            yield return new WaitForSeconds(clipLength);



            // Line 11-13
            clipLength = playSeriesOfAudioClips.GetClipLength(4) + playSeriesOfAudioClips.GetClipLength(5) + playSeriesOfAudioClips.GetClipLength(6);

            //Flicker Lights and shake camera here
            StartCoroutine(FlickerLightsForTime(clipLength));

            yield return new WaitForSeconds(clipLength);


            //Full Nightmare here
            nightmareController.SwitchToNightmare();

            // Line 14-16
            clipLength = playSeriesOfAudioClips.GetClipLength(7) + playSeriesOfAudioClips.GetClipLength(8) + playSeriesOfAudioClips.GetClipLength(9);
            yield return new WaitForSeconds(clipLength);


            //Normal again here
            nightmareController.SwitchToDream();

            // Line 17-20
            clipLength = playSeriesOfAudioClips.GetClipLength(10) + playSeriesOfAudioClips.GetClipLength(11) + playSeriesOfAudioClips.GetClipLength(12) + +playSeriesOfAudioClips.GetClipLength(13);
            yield return new WaitForSeconds(clipLength);

            firstClue.SetActive(true);


        }

        
        IEnumerator FlickerLightsForTime(float timeRequested)
        {
            float totaTimeSpent = 0;
            bool lightsOn = true;

            yield return new WaitForSeconds(0.1f);
            Debug.Log(Time.time + " : Need to wait for " + timeRequested);

            while (totaTimeSpent < timeRequested)
            {
                float randTime = Random.Range(0.0f, 1.0f);

                if (!lightsOn)
                {
                    lightsOn = true;
                    nightmareController.SetGlobalLightActive(true);
                    Debug.Log(Time.time + " : Turning lights on");
                }
                else
                {
                    lightsOn = false;
                    nightmareController.SetGlobalLightActive(false);
                    Debug.Log(Time.time + " : Turning lights off");
                }
                yield return new WaitForSeconds(randTime);
                totaTimeSpent += randTime;
            }
        }

        IEnumerator SlowClearBlackout(float time)
        {
            //Time.timeScale = 0;
            // Go through all levels of alpha over set time
            for (int i = 0; i < 256; i++)
            {
                yield return new WaitForSeconds(time / 256);

                //Debug.Log(Time.time + " set alpha " + (255-i));
                float alpha = (255.0f - i)/ 255.0f;
                GameObject.Find("[UI]/Canvas/BlackoutPanel").GetComponent<Image>().color = new Color(0, 0, 0, alpha);
            }

        }
    }
}