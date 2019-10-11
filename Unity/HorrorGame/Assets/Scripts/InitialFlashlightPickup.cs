using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopZombies {
    public class InitialFlashlightPickup : MonoBehaviour
    {
        public GameObject playerFlashlight;

        // Start is called before the first frame update
        void Start()
        {

        }
        public void Use()
        {
            playerFlashlight.GetComponent<FlashlightController>().makeUseable();
            transform.gameObject.SetActive(false);
        }
    }
}