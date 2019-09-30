using System.Collections;
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


        // Start is called before the first frame update
        void Start()
        {
            if (resumeButton)
                resumeButton.onClick.AddListener(ResumeButtonOnClick);

            if (quitButton)
                quitButton.onClick.AddListener(QuitButtonOnClick);

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

        public void PauseGame()
        {
            // Give mouse back to user
            Cursor.lockState = fPSController.cursorLockedMode = CursorLockMode.None;
            Cursor.visible = (CursorLockMode.Locked != fPSController.cursorLockedMode);

            // Disable player
            fPSController.InputControl(false);
            //disable scene time
            Time.timeScale = 0;

            //Enable panel views
            GameObject.Find("[UI]/Canvas/PausePanel").GetComponent<Image>().color = initialColor;

            // Show all the panel children
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i).gameObject;
                if (child != null)
                    child.SetActive(true);
            }
            
        }


        void ResumeButtonOnClick()
        {
            // Disable this menu view and re-enabl player
            fPSController.cursorLockedMode = CursorLockMode.Locked;
            Cursor.lockState = fPSController.cursorLockedMode;
            // Hide cursor when locking
            Cursor.visible = (CursorLockMode.Locked != fPSController.cursorLockedMode);


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
            fPSController.InputControl(true);
            //Re-enable scene time
            Time.timeScale = 1;
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