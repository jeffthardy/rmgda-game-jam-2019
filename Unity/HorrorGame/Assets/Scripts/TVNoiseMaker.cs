using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TopZombies {
    public class TVNoiseMaker : MonoBehaviour
    {
        float updateSpeed = 0.02f;
        Renderer rend;
        float updateTime;
        public int materialIndex;

        public bool tvIsOn = true;

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
                if (tvIsOn)
                {
                    updateTime = Time.time + updateSpeed;
                    float offsetx = Random.Range(-256.0f, 256.0f);
                    float offsety = Random.Range(-256.0f, 256.0f);
                    rend.materials[materialIndex].color = new Color(1, 1, 1, 1);
                    rend.materials[materialIndex].SetTextureOffset("_MainTex", new Vector2(offsetx, offsety));
                }
                else
                {

                    rend.materials[materialIndex].color = new Color(0, 0, 0, 1);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<FPSController>() != null)
            {
                SetPower(true);
            }
    }

        public void SetPower(bool on)
        {
            tvIsOn = on;
        }
    }
}