using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopZombies {
    public class ClueController : MonoBehaviour
    {
        public bool isEnabled = false;
        public AudioClip useSound;
        public int uses = 1;
        public int useCount = 0;
        public float timeDisabledAfterUse = 0.5f;

        public bool triggersNightmareMode = false;
        public int audioClipsBeforeTrigger = 0;
        public bool triggersDreamMode = false;
        public GameObject blockage;
        public bool enablesClue = true;
        public GameObject nextClue;
        public float minTimeToHoldItem = 1.0f;
        public bool triggersNewSpawnPoint = true;
        public bool spawnsEnemy=false;
        public GameObject enemy;

        private FlashlightController flashlight;


        private NightmareController nightmareController;
        private PlaySeriesOfAudioClips playSeriesOfAudioClips;

        private FPSController fPSController;
        private bool isViewingClue = false;
        private AudioSource audioSource;
        private float timeAvailable = 0;
        private Quaternion originalRotation;
        private Vector3 originalPosition;
        private Vector3 originalScale;
        private float releaseTime=0;
        private float targetSize = 0.8f;
        private float minTargetSize = 0.1f;
        private float maxTargetSize = 2.0f;

        private float zoomStepSize = 0.05f;

        // Start is called before the first frame update
        void Start()
        {
            nightmareController = GameObject.Find("NightmareController").GetComponent<NightmareController>();
            fPSController = GameObject.Find("Player").GetComponent<FPSController>();
            flashlight = GameObject.Find("Player/MainCamera/Flashlight").GetComponent<FlashlightController>();
            audioSource = GetComponent<AudioSource>();
            playSeriesOfAudioClips = GetComponent<PlaySeriesOfAudioClips>();
            //if (spawnsEnemy)
            //    enemy.SetActive(false);

            //Disable final scene trigger at startup
            if(enablesClue)
                StartCoroutine(DisableNextClue());


            //Debug.Log("Started up " + gameObject.name);

        }

        IEnumerator DisableNextClue()
        {
            //Wait a tiny bit to let things get out of start
            yield return new WaitForSeconds(0.01f);
            nextClue.SetActive(false);
        }

        float rotationX = 0;
        float rotationY = 0;
        float horizontalInput;
        float verticalInput;

        float flashlightTimeAvailable;
        // Update is called once per frame
        void Update()
        {

            // Normal input should be disabled during this...
            if (isViewingClue)
            {

                // Still allow flashlight?
                if (Input.GetButton("Flashlight"))
                {
                    if(Time.realtimeSinceStartup > flashlightTimeAvailable)
                    {
                        flashlightTimeAvailable = Time.realtimeSinceStartup + 1.0f;
                        flashlight.toggleFlashlight();
                    }
                }


                var posScale = Input.GetKey(KeyCode.W) ? 1 : 0;
                var negScale = Input.GetKey(KeyCode.S) ? 1 : 0;
                var scaleChange = posScale - negScale;
                if (scaleChange != 0)
                {
                    //Debug.Log("Scaling due to input " + scaleChange);
                    targetSize = targetSize + scaleChange * zoomStepSize;
                    // Dont allow zoom to get too out of hand and go negative or massive
                    if (targetSize < minTargetSize)
                        targetSize = minTargetSize;
                    if (targetSize > maxTargetSize)
                        targetSize = maxTargetSize;
                    //Debug.Log("New Size " + targetSize);
                    RescaleClue();
                }

                // Read the mouse input axis
                rotationX += Input.GetAxis("Mouse X") * fPSController.mouseSensitivityX;
                rotationY += Input.GetAxis("Mouse Y") * fPSController.mouseSensitivityY;
                Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
                Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);

                transform.localRotation = originalRotation * xQuaternion * yQuaternion;

                // This will reset position of clue to original orientation
                if ((Input.GetButton("Use") && (Time.realtimeSinceStartup > releaseTime)))
                    PutDownClue();

            }

        }

        float lengthOfAudio;
        float timeDoneWithAudio;

        public void Use()
        {
            // In case we want something you can use more than once
            if(isEnabled && (useCount < uses) && (Time.time > timeAvailable))
            {
                timeAvailable = Time.time + timeDisabledAfterUse;
                Debug.Log("using item " + gameObject);
                useCount++;
                if (playSeriesOfAudioClips != null)
                {
                    playSeriesOfAudioClips.PlaySeries();
                    lengthOfAudio = playSeriesOfAudioClips.GetClipsLength();
                    timeDoneWithAudio = Time.time + lengthOfAudio;
                    //Debug.Log("Playing series");
                }
                else if (useSound != null)
                {
                    audioSource.PlayOneShot(useSound);
                } else
                {

                    Debug.Log("No audio");
                }

                // This is the last use
                if (useCount == uses)
                {
                    GetComponent<FlashingIndicator>().DisableFlasher();
                }

                if (triggersNightmareMode)
                {
                    var triggerDelay = 0.0f;
                    for(int i=0;i< audioClipsBeforeTrigger;i++)
                        triggerDelay += playSeriesOfAudioClips.GetClipLength(i);
                    StartCoroutine(TriggerNightmareWithDelay(triggerDelay));
                }

                if (triggersDreamMode)
                {
                    nightmareController.SwitchToDream();
                    blockage.SetActive(false);
                    if (spawnsEnemy)
                        enemy.SetActive(false);
                    if (triggersNewSpawnPoint)
                        fPSController.RecordNewSpawnPoint();
                }


                originalPosition = transform.position;
                originalScale = transform.localScale;
                originalRotation = transform.rotation;
                ViewClue();

                StartCoroutine(DelayClueEnable(timeDoneWithAudio));
            }
        }
        IEnumerator TriggerNightmareWithDelay(float triggerDelay)
        {
            yield return new WaitForSeconds(triggerDelay);
            nightmareController.SwitchToNightmare();
            if (spawnsEnemy)
                enemy.SetActive(true);
        }

            IEnumerator DelayClueEnable(float timeDoneWithAudio)
        {
            //Wait a tiny bit to let things get out of start
            while(Time.time < timeDoneWithAudio)
                yield return new WaitForSeconds(1.0f);
            if (enablesClue)
            {
                if (!nextClue.activeSelf)
                {
                    nextClue.SetActive(true);
                }

                if (nextClue.GetComponent<ClueController>() != null)
                {
                    nextClue.GetComponent<ClueController>().enableClue();
                }
            }
        }
        

        void ViewClue()
        {

            // Disable player
            fPSController.InputControl(false);
            //disable scene time
            Time.timeScale = 0;
            isViewingClue = true;
            Vector3 xyz = transform.GetComponentInChildren<MeshFilter>().mesh.bounds.size;
            float size = Mathf.Max(xyz.x, xyz.y, xyz.z);
            
            transform.position = Camera.main.transform.position + Camera.main.transform.forward * 1;
            float scaleFactor = targetSize / size;
            Vector3 newScale = new Vector3(originalScale.x * scaleFactor, originalScale.y * scaleFactor, originalScale.z * scaleFactor);
            transform.localScale = newScale;
            releaseTime = Time.realtimeSinceStartup + minTimeToHoldItem;
        }

        void RescaleClue()
        {
            Vector3 xyz = transform.GetComponentInChildren<MeshFilter>().mesh.bounds.size;
            float size = Mathf.Max(xyz.x, xyz.y, xyz.z);
            float scaleFactor = targetSize / size;
            Vector3 newScale = new Vector3(originalScale.x * scaleFactor, originalScale.y * scaleFactor, originalScale.z * scaleFactor);
            transform.localScale = newScale;
        }

        void PutDownClue()
        {
            transform.rotation = originalRotation;
            transform.position = originalPosition;
            transform.localScale = originalScale;

            // Enable player
            fPSController.InputControl(true);
            //enable scene time
            Time.timeScale = 1;
            isViewingClue = false;
        }

        public void enableClue()
        {
            isEnabled = true;
            GetComponent<FlashingIndicator>().EnableFlasher();
        }
    }
}