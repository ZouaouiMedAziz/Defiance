using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneScript : MonoBehaviour
{

    public bool isSeeingFolder = false;
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isSeeingFolder = true;
        }
    }
}
