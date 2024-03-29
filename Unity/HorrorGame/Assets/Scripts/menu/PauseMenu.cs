﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace TopZombies
{

    public class PauseMenu : MonoBehaviour
    {
        public Button resumeButton;
        public Button quitButton;
        public string menuSceneName;
        public Color initialColor = new Color(0,0,0,0);
        private Color initialColorOff;

        private FPSController fPSController;
        public Slider mouseSlider;

        bool isPaused = false;


        // Start is called before the first frame update
        void Start()
        {
            if (resumeButton)
                resumeButton.onClick.AddListener(ResumeButtonOnClick);

            if (quitButton)
                quitButton.onClick.AddListener(QuitButtonOnClick);

            if (mouseSlider)
                mouseSlider.onValueChanged.AddListener(delegate { UpdateMouseSensitivity(); });

            fPSController = GameObject.Find("Player").GetComponent<FPSController>();
            //initialColor = GetComponent<Image>().color;
            initialColorOff = new Color(initialColor.r, initialColor.g, initialColor.b, 0);

            // Disable children so they aren't on the screen
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i).gameObject;
                if (child != null)
                    child.SetActive(false);
            }
            if(GetComponent<Image>().color != null)
                GameObject.Find("[UI]/Canvas/PausePanel").GetComponent<Image>().color = initialColorOff;

        }

        CursorLockMode previousCursorLockState;
        bool previousCursorVisible;
        bool previousFPSInputEnabled;
        float previousTimeScale;

        CursorLockMode pauseCursorLockState;
        bool pauseCursorVisible;
        bool pauseFPSInputEnabled;
        float pauseTimeScale;

        void UpdateMouseSensitivity()
        {
            float mouseSense = mouseSlider.value;
            if (fPSController.mouseSensitivityX != mouseSense)
                fPSController.mouseSensitivityX = mouseSense;
            if (fPSController.mouseSensitivityY != mouseSense)
                fPSController.mouseSensitivityY = mouseSense;
        }

        public void PauseGame()
        {
            if (!isPaused)
            {
                isPaused = true;
                previousCursorLockState = Cursor.lockState;
                previousCursorVisible = Cursor.visible;
                previousFPSInputEnabled = fPSController.enableInput;
                previousTimeScale = Time.timeScale;

                pauseCursorLockState = CursorLockMode.None;
                pauseCursorVisible = true;
                pauseFPSInputEnabled = false;
                pauseTimeScale = 0;

                // Give mouse back to user
                Cursor.lockState = fPSController.cursorLockedMode = pauseCursorLockState;
                Cursor.visible = pauseCursorVisible;

                // Disable player
                fPSController.InputControl(pauseFPSInputEnabled);
                //disable scene time
                Time.timeScale = pauseTimeScale;

                //Enable panel views
                GameObject.Find("[UI]/Canvas/PausePanel").GetComponent<Image>().color = initialColor;

                // Show all the panel children
                for (int i = 0; i < transform.childCount; i++)
                {
                    var child = transform.GetChild(i).gameObject;
                    if (child != null)
                        child.SetActive(true);
                }

                PlaySeriesOfAudioClips.PauseAll();
            }

        }

        void ResumeButtonOnClick()
        {
            // Disable this menu view and re-enable player
            if(fPSController.cursorLockedMode == pauseCursorLockState)
                fPSController.cursorLockedMode = previousCursorLockState;
            Cursor.lockState = previousCursorLockState;
            // Hide cursor when locking
            if (Cursor.visible == pauseCursorVisible)
                Cursor.visible = previousCursorVisible;


            //Enable panel views
            GameObject.Find("[UI]/Canvas/PausePanel").GetComponent<Image>().color = initialColorOff;

            //Disable all the children as well
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i).gameObject;
                if (child != null)
                    child.SetActive(false);
            }

            // Re-enable player control
            if (fPSController.enableInput == pauseFPSInputEnabled)
                fPSController.InputControl(previousFPSInputEnabled);
            //Re-enable scene time
            if (Time.timeScale == pauseTimeScale)
                Time.timeScale = previousTimeScale;

            PlaySeriesOfAudioClips.UnpauseAll();
            isPaused = false;
        }



        void QuitButtonOnClick()
        {
            Debug.Log("You have clicked the quit button!");
            Cursor.lockState = fPSController.cursorLockedMode = CursorLockMode.None;
            Cursor.visible = (CursorLockMode.Locked != fPSController.cursorLockedMode);
#if UNITY_EDITOR
            SceneManager.LoadScene(menuSceneName);
#elif UNITY_WEBGL
            SceneManager.LoadScene(menuSceneName);
#else
            SceneManager.LoadScene(menuSceneName);
#endif

        }

    }
}
