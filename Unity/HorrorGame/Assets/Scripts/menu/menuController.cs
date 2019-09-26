﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class menuController : MonoBehaviour
{
    public Button m_StartOption1;
    public Button m_Start1stButton;
    public Button m_StartFPSButton;
    public Button m_OptionsButton;
    public Button m_AboutButton;
    public Button m_QuitButton;

    // Start is called before the first frame update
    void Start()
    {
        if (m_StartOption1)
            m_StartOption1.onClick.AddListener(StartOption1OnClick);
        if (m_Start1stButton)
            m_Start1stButton.onClick.AddListener(Start1stPersonOnClick);
        if (m_StartFPSButton)
            m_StartFPSButton.onClick.AddListener(StartFPSOnClick);
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
        //SceneManager.LoadScene("Options");
    }

    void StartOption1OnClick()
    {
        Debug.Log("You have clicked the start button!");

        // Clear any existing player data which is used for life storage
        PlayerPrefs.DeleteAll();

        SceneManager.LoadScene("2_5dPerson");
    }
    void Start1stPersonOnClick()
    {
        Debug.Log("You have clicked the start button!");
        SceneManager.LoadScene("1stPersonTeleport");
    }
    void StartFPSOnClick()
    {
        Debug.Log("You have clicked the start button!");
        SceneManager.LoadScene("1stPersonShooter");
    }

    void AboutOnClick()
    {
        Debug.Log("You have clicked the start button!");
        //SceneManager.LoadScene("About");
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