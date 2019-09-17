using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticleController : MonoBehaviour
{
    private void Start()
    {
        GUI.color = Color.yellow;
    }
    void OnGUI()
    {
        GUI.Box(new Rect(Screen.width / 2, Screen.height / 2, 5, 5), "");
    }
}
