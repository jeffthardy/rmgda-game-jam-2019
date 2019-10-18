using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopZombies
{
    public class HidingRoomScript : MonoBehaviour
    {

        public GameObject entranceDoor1;

        public AudioClip finalFightStruggle;
        public AudioClip finalFightGasp;
        public AudioClip gottaFindDadClip;
        public GameObject finalFightAudio;
        public float postAudioDoorDelay = 1.0f;
        public GameObject nextScene;

        public GameObject previousScene;
        public GameObject enemy;
        public GameObject dad;
        public GameObject guestBedroomInfoUI;


        private AudioSource finalFightAudioSource;
        private bool fightHasHappened = false;
        private Light fightLight;

        public bool triggersNewSpawnPoint = true;
        private FPSController fPSController;


        // Start is called before the first frame update
        void Start()
        {
            finalFightAudioSource = finalFightAudio.GetComponent<AudioSource>();
            fPSController = GameObject.Find("Player").GetComponent<FPSController>();
            fightLight = GetComponentInChildren<Light>();

            entranceDoor1.GetComponent<DoorController>().EnableDoor();
            if (!entranceDoor1.GetComponent<DoorController>().isOpen)
                entranceDoor1.GetComponent<DoorController>().Use();

            nextScene.SetActive(false);
            enemy.SetActive(false);
            fightLight.enabled = false;
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


                    guestBedroomInfoUI.SetActive(false);
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
            StartCoroutine(FlashLights());
            finalFightAudioSource.PlayOneShot(finalFightStruggle);
            yield return new WaitForSeconds(finalFightStruggle.length);
            finalFightAudioSource.PlayOneShot(finalFightGasp);
            yield return new WaitForSeconds(finalFightGasp.length + postAudioDoorDelay);

            entranceDoor1.GetComponent<DoorController>().EnableDoor();
            entranceDoor1.GetComponent<DoorController>().Use();
            finalFightAudioSource.PlayOneShot(gottaFindDadClip);
            nextScene.SetActive(true);
            previousScene.SetActive(false);
        }


        IEnumerator FlashLights()
        {
            var startTime = Time.time;
            while(Time.time < startTime + finalFightStruggle.length)
            {
                var lightTime = Random.Range(0.1f, 0.2f);
                fightLight.enabled = !fightLight.enabled;
                yield return new WaitForSeconds(lightTime);
            }
            fightLight.enabled = true;

            yield return new WaitForSeconds(finalFightGasp.length);
            fightLight.enabled = false;
        }
    }
}