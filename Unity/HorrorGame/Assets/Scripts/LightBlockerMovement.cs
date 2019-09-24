using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBlockerMovement : MonoBehaviour
{
    public bool moveX = true;
    public bool moveY = false;
    public bool moveZ = false;
    public float startX = -600;
    public float stopX = 0;
    public float startY = 0;
    public float stopY = 0;
    public float startZ = 0;
    public float stopZ = 0;
    public float startDelay = 2;
    public float timeToFinish = 10;


    private float startTime;
    private float moveTime;
    private float newXPosition;
    private float newYPosition;
    private float newZPosition;

    // Start is called before the first frame update
    void Start()
    {
        if (!moveX)
            startX = transform.position.x;

        if (!moveY)
            startY = transform.position.y;

        if (!moveZ)
            startZ = transform.position.z;

        transform.position = new Vector3(startX, startY, startZ);
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        moveTime = Time.time - startTime - startDelay;
        if(moveTime>0 && moveTime< timeToFinish)
        {
            if (!moveX)
                newXPosition = startX;
            else
                newXPosition = (stopX - startX) * (moveTime / timeToFinish);

            if (!moveY)
                newYPosition = startY;
            else
                newYPosition = (stopY - startY) * (moveTime / timeToFinish);

            if (!moveZ)
                newZPosition = startZ;
            else
                newZPosition = (stopZ - startZ) * (moveTime / timeToFinish);

            transform.position = new Vector3(startX+ newXPosition, newYPosition, newZPosition);
        }
    }
}
