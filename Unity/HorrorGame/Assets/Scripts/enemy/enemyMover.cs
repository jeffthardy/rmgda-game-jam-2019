using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TopZombies
{
    public class enemyMover : MonoBehaviour
    {
        public GameObject[] goals;
        private NavMeshAgent agent;
        private Transform currentTarget;
        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            currentTarget = goals[0].transform;
        }


        // Update is called once per frame
        void Update()
        {
            agent.destination = currentTarget.position;
            //Debug.Log(agent.destination);
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Collision detected with " + other.gameObject);
            for (int i = 0; i < goals.Length; i++)
            {
                if (other.gameObject == goals[i].gameObject)
                {
                    int next = (i + 1) % goals.Length;
                    //Debug.Log(currentTarget);
                    currentTarget = goals[next].transform;
                    //Debug.Log("changing target from  " + i + " to " + next);
                    //Debug.Log(currentTarget);
                }
            }

        }
    }
}