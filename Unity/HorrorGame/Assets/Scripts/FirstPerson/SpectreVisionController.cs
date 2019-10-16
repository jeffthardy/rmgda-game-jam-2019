using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopZombies
{
    public class SpectreVisionController : MonoBehaviour
    {
        public GameObject nextClue;

        private Renderer spectreRenderer;
        private CameraTracker clueCamera;
        // Start is called before the first frame update
        void Start()
        {
            spectreRenderer = GetComponentInChildren<Renderer>();
            clueCamera = GameObject.Find("Player/ClueCamera").GetComponent<CameraTracker>();

            spectreRenderer.enabled = false;
        }


        public void DisplaySpectreForTime(float time)
        {
            StartCoroutine(DisplaySpectreForTimeDelay(time));
        }


        IEnumerator DisplaySpectreForTimeDelay(float time)
        {
            // Restore to previous state, which was probably disabled.  Could generate race condition...
            //var wasEnabled = clueCamera.isEnabled;
            clueCamera.EnableCamera(true);
            spectreRenderer.enabled = true;
            yield return new WaitForSeconds(0.5f);

            var playSeriesOfAudioClips = GetComponent<PlaySeriesOfAudioClips>();
            if (playSeriesOfAudioClips != null)
            {
                playSeriesOfAudioClips.PlaySeries();
            }


            yield return new WaitForSeconds(playSeriesOfAudioClips.GetClipsLength()/2.0f);
            spectreRenderer.enabled = false;
            clueCamera.EnableCamera(false);

            if (!nextClue.activeSelf)
            {
                nextClue.SetActive(true);
            }

            if (nextClue.GetComponent<ClueController>() != null)
            {
                nextClue.GetComponent<ClueController>().enableClue();
            }
        }
        
    }
}