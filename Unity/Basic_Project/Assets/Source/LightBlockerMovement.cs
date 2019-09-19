using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBlockerMovement : MonoBehaviour
{
    public float startX = -600;
    public float stopX = 0;
    public float startDelay = 2;
    public float timeToFinish = 10;


    private float startTime;
    private float moveTime;
    private float newXPosition;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(startX, transform.position.y, transform.position.z);
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        moveTime = Time.time - startTime - startDelay;
        if(moveTime>0 && moveTime< timeToFinish)
        {
            newXPosition = (stopX - startX) * (moveTime / timeToFinish);
            transform.position = new Vector3(startX+ newXPosition, transform.position.y, transform.position.z);
        }
    }
}
