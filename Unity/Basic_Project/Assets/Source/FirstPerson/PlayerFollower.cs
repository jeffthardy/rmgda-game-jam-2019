using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    public Vector3 positionOffset = new Vector3(0,0,-90);

    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = player.transform.position + positionOffset;
        transform.rotation = player.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + positionOffset;
        transform.rotation = player.transform.rotation;
    }
    
}
