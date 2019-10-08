using UnityEngine;

public class DoorController : MonoBehaviour
{

    public bool isEnabled = true;
    public bool isOpen = false;
    public bool rotateClockwise = true;
    public float useRate = 0.5f;
    public AudioClip doorOpen;
    public AudioClip doorClose;


    private float availableTime;
    private AudioSource audioSource;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        availableTime = Time.time + useRate;
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Use()
    {
        if((Time.time > availableTime) && isEnabled)
        {
            isOpen = !isOpen;
           // Debug.Log("Using " + gameObject + " to make it open == " + isOpen);

            if (isOpen)
            {
                audioSource.clip = doorClose;
                if (rotateClockwise)
                {
                    animator.SetTrigger("Close");
                    //transform.Rotate(new Vector3(0, 90, 0));
                }
                else
                {
                    animator.SetTrigger("Close");
                    //transform.Rotate(new Vector3(0, -90, 0));
                }
            }
            else
            {
                audioSource.clip = doorOpen;
                if (rotateClockwise)
                {
                    animator.SetTrigger("Open");
                    //transform.Rotate(new Vector3(0, -90, 0));
                }
                else
                {
                    animator.SetTrigger("Open");
                    //transform.Rotate(new Vector3(0, 90, 0));
                }
            }
            // Dont allow the user to hit this too fast
            availableTime = Time.time + useRate;

            audioSource.Stop();
            audioSource.Play(0);
        }
    }

    public void DisableDoor()
    {
        isEnabled = false;
    }
    public void EnableDoor()
    {
        isEnabled = true;
    }
}
