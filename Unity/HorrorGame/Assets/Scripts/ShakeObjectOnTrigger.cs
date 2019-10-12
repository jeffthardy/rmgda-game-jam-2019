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


        List<Collider> TriggerList = new List<Collider>();
        float closestEnemyDistance = 100.0f;


        // Start is called before the first frame update
        void Start()
        {
            initialPosition = Camera.main.transform.localPosition;
            currentPosition = initialPosition;
            timeToChange = Time.time;
            myDOFChanger = GameObject.Find("PostProcessingEffects").GetComponent<constantDOFChanger>();
            closestEnemyDistance = dofDistanceMax;
        }

        // Update is called once per frame
        void Update()
        {
            //Cleanup deactivated enemies since they dont triggerExit
            foreach (Collider col in TriggerList)
            {
                if (!col.gameObject.activeInHierarchy)
                    TriggerList.Remove(col);
            }

            if ((TriggerList.Count > 0) && (Time.time > timeToChange))
            {
                timeToChange = Time.time + changeRate;
                MoveTransformOffset();

                foreach (Collider col in TriggerList)
                {
                    closestEnemyDistance = dofDistanceMax;
                    if (Vector3.Distance(col.transform.position, transform.position) < closestEnemyDistance)
                        closestEnemyDistance = Vector3.Distance(col.transform.position, transform.position);
                }
                // Calculate distance to enemy and use that to set DOF 2 closest and 6 furthest?
                currentDOF = ((closestEnemyDistance) / dofDistanceMax) * (dofMax - dofMin) + dofMin;
                //Debug.Log("Closest enemy is " + closestEnemyDistance + "  DOF " + currentDOF);
                myDOFChanger.SetDOF(true, currentDOF);
                isTriggered = true;
            }

            if (TriggerList.Count == 0)
            {
                Camera.main.transform.localPosition = initialPosition;
                myDOFChanger.SetDOF(false, currentDOF);
                isTriggered = false;
                closestEnemyDistance = dofDistanceMax;
            }

        }

        private void MoveTransformOffset()
        {
            float xChange = Random.Range(-xOffsetMax, xOffsetMax);
            float yChange = Random.Range(-yOffsetMax, yOffsetMax);
            float zChange = Random.Range(-zOffsetMax, zOffsetMax);
                        
            currentPosition = new Vector3(initialPosition.x + xChange, initialPosition.y + yChange, initialPosition.z + zChange);
            //Debug.Log("Setting new offset " + currentPosition);
            Camera.main.transform.localPosition = currentPosition;
        }

        public void ResetDetector()
        {
            triggerCount = 0;
            isTriggered = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                TriggerList.Add(other);
            }
        }
//        private void OnTriggerStay(Collider other)
//        {
//            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
//            {
//                if(Vector3.Distance(other.transform.position, transform.position) < closestEnemyDistance)
//                    closestEnemyDistance = Vector3.Distance(other.transform.position, transform.position);
//            }
//        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                if (TriggerList.Contains(other))
                    TriggerList.Remove(other);
            }
        }

        private void CalculateDOF()
        {

        }
    }
}