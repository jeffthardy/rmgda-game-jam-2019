using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopZombies
{

    public class PlayerDetectorAI : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerStay(Collider other)
        {
            // Detect if player is within our trigger
            if (other.gameObject.GetComponent<FPSController>() != null)
            {
                // Detect if we can see player via ray trace...


                //Debug.Log("Detected " + other.gameObject + " And setting as new destination!");
                //Set player as new target
                transform.parent.gameObject.GetComponentInChildren<enemyMover>().SetNewGoal(other.gameObject.transform.position);
                //gameObject.GetComponentInParent<enemyMover>().SetNewGoal(other.gameObject.transform.position);

                // Play audio on detection
                //Debug.Log("Attempting to play detect audio");
                if (transform.parent.gameObject.GetComponentInChildren<EnemyAudioGenerator>() != null)
                    transform.parent.gameObject.GetComponentInChildren<EnemyAudioGenerator>().playDetectAudio();
                else
                    Debug.Log("Couldnt connect to audio generator");
            }
        }
    }
}