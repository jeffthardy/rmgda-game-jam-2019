using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TopZombies
{
    public class ShakeObjectOnTrigger : MonoBehaviour
    {
        public float xOffsetMax = 1.0f;
        public float yOffsetMax = 1.0f;
        public float zOffsetMax = 1.0f;
        public float changeRate = 0.5f;
        public float dofDistanceMax = 20.0f;
        public float dofMin = 1.0f;
        public float dofMax = 5.0f;
        public bool isTriggered = false;

        private int triggerCount = 0;
        private float timeToChange;

        private Vector3 initialPosition;
        private Vector3 currentPosition;
        private float currentDOF = 0;

        private constantDOFChanger myDOFChanger;


        // Start is called before the first frame update
        void Start()
        {
            initialPosition = transform.localPosition;
            currentPosition = initialPosition;
            timeToChange = Time.time;
            myDOFChanger = GameObject.Find("PostProcessingEffects").GetComponent<constantDOFChanger>();
        }

        // Update is called once per frame
        void Update()
        {
            if((triggerCount > 0) && (Time.time > timeToChange))
            {
                timeToChange = Time.time + changeRate;
                MoveTransformOffset();

                // Calculate distance to enemy and use that to set DOF 2 closest and 6 furthest?
                myDOFChanger.SetDOF(true, currentDOF);
                isTriggered = true;
            }

            if (triggerCount == 0)
            {
                transform.localPosition = initialPosition;
                myDOFChanger.SetDOF(false, currentDOF);
                isTriggered = false;
            }

        }

        private void MoveTransformOffset()
        {
            float xChange = Random.Range(-xOffsetMax, xOffsetMax);
            float yChange = Random.Range(-yOffsetMax, yOffsetMax);
            float zChange = Random.Range(-zOffsetMax, zOffsetMax);

            currentPosition = new Vector3(initialPosition.x + xChange, initialPosition.y + yChange, initialPosition.z + zChange);
            Debug.Log("Setting new offset " + currentPosition);
            transform.localPosition = currentPosition;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                triggerCount++;
                Debug.Log("Enemy Count " + triggerCount);
                currentDOF = (Vector3.Distance(other.transform.position, transform.position) / dofDistanceMax) * (dofMax- dofMin) + dofMin; 
            }
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                currentDOF = (Vector3.Distance(other.transform.position, transform.position) / dofDistanceMax) * (dofMax - dofMin) + dofMin;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                triggerCount--;
                Debug.Log("Enemy Count " + triggerCount);
            }
        }

        private void CalculateDOF()
        {

        }
    }
}