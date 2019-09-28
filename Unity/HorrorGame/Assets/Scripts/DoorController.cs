﻿using UnityEngine;

public class DoorController : MonoBehaviour
{

    public bool isOpen = false;
    public bool rotateClockwise = true;
    public float useRate = 0.5f;
    public AudioClip doorOpen;
    public AudioClip doorClose;


    private float availableTime;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        availableTime = Time.time + useRate;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Use()
    {
        if(Time.time > availableTime)
        {
            isOpen = !isOpen;
           // Debug.Log("Using " + gameObject + " to make it open == " + isOpen);

            if (isOpen)
            {
                audioSource.clip = doorClose;
                if (rotateClockwise)
                {
                    transform.Rotate(new Vector3(0, 90, 0));
                }
                else
                {
                    transform.Rotate(new Vector3(0, -90, 0));
                }
            }
            else
            {
                audioSource.clip = doorOpen;
                if (rotateClockwise)
                {
                    transform.Rotate(new Vector3(0, -90, 0));
                }
                else
                {
                    transform.Rotate(new Vector3(0, 90, 0));
                }
            }
            // Dont allow the user to hit this too fast
            availableTime = Time.time + useRate;

            audioSource.Stop();
            audioSource.Play(0);
        }
    }
}