using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace TopZombies
{
    public class creditsMenu : MonoBehaviour
    {
        public Button backButton;
        public string menuSceneName;


        // Start is called before the first frame update
        void Start()
        {
            if (backButton)
                backButton.onClick.AddListener(MenuButtonOnClick);

        }

        void MenuButtonOnClick()
        {
            Debug.Log("You have clicked the back button!");

            // Clear any existing player data which is used for life storage
            //PlayerPrefs.DeleteAll();

            SceneManager.LoadScene(menuSceneName);
        }
        
    }
}