using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopZombies
{
    public class level1ExitEnable : MonoBehaviour
    {

        public GameObject entranceDoor1;
        public GameObject exitDoor1;
        public GameObject exitDoor2;

        public AudioClip finalFightSounds;
        public GameObject finalFightAudio;
        public float postAudioDoorDelay = 1.0f;


        private AudioSource finalFightAudioSource;
        private bool fightHasHappened = false;
        




        // Start is called before the first frame update
        void Start()
        {
            exitDoor1.GetComponent<DoorController>().DisableDoor();
            exitDoor2.GetComponent<DoorController>().DisableDoor();
            finalFightAudioSource = finalFightAudio.GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<FPSController>() != null)
            {
                if (!fightHasHappened)
                {
                    if (entranceDoor1.GetComponent<DoorController>().isOpen)
                        entranceDoor1.GetComponent<DoorController>().Use();
                    entranceDoor1.GetComponent<DoorController>().DisableDoor();

                    StartCoroutine(PlayFinalFight());
                }
            }
        }
        IEnumerator PlayFinalFight()
        {
            fightHasHappened = true;
            finalFightAudioSource.PlayOneShot(finalFightSounds);
            yield return new WaitForSeconds(finalFightSounds.length + postAudioDoorDelay);

            entranceDoor1.GetComponent<DoorController>().EnableDoor();
            entranceDoor1.GetComponent<DoorController>().Use();
            enableExit();

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