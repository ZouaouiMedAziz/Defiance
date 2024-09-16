using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwichScriptDogzone2 : MonoBehaviour
{
    public CinemachineVirtualCamera vCam_Dog;     // Reference to dog's camera
    public GameObject Zone;






    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("dog"))
        {
            vCam_Dog.Priority = 30;
            Zone.SetActive(false);

        }
    }

  

    public void FocusOnPlayer()
    {
        

      
       
    }


}
