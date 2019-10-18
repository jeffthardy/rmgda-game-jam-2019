using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TopZombies
{
    public class ChaseToHidingSpot : MonoBehaviour
    {
        public GameObject blockers;
        public GameObject nextScene;
        public GameObject dad;
        public GameObject ghost;
        public GameObject cameraLocation;
        public GameObject guestBedroomInfoUI;


        private bool chaseHasHappened = false;



        public GameObject chaseDoor;
        public GameObject kitchenDoor;
        public GameObject animatedTarget;
        public Animator viewAnimator;
        private FPSController fPSController;

        private Vector3 originalCameraLocation;

        // Start is called before the first frame update
        void Start()
        {
            blockers.SetActive(false);
            dad.SetActive(false);
            ghost.SetActive(false);
            fPSController = GameObject.Find("Player").GetComponent<FPSController>();
            guestBedroomInfoUI.SetActive(false);
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
                    dad.SetActive(true);
                    if (!kitchenDoor.GetComponent<DoorController>().isOpen)
                        kitchenDoor.GetComponent<DoorController>().Use();
                    if (!chaseDoor.GetComponent<DoorController>().isOpen)
                        chaseDoor.GetComponent<DoorController>().Use();

                    fPSController.InputControl(false);
                    originalCameraLocation = Camera.main.transform.position;
                    Camera.main.transform.position = cameraLocation.transform.position;
                    fPSController.CameraTarget(animatedTarget);
                    StartCoroutine(PlayChase());

                }
            }
        }
        IEnumerator PlayChase()
        {
            chaseHasHappened = true;

            viewAnimator.SetTrigger("Play");
            dad.GetComponent<PlaySeriesOfAudioClips>().PlaySeries();
            dad.GetComponent<NavMeshAgent>().enabled = false;
            yield return new WaitForSeconds(5.0f);
            dad.GetComponent<NavMeshAgent>().enabled = true;
            fPSController.ResetMouseView();
            Camera.main.transform.position = originalCameraLocation;
            fPSController.InputControl(true);

            // Add enemy with chase and direction
            ghost.SetActive(true);
            guestBedroomInfoUI.SetActive(true);

            nextScene.SetActive(true);
        }
    }
}