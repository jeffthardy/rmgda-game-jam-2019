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
           // Debug.Log("Using " + gameObject + " to make it open == " + isOpen);

            if (isOpen)
            {
                audioSource.clip = doorClose;
                if (rotateClockwise)
                {
                    animator.SetTrigger("CloseNegative");
                    //transform.Rotate(new Vector3(0, 90, 0));
                }
                else
                {
                    animator.SetTrigger("ClosePositive");
                    //transform.Rotate(new Vector3(0, -90, 0));
                }
            }
            else
            {
                audioSource.clip = doorOpen;
                if (rotateClockwise)
                {
                    animator.SetTrigger("OpenNegative");
                    //transform.Rotate(new Vector3(0, -90, 0));
                }
                else
                {
                    animator.SetTrigger("OpenPositive");
                    //transform.Rotate(new Vector3(0, 90, 0));
                }
            }
            isOpen = !isOpen;
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
