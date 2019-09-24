using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class constantDOFChanger : MonoBehaviour
{

    PostProcessVolume m_Volume;
    DepthOfField m_dof;
    public int layerToAffect = 9; // ground

    // Start is called before the first frame update
    void Start()
    {

        m_dof = ScriptableObject.CreateInstance<DepthOfField>();
        //m_dof.enabled.Override(true);
        m_dof.focusDistance.Override(1f);

        m_Volume = PostProcessManager.instance.QuickVolume(layerToAffect, 100f, m_dof);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            m_dof.enabled.value = true;
            m_dof.focusDistance.value = 0;
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            m_dof.enabled.value = false;
            //m_dof.focusDistance.value = 0;
        }
    }
}
