using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopZombies
{
    public class SpectreVisionController : MonoBehaviour
    {

        private Renderer spectreRenderer;
        // Start is called before the first frame update
        void Start()
        {
            spectreRenderer = GetComponentInChildren<Renderer>();

            spectreRenderer.enabled = false;
        }


        public void DisplaySpectreForTime(float time)
        {
            StartCoroutine(DisplaySpectreForTimeDelay(time));
        }


        IEnumerator DisplaySpectreForTimeDelay(float time)
        {
            spectreRenderer.enabled = true;
            yield return new WaitForSeconds(time);
            spectreRenderer.enabled = false;
        }
        
    }
}