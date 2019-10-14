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


        public GameObject buttonPanel;
        public GameObject gameCreditsPanel;

        // Start is called before the first frame update
        void Start()
        {
            buttonPanel =  GameObject.Find("[UI]/Canvas/ButtonPanel");
            gameCreditsPanel = GameObject.Find("[UI]/Canvas/GameCreditsPanel");
            Time.timeScale = 1;
            if (startButton)
                startButton.onClick.AddListener(StartButtonOnClick);
            if (creditsButton)
                creditsButton.onClick.AddListener(CreditsOnClick);
            if (quitButton)
                quitButton.onClick.AddListener(QuitOnClick);
            if (optionsButton)
                optionsButton.onClick.AddListener(OptionsOnClick);


            string[] scenes = { "Level1_KatsBedroom",
                                "Level1_House",
                                "Level1_ExitArea",
                                "Level1_Twin1Bedroom",
                                "Level1_Twin2Bedroom",
                                "Level1_ParentsBedroom",
                                "Level1_ParentsBathroom",
                                "Level1_LivingRoom",
                                "Level1_GuestRoom",
                                "Level1_Kitchen",
                                "Level1_DiningRoom",
            };

            foreach (string scene in scenes)
            {
                Scene sceneToLoad = SceneManager.GetSceneByName(scene);
                if (!sceneToLoad.isLoaded)
                    SceneManager.LoadScene(scene, LoadSceneMode.Additive);
            }

        }

        private void Update()
        {

            if (Input.GetButtonDown("Quit") || Input.GetButtonDown("Cancel"))
            {
                buttonPanel.SetActive(true);
                gameCreditsPanel.SetActive(false);
            }
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
            //SceneManager.LoadScene(creditsSceneName);
            buttonPanel.SetActive(false);
            gameCreditsPanel.SetActive(true);

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