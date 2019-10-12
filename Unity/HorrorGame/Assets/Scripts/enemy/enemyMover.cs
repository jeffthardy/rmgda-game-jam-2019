﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TopZombies
{
    public class enemyMover : MonoBehaviour
    {
        public GameObject[] goals;
        public float playerLostWait = 3.0f;
        private NavMeshAgent agent;
        private Vector3 currentTarget;
        private Vector3 playerLocation;

        private Animator viewAnimator;
        private EnemyAudioGenerator enemyAudioGenerator;

        private float minDistanceToPoint = 1.5f;

        float pingRate = 1.0f;
        float nextPing = 0.0f;

        float minDistancePerPing = 1.0f;
        Vector3 myLastPosition;
        bool isCurrentlyResettingTarget = false;

        // Start is called before the first frame update
        void Start()
        {
            viewAnimator = transform.parent.GetComponentInChildren<Animator>();
            enemyAudioGenerator = transform.parent.GetComponentInChildren<EnemyAudioGenerator>();
            agent = transform.parent.GetComponent<NavMeshAgent>();
            currentTarget = goals[0].transform.position;
            Debug.Log("initial target  " + currentTarget + " from " + goals[0].transform.position);
            myLastPosition = transform.position;
            if(viewAnimator != null)
                viewAnimator.SetTrigger("Walk");
        }




        // Update is called once per frame
        void Update()
        {
            agent.destination = currentTarget;
            //Debug.Log(agent.destination);

            //Handle player tracking and resetting once reaching the last know player location
            if ((Vector3.Distance(transform.position, playerLocation) < minDistanceToPoint) && (currentTarget == playerLocation))
            {
                // Go back to waypoints
                if (!isCurrentlyResettingTarget)
                    StartCoroutine(ResetMovePatternAfterWait());
                //Debug.Log("Reached last player location, Resetting back to goals." + Vector3.Distance(transform.position, playerLocation));
            }
            else if (currentTarget == playerLocation)
            {
                //Debug.Log("Distance to player" + Vector3.Distance(transform.position, playerLocation));

            }

            if ((Time.time > nextPing) && !isCurrentlyResettingTarget)
            {
                nextPing = Time.time + pingRate;
            
                if (Vector3.Distance(transform.position, myLastPosition) < minDistancePerPing)
                {
                    Debug.Log("WARNING: " + gameObject.name + " might be stuck? going to target " + currentTarget + " with distance of " + Vector3.Distance(transform.position, currentTarget));
                    Debug.Log("Path Status: " + agent.pathStatus);
                    Debug.Log("Path exists: " + agent.hasPath);
                    if(agent.hasPath == false)
                    {
                        StartCoroutine(ResetMovePatternAfterWait());
                        Debug.Log("Reset to next waypoint goal after wait");
                    }
                }
            
                
                myLastPosition = transform.position;
            }
        }


        int lastGoal;
        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log("Collision detected with " + other.gameObject);
            for (int i = 0; i < goals.Length; i++)
            {
                if (other.gameObject == goals[i].gameObject)
                {
                    lastGoal = i;
                    if (!isCurrentlyResettingTarget)
                        StartCoroutine(ResetMovePatternAfterWait());
                    //int next = (i + 1) % goals.Length;
                    //lastGoal = next;
                    //Debug.Log(currentTarget);
                    //currentTarget = goals[next].transform.position;
                    //Debug.Log("changing target from  " + i + " to " + next);
                    //Debug.Log(currentTarget);
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            for (int i = 0; i < goals.Length; i++)
            {
                if (other.gameObject == goals[i].gameObject)
                {
                    lastGoal = i;
                    if (!isCurrentlyResettingTarget)
                        StartCoroutine(ResetMovePatternAfterWait());
                }
            }
        }

        public int GetNextGoal()
        {
            return (lastGoal + 1) % goals.Length;

        }

        public void SetNewGoal(Vector3 target)
        {
            playerLocation = target;
            currentTarget = playerLocation;
            if (viewAnimator != null)
                viewAnimator.SetTrigger("Walk");
            Debug.Log("changing target to player  " + playerLocation);
        }

        IEnumerator ResetMovePatternAfterWait()
        {
            isCurrentlyResettingTarget = true;
            Debug.Log("Set to idle");
            if (viewAnimator != null)
                viewAnimator.SetTrigger("Idle");
            enemyAudioGenerator.playIdleAudio();
            yield return new WaitForSeconds(playerLostWait);
            if (viewAnimator != null)
                viewAnimator.SetTrigger("Walk");
            enemyAudioGenerator.playWalkingAudio();
            Debug.Log("Set to walk");
            currentTarget = goals[GetNextGoal()].transform.position;
            Debug.Log("Resetting target to " + currentTarget);
            StartCoroutine(ClearResettingFlagAfterDelay());
        }

        // Wait a second so things looking for stalls wouldnt think it immediately stalls out of idle
        IEnumerator ClearResettingFlagAfterDelay()
        {
            yield return new WaitForSeconds(1.0f);
            isCurrentlyResettingTarget = false;
        }
    }
}