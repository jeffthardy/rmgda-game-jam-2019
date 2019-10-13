using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopZombies
{
    public class FinalDadScene : MonoBehaviour
    {
        public GameObject exitDoor1;
        public GameObject exitDoor2;
        public AudioClip dadGoodbyeClip;
        public float postAudioDoorDelay = 1.0f;

        private bool goodbyeHasHappened = false;


        private AudioSource dadGoodbyeAudioSource;
        public bool triggersNewSpawnPoint = true;
        private FPSController fPSController;


        // Start is called before the first frame update
        void Start()
        {
            dadGoodbyeAudioSource = GetComponent<AudioSource>();
            fPSController = GameObject.Find("Player").GetComponent<FPSController>();

            exitDoor1.GetComponent<DoorController>().DisableDoor();
            exitDoor2.GetComponent<DoorController>().DisableDoor();

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<FPSController>() != null)
            {
                if (!goodbyeHasHappened)
                {
                    //Debug.Log("Final Goodbyes");
                    StartCoroutine(PlayFinalDadWords());
                    if (triggersNewSpawnPoint)
                        fPSController.RecordNewSpawnPoint();
                }
            }
        }

        IEnumerator PlayFinalDadWords()
        {
            goodbyeHasHappened = true;
            dadGoodbyeAudioSource.PlayOneShot(dadGoodbyeClip);
            enableExit();
            yield return new WaitForSeconds(dadGoodbyeClip.length + postAudioDoorDelay);


        }

        void enableExit()
        {
            exitDoor1.GetComponent<DoorController>().EnableDoor();
            exitDoor2.GetComponent<DoorController>().EnableDoor();
            exitDoor1.GetComponent<DoorController>().Use();
            exitDoor2.GetComponent<DoorController>().Use();
        }
    }
}