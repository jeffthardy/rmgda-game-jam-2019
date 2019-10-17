using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopZombies {
    public class InitialFlashlightPickup : MonoBehaviour
    {
        public GameObject playerFlashlight;
        public GameObject onScreenInstructions;

        // Start is called before the first frame update
        public void ShowInstructions()
        {
            onScreenInstructions.SetActive(true);
        }
        public void Use()
        {
            playerFlashlight.GetComponent<FlashlightController>().makeUseable();
            transform.gameObject.SetActive(false);
            onScreenInstructions.SetActive(false);
        }
    }
}
