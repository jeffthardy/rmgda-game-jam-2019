using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopZombies
{
    public class HidingRoomScript : MonoBehaviour
    {

        public GameObject entranceDoor1;

        public AudioClip finalFightSounds;
        public AudioClip gottaFindDadClip;
        public GameObject finalFightAudio;
        public float postAudioDoorDelay = 1.0f;
        public GameObject nextScene;

        public GameObject previousScene;
        public GameObject enemy;
        public GameObject dad;


        private AudioSource finalFightAudioSource;
        private bool fightHasHappened = false;

        public bool triggersNewSpawnPoint = true;
        private FPSController fPSController;


        // Start is called before the first frame update
        void Start()
        {
            finalFightAudioSource = finalFightAudio.GetComponent<AudioSource>();
            fPSController = GameObject.Find("Player").GetComponent<FPSController>();

            entranceDoor1.GetComponent<DoorController>().EnableDoor();
            if (!entranceDoor1.GetComponent<DoorController>().isOpen)
                entranceDoor1.GetComponent<DoorController>().Use();

            nextScene.SetActive(false);
            enemy.SetActive(false);
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

                    enemy.SetActive(true);
                    dad.SetActive(false);
                    StartCoroutine(PlayFinalFight());


                    if (triggersNewSpawnPoint)
                        fPSController.RecordNewSpawnPoint();


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
            finalFightAudioSource.PlayOneShot(gottaFindDadClip);
            nextScene.SetActive(true);
            previousScene.SetActive(false);
        }

        

    }
}