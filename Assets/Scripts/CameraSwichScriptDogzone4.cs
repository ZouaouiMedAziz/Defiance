using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwichScriptDogzone4 : MonoBehaviour
{
    public CinemachineVirtualCamera vCam_Dog;     // Reference to dog's camera
    public GameObject Player;
    public GameObject ZoneDisabled;



  


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("dog"))
        {
            vCam_Dog.Priority = 30;
            Player.SetActive(false);
            ZoneDisabled.SetActive(false); 
        }
    }

  

    public void FocusOnPlayer()
    {
        

      
       
    }


}
