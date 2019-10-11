﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TopZombies
{
    public class enemyMover : MonoBehaviour
    {
        public GameObject[] goals;
        public float playerLostWait = 1.0f;
        private NavMeshAgent agent;
        private Vector3 currentTarget;
        private Vector3 playerLocation;

        private Animator viewAnimator;

        private float minDistanceToPoint = 0.5f;

        float pingRate = 1.0f;
        float nextPing = 0.0f;

        float minDistancePerPing= 1.0f;
        Vector3 myLastPosition;

        // Start is called before the first frame update
        void Start()
        {
            viewAnimator = GetComponentInChildren<Animator>();
            agent = GetComponent<NavMeshAgent>();
            currentTarget = goals[0].transform.position;
            myLastPosition = transform.position;
        }




        // Update is called once per frame
        void Update()
        {
            agent.destination = currentTarget;
            //Debug.Log(agent.destination);

            //Handle cases where path detect gets frozen?  FIXME

            if (agent.isStopped)
            {
                viewAnimator.SetTrigger("Idle");
            } else
            {
                viewAnimator.SetTrigger("Walk");
            }

            //Handle player tracking and resetting once reaching the last know player location
            if ((Vector3.Distance(transform.position, playerLocation) < minDistanceToPoint) && (currentTarget == playerLocation))
            {
                // Go back to waypoints
                StartCoroutine(ResetMovePatternAfterWait(goals[0].transform.position));
                //Debug.Log("Reached last player location, Resetting back to goals." + Vector3.Distance(transform.position, playerLocation));
            }
            else if (currentTarget == playerLocation)
            {
                //Debug.Log("Distance to player" + Vector3.Distance(transform.position, playerLocation));

            }
            

            if(Time.time > nextPing)
            {
                nextPing = Time.time + pingRate;

                if (Vector3.Distance(transform.position, myLastPosition) < minDistancePerPing)
                    Debug.Log("WARNING: " + gameObject.name + " might be stuck?");

                myLastPosition = transform.position;
            }


        }

        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log("Collision detected with " + other.gameObject);
            for (int i = 0; i < goals.Length; i++)
            {
                if (other.gameObject == goals[i].gameObject)
                {
                    int next = (i + 1) % goals.Length;
                    //Debug.Log(currentTarget);
                    currentTarget = goals[next].transform.position;
                    //Debug.Log("changing target from  " + i + " to " + next);
                    //Debug.Log(currentTarget);
                }
            }
        }
        public void SetNewGoal(Vector3 target)
        {
            playerLocation = target;
            currentTarget = playerLocation;
        }

        IEnumerator ResetMovePatternAfterWait(Vector3 target)
        {
            yield return new WaitForSeconds(playerLostWait);
            currentTarget = target;
            Debug.Log("Resetting target to " + currentTarget);
        }
    }
}