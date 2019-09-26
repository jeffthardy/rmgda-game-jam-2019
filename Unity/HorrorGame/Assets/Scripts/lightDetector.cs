using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopZombies
{
    public class lightDetector : MonoBehaviour
    {

        private MeshRenderer meshRender;
        public bool isInLight = true;
        public int lightCount = 1;
        public int layerMask = 8;
        public int mode = 0;

        public Material inLight;
        public Material outOfLight;

        private Dictionary<string, bool> lightDictionary = new Dictionary<string, bool>();

        // Start is called before the first frame update
        void Start()
        {
            meshRender = GetComponent<MeshRenderer>();
            // Default player as Green
            //meshRender.material.color = new Color(0, 1, 0);

            // Invert to make it exclude rather than only include it
            layerMask = 1 << layerMask;
            layerMask = ~layerMask;
        }

        // Update is called once per frame
        void Update()
        {
            if (mode == 0)
            {
                if (lightCount <= 0)
                {
                    isInLight = false;
                    meshRender.material.color = new Color(1, 0, 0);
                    meshRender.material.SetColor("_EmissionColor", new Color(1, 0, 0));
                }
                else
                {
                    isInLight = true;
                    meshRender.material.color = new Color(0, 1, 0);
                    meshRender.material.SetColor("_EmissionColor", new Color(0, 1, 0));
                }

            }
            else
            {

                if (lightCount <= 0)
                {
                    isInLight = false;
                    meshRender.material = outOfLight;
                    //Debug.Log("Applying outoflight Texture");

                }
                else
                {
                    isInLight = true;
                    meshRender.material = inLight;
                    //Debug.Log("Applying inlight Texture");
                }
            }
        }


        private void OnTriggerEnter(Collider collider)
        {
            // We entered a light area
            if (collider.gameObject.tag == "light")
            {
                // Now verify raycast that something isn't blocking light source
                RaycastHit hit;
                Vector3 fromPosition = collider.gameObject.transform.position;
                Vector3 toPosition = transform.position;

                // In this case we only want an orthographic collition with the light assuming infinity location
                if (collider.gameObject.name == "WorldLight")
                    fromPosition = new Vector3(toPosition.x, toPosition.y, fromPosition.z);

                Vector3 direction = toPosition - fromPosition;


                if (Physics.Raycast(fromPosition, direction, out hit, Mathf.Infinity, layerMask))
                {
                    if (hit.collider.gameObject.transform == transform)
                    {
                        if (!lightDictionary.ContainsKey(collider.gameObject.name))
                        {
                            lightDictionary.Add(collider.gameObject.name, true);
                            lightCount++;
                        }
                        else
                        {
                            if (lightDictionary[collider.gameObject.name] == false)
                            {
                                lightDictionary[collider.gameObject.name] = true;
                                lightCount++;
                            }
                            //Else do nothing since we already counted it

                        }

                        //Debug.Log("Entered " + collider.gameObject + " , light count is " + lightCount);

                    }
                    else
                    {
                        // It didn't actually hit us due to something blocking it.
                        lightDictionary[collider.gameObject.name] = false;

                    }
                }

            }
        }

        private void OnTriggerStay(Collider collider)
        {
            // We entered a light area
            if (collider.gameObject.tag == "light")
            {
                // Now verify raycast that something isn't blocking light source
                RaycastHit hit;
                Vector3 fromPosition = collider.gameObject.transform.position;
                Vector3 toPosition = transform.position;

                // In this case we only want an orthographic collition with the light assuming infinity location
                if (collider.gameObject.name == "WorldLight")
                    fromPosition = new Vector3(toPosition.x, toPosition.y, toPosition.z - 1000);

                Vector3 direction = toPosition - fromPosition;


                if (Physics.Raycast(fromPosition, direction, out hit, Mathf.Infinity, layerMask))
                {

                    if (hit.collider.gameObject.transform == transform)
                    {
                        if (!lightDictionary.ContainsKey(collider.gameObject.name))
                        {
                            lightDictionary.Add(collider.gameObject.name, true);
                            lightCount++;
                        }
                        else
                        {
                            if (lightDictionary[collider.gameObject.name] == false)
                            {
                                lightDictionary[collider.gameObject.name] = true;
                                lightCount++;
                            }
                            //Else do nothing since we already counted it

                        }
                        //Debug.Log("Stayed in " + collider.gameObject + " , light count is " + lightCount);

                    }
                    // Ray hit some other object instead of us
                    else
                    {
                        if (!lightDictionary.ContainsKey(collider.gameObject.name))
                        {
                            //not sure this can happen
                        }
                        else
                        {
                            if (lightDictionary[collider.gameObject.name] == true)
                            {
                                lightDictionary[collider.gameObject.name] = false;
                                lightCount--;
                            }
                            //Else do nothing since we already counted it

                        }
                        //Debug.Log("Exited due to obstruction of " + collider.gameObject + " by " + hit.collider.gameObject.name + ", light count is " + lightCount);

                    }
                }
                //Ray hit nothing, so light is very far away?
                else
                {

                    if (!lightDictionary.ContainsKey(collider.gameObject.name))
                    {
                        //not sure this can happen
                    }
                    else
                    {
                        if (lightDictionary[collider.gameObject.name] == true)
                        {
                            lightDictionary[collider.gameObject.name] = false;
                            lightCount--;
                        }
                        //Else do nothing since we already counted it

                    }
                    //Debug.Log("Exited " + collider.gameObject + " due to distance?  , light count is " + lightCount);

                }

            }
        }

        private void OnTriggerExit(Collider collider)
        {
            // We left a light area
            if (collider.gameObject.tag == "light")
            {
                // No need to verify raycast since we left it?
                if (!lightDictionary.ContainsKey(collider.gameObject.name))
                {
                    //not sure this can happen
                }
                else
                {
                    if (lightDictionary[collider.gameObject.name] == true)
                    {
                        lightDictionary[collider.gameObject.name] = false;
                        lightCount--;
                    }
                    //Else do nothing since we already counted it

                }
                //Debug.Log("Exited " + collider.gameObject + " , light count is " + lightCount);
            }
        }

    }
}