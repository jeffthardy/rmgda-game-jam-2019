using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopZombies
{
    public class ChaseToHidingSpot : MonoBehaviour
    {
        public GameObject blockers;
        public GameObject nextScene;


        private bool chaseHasHappened = false;



        public GameObject chaseDoor;

        // Start is called before the first frame update
        void Start()
        {
            blockers.SetActive(false);
        }
        

        // Update is called once per frame
        void Update()
        {

        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<FPSController>() != null)
            {
                if (!chaseHasHappened)
                {
                    blockers.SetActive(true);
                    if (!chaseDoor.GetComponent<DoorController>().isOpen)
                        chaseDoor.GetComponent<DoorController>().Use();

                    StartCoroutine(PlayChase());

                }
            }
        }
        IEnumerator PlayChase()
        {
            chaseHasHappened = true;

            yield return new WaitForSeconds(1.0f);
            // Add enemy with chase and direction

            nextScene.SetActive(true);
        }
    }
}