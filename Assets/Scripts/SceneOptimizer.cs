using System.Collections.Generic;
using UnityEngine;
public class SceneOptimizer : MonoBehaviour
{
    public Transform playerTransform;    // Player reference
    public float disableDistance = 50f;  // Distance to disable objects
    public float checkInterval = 2f;     // Time interval to check objects (seconds)

    private List<GameObject> sceneObjects = new List<GameObject>();

    void Start()
    {
        // Find all objects tagged as "Optimizable" and add them to the list
        GameObject[] optimizableObjects = GameObject.FindGameObjectsWithTag("Optimizable");
        sceneObjects.AddRange(optimizableObjects);

        // Repeatedly check distances at intervals
        InvokeRepeating("CheckAndOptimize", checkInterval, checkInterval);
    }

    void CheckAndOptimize()
    {
        foreach (GameObject obj in sceneObjects)
        {
            if (obj != null)
            {
                // Calculate the distance between the player and the object
                float distance = Vector3.Distance(playerTransform.position, obj.transform.position);

                // Disable the object if it's beyond the disableDistance, enable it if within range
                if (distance > disableDistance && obj.activeSelf)
                {
                    obj.SetActive(false);  // Deactivate the object
                }
                else if (distance <= disableDistance && !obj.activeSelf)
                {
                    obj.SetActive(true);   // Reactivate the object
                }
            }
        }
    }
}