using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class menuController : MonoBehaviour
{
    public Button m_StartButton;
    public Button m_OptionsButton;
    public Button m_AboutButton;
    public Button m_QuitButton;

    // Start is called before the first frame update
    void Start()
    {
        if (m_StartButton)
            m_StartButton.onClick.AddListener(StartOnClick);
        if (m_AboutButton)
            m_AboutButton.onClick.AddListener(AboutOnClick);
        if (m_QuitButton)
            m_QuitButton.onClick.AddListener(QuitOnClick);
        if (m_OptionsButton)
            m_OptionsButton.onClick.AddListener(OptionsOnClick);

    }

    void OptionsOnClick()
    {
        Debug.Log("You have clicked the Options button!");
        SceneManager.LoadScene("Options");
    }

    void StartOnClick()
    {
        Debug.Log("You have clicked the start button!");
        SceneManager.LoadScene("SampleScene");
    }

    void AboutOnClick()
    {
        Debug.Log("You have clicked the start button!");
        SceneManager.LoadScene("About");
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
