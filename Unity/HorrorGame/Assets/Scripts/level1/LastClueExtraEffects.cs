using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopZombies
{

    public class LastClueExtraEffects : MonoBehaviour
    {
        public DoorController kitchenDoor;
        public DoorController livingRoomDoor;

        private Light flashingLight;
        private AudioSource scareAudioSource;
        private PlaySeriesOfAudioClips playSeriesOfAudioClips;

        // Start is called before the first frame update
        void Start()
        {
            scareAudioSource = GetComponent<AudioSource>();
            flashingLight = GetComponentInChildren<Light>();
            playSeriesOfAudioClips = GetComponent<PlaySeriesOfAudioClips>();
            flashingLight.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void TriggerEndSequence()
        {
            if (kitchenDoor.GetComponent<DoorController>().isOpen)
                kitchenDoor.GetComponent<DoorController>().Use();
            kitchenDoor.GetComponent<DoorController>().DisableDoor();

            if (livingRoomDoor.GetComponent<DoorController>().isOpen)
                livingRoomDoor.GetComponent<DoorController>().Use();
            livingRoomDoor.GetComponent<DoorController>().DisableDoor();

            StartCoroutine(FlashingLightsAndDoor());
        }

        IEnumerator FlashingLightsAndDoor()
        {
            playSeriesOfAudioClips.PlaySeries();
            StartCoroutine(FlashLights());
            yield return new WaitForSeconds(playSeriesOfAudioClips.GetClipsLength());

            livingRoomDoor.GetComponent<DoorController>().EnableDoor();
            livingRoomDoor.GetComponent<DoorController>().Use();
        }

        IEnumerator FlashLights()
        {
            var startTime = Time.time;
            while (Time.time < startTime + playSeriesOfAudioClips.GetClipsLength())
            {
                var lightTime = Random.Range(0.1f, 0.5f);
                flashingLight.enabled = !flashingLight.enabled;
                yield return new WaitForSeconds(lightTime);
            }
            flashingLight.enabled = false;
        }
    }
}