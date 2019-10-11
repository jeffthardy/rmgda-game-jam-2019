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
        public AudioClip whiteNoise;

        public bool tvIsOn = true;

        private AudioSource audioSource;

        // Start is called before the first frame update
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
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
            if (on)
            {
                audioSource.Stop();
                audioSource.loop = true;
                audioSource.clip = whiteNoise;
                audioSource.volume = 0.5f;
                audioSource.Play();
            } else
            {
                audioSource.Stop();
            }
        }
    }
}