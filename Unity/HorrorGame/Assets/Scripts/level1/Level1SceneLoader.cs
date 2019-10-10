using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TopZombies
{
    public class Level1SceneLoader : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
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

        // Update is called once per frame
        void Update()
        {

        }
    }
}