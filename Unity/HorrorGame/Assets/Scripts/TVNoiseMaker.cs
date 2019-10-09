using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVNoiseMaker : MonoBehaviour
{
    float updateSpeed = 0.02f;
    Renderer rend;
    float updateTime;
    public int materialIndex;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        updateTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > updateTime)
        {
            updateTime = Time.time + updateSpeed;
            float offsetx = Random.Range(-256.0f, 256.0f);
            float offsety = Random.Range(-256.0f, 256.0f);
            rend.materials[materialIndex].SetTextureOffset("_MainTex", new Vector2(offsetx, offsety));
        }
    }
}
