using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationController : MonoBehaviour
{

    public GameObject player;
    public float distThreshold = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(player.transform.position, transform.position);
        if(dist < distThreshold)
        {
            GetComponent<MeshRenderer>().enabled = false;
        } else
        {
            GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
