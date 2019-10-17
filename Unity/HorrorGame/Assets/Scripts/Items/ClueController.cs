using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        public bool allowRotation = true;
        public bool allowScale = true;

        private FlashlightController flashlight;


        private NightmareController nightmareController;
        private float nightmareTriggerDelay = 0.0f;
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

        public bool causesSpectreVision = false;
        private SpectreVisionController spectreVisionController;
        private BackgroundMusicController backgroundMusicController;

        private CameraTracker clueCameraTracker;

        // Start is called before the first frame update
        void Start()
        {
            nightmareController = GameObject.Find("NightmareController").GetComponent<NightmareController>();
            fPSController = GameObject.Find("Player").GetComponent<FPSController>();
            flashlight = GameObject.Find("Player/MainCamera/Flashlight").GetComponent<FlashlightController>();
            spectreVisionController = GameObject.Find("Player/ClueCamera/SpectrePopup").GetComponent<SpectreVisionController>();
            backgroundMusicController = GameObject.Find("Player/BGMusic").GetComponent<BackgroundMusicController>();
            clueCameraTracker = GameObject.Find("Player/ClueCamera").GetComponent<CameraTracker>();
            audioSource = GetComponent<AudioSource>();
            playSeriesOfAudioClips = GetComponent<PlaySeriesOfAudioClips>();
            //if (spawnsEnemy)
            //    enemy.SetActive(false);

            nightmareTriggerDelay = 0.0f;
            for (int i = 0; i < audioClipsBeforeTrigger; i++)
                nightmareTriggerDelay += playSeriesOfAudioClips.GetClipLength(i);
            if(nightmareTriggerDelay != 0)
                Debug.Log("Nightmare triggers with " + nightmareTriggerDelay + " s delay.");

            //Disable final scene trigger at startup
            if (enablesClue)
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

                if (allowScale)
                {
                    var posScale = Input.GetKey(KeyCode.W) ? 1 : 0;
                    var negScale = Input.GetKey(KeyCode.S) ? 1 : 0;
                    var scaleChange = posScale - negScale;
                    if (scaleChange != 0)
                    {
                        //Debug.Log("Scaling due to input " + scaleChange);
                        targetSize = targetSize + scaleChange * Time.deltaTime;
                        // Dont allow zoom to get too out of hand and go negative or massive
                        if (targetSize < minTargetSize)
                            targetSize = minTargetSize;
                        if (targetSize > maxTargetSize)
                            targetSize = maxTargetSize;
                        //Debug.Log("New Size " + targetSize);
                        RescaleClue();
                    }
                }

                if (allowRotation)
                {
                    // Read the mouse input axis
                    rotationX += Input.GetAxis("Mouse X") * fPSController.mouseSensitivityX;
                    rotationY += Input.GetAxis("Mouse Y") * fPSController.mouseSensitivityY;
                    Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
                    Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);

                    transform.localRotation = originalRotation * xQuaternion * yQuaternion;
                }

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
                    StartCoroutine(TriggerNightmareWithDelay(nightmareTriggerDelay));
                }

                if (triggersDreamMode)
                {
                    backgroundMusicController.PlayMusic(BackgroundMusicController.MusicTypes.normal);
                    nightmareController.SwitchToDream();
                    blockage.SetActive(false);
                    if (spawnsEnemy)
                        enemy.SetActive(false);
                }


                // Changing to respawn even at nightmare trigger, because last good location could be harder to get back to the new good location
                // This could end up resulting in repeated deaths though...
                if (triggersNewSpawnPoint)
                    fPSController.RecordNewSpawnPoint();


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
            backgroundMusicController.PlayMusic(BackgroundMusicController.MusicTypes.nightmare);
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

        Vector3 originalBounds;
        float originalSize;

        void ViewClue()
        {

            // Disable player
            fPSController.InputControl(false);
            //disable scene time
            //Time.timeScale = 0;
            isViewingClue = true;
            originalBounds = transform.GetComponentInChildren<Renderer>().bounds.size;
            originalSize = Mathf.Max(originalBounds.x, originalBounds.y, originalBounds.z);
            if (allowRotation || allowScale)
            {
                Debug.Log("Object size is " + originalSize);
                transform.position = Camera.main.transform.position + Camera.main.transform.forward * 1;
                float scaleFactor = targetSize / originalSize;
                Debug.Log("scaleFactor is " + scaleFactor);
                Vector3 newScale = new Vector3(originalScale.x * scaleFactor, originalScale.y * scaleFactor, originalScale.z * scaleFactor);
                transform.localScale = newScale;
                Debug.Log("newScale is " + newScale);
            }
            releaseTime = Time.realtimeSinceStartup + minTimeToHoldItem;
            clueCameraTracker.EnableCamera(true);
        }

        void RescaleClue()
        {
            //Vector3 xyz = transform.GetComponentInChildren<Renderer>().bounds.size;
            //float size = Mathf.Max(originalBounds.x, xyz.y, xyz.z);
            float scaleFactor = targetSize / originalSize;
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

            clueCameraTracker.EnableCamera(false);

            if (causesSpectreVision)
                spectreVisionController.DisplaySpectreForTime(1.0f);

            playSeriesOfAudioClips.Unpause();
        }

        public void PauseAudioSeriesIfViewing()
        {
            if (isViewingClue)
            {
                playSeriesOfAudioClips.Pause();
            }
        }

        public void enableClue()
        {
            isEnabled = true;
            GetComponent<FlashingIndicator>().EnableFlasher();
        }
    }
}
