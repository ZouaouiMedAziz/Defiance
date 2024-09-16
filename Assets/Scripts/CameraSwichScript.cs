using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwichScript : MonoBehaviour
{
    public CinemachineVirtualCamera vCam_Player;  // Reference to player's camera
    public CinemachineVirtualCamera vCam_Dog;     // Reference to dog's camera
    public DogScript dogScript;
    public GameObject dog;
    public GameObject Guid;

    public ZoneScript zoneScript;
    public PlayerScript playerScript;  // Reference to the player script
    public GameObject document; // The document GameObject that will appear


    void Start()
    {
        dogScript.DisableFollowing();
        Guid.SetActive(false);
        FocusOnPlayer();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (zoneScript.isSeeingFolder==true) {
                dog.SetActive(true);
                Guid.SetActive(true);
                document.SetActive(false);
                FocusOnDog();
            }
        }
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Tab)){
        openGuid();

        }
        if(Input.GetKeyDown(KeyCode.Return)){

        closeGuid();
        }
    }

    public void openGuid()
    {
      Guid.SetActive(true);

    }
     public void closeGuid()
    {
      Guid.SetActive(false);

    }


    public void FocusOnDog()
    {
        vCam_Dog.Priority = 10;
        vCam_Player.Priority = 0;

        dogScript.DisableFollowing();
        playerScript.EnableMovement(false);  // Disable player movement
        dogScript.StartKeywordRecognizer();

    }

    public void FocusOnPlayer()
    {
        vCam_Player.Priority = 10;
        vCam_Dog.Priority = 0;

        dogScript.EnableFollowing();
        playerScript.EnableMovement(true);  // Enable player movement
    }


}
