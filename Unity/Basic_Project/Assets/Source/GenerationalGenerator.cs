using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationalGenerator : MonoBehaviour
{


    public GameObject[] previousLifeModels;

    private int nextLifeID;
    private string key;

    // Start is called before the first frame update
    void Start()
    {
        bool foundKey = true;
        int lifeID = 0;
        nextLifeID = 0;

        //PlayerPrefs.DeleteAll();

        while (foundKey)
        {
            key = "life" + lifeID + "x";
            if (PlayerPrefs.HasKey(key))
            {
                float x = PlayerPrefs.GetFloat(key);
                key = "life" + lifeID + "y";
                float y = PlayerPrefs.GetFloat(key);
                key = "life" + lifeID + "z";
                float z = PlayerPrefs.GetFloat(key);
                key = "life" + lifeID + "type";
                int type = PlayerPrefs.GetInt(key);
                Debug.Log("Loading : "+ x + ", " + y + ", " + z + "," + type);
                Instantiate(previousLifeModels[type], new Vector3(x, y, z), previousLifeModels[type].transform.rotation);
                //previousLifeModels[type].transform.localPosition = new Vector3(x, y, z);
                lifeID += 1;
            }
            else
            {
                nextLifeID = lifeID;
                foundKey = false;
            }

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void storeLife(Vector3 location, int type)
    {
        Debug.Log("Saving: " + location.x + ", " + location.y + ", " + location.z + "," + type);

        //Record the tombstone cordinates
        key = "life" + nextLifeID + "x";
        PlayerPrefs.SetFloat(key, location.x);
        key = "life" + nextLifeID + "y";
        PlayerPrefs.SetFloat(key, location.y);
        key = "life" + nextLifeID + "z";
        PlayerPrefs.SetFloat(key, location.z);
        key = "life" + nextLifeID + "type";
        PlayerPrefs.SetInt(key, type);

        nextLifeID += 1;
    }


    private void OnApplicationQuit()
    {
        // Clear the bodys
        PlayerPrefs.DeleteAll();
    }


}
