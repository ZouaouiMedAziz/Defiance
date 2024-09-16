using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Find all objects in the scene that have the MyScript component attached
        AmmoCount[] objectsWithScript = FindObjectsOfType<AmmoCount>();

        // Log the names of the GameObjects that have this script
        foreach (AmmoCount obj in objectsWithScript)
        {
            Debug.Log("GameObject with MyScript: " + obj.gameObject.name);
        }
    }
}
