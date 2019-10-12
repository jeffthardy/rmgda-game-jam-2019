using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TopZombies
{
    public class EnableChildrenOnPlayer : MonoBehaviour
    {

        private NightmareController nightmareController;
        // Start is called before the first frame update
        void Start()
        {
            nightmareController = GameObject.Find("NightmareController").GetComponent<NightmareController>();

            // give starting state of off, so we can keep them on in the editor?
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<FPSController>() != null)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (!nightmareController.nightmareMode)
            {
                if (other.gameObject.GetComponent<FPSController>() != null)
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        transform.GetChild(i).gameObject.SetActive(true);
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.GetComponent<FPSController>() != null)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
    }
}