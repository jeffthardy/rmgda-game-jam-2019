using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace TopZombies
{
    public class menuController : MonoBehaviour
    {
        public Button startButton;
        public Button optionsButton;
        public Button creditsButton;
        public Button quitButton;
        public string startSceneName;
        public string creditsSceneName;

        // Start is called before the first frame update
        void Start()
        {
            if (startButton)
                startButton.onClick.AddListener(StartButtonOnClick);
            if (creditsButton)
                creditsButton.onClick.AddListener(CreditsOnClick);
            if (quitButton)
                quitButton.onClick.AddListener(QuitOnClick);
            if (optionsButton)
                optionsButton.onClick.AddListener(OptionsOnClick);

        }

        void StartButtonOnClick()
        {
            Debug.Log("You have clicked the start button!");

            // Clear any existing player data which is used for life storage
            //PlayerPrefs.DeleteAll();

            SceneManager.LoadScene(startSceneName);
        }

        void CreditsOnClick()
        {
            Debug.Log("You have clicked the start button!");
            //SceneManager.LoadScene("About");
            SceneManager.LoadScene(creditsSceneName);
        }

        void OptionsOnClick()
        {
            Debug.Log("You have clicked the Options button!");
            //SceneManager.LoadScene("Options");
        }


        void QuitOnClick()
        {
            Debug.Log("You have clicked the quit button!");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
        //Application.OpenURL(webplayerQuitURL);
#else
        Application.Quit();
#endif

        }
    }
}