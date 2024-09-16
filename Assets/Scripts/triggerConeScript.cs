using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerConeScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        if (meshCollider != null)
        {
            meshCollider.convex = true; // Ensure it's convex
            meshCollider.isTrigger = true; // Set as trigger
        }
    }

  
}
