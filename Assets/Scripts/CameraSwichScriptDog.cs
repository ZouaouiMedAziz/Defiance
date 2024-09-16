using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwichScriptDog : MonoBehaviour
{
    public CinemachineVirtualCamera vCam_Player;  // Reference to player's camera
    public CinemachineVirtualCamera vCam_Dog;     // Reference to dog's camera
    public DogScript dogScript;
    public PlayerScript playerScript;  // Reference to the player script
    public GameObject Player;
    public GameObject CanvaDoc;
    public GameObject Marker;
    public GameObject MarkerCircle;
    public Vector3 newMarkerPosition;
    public MiniMapMarkerGuide miniMapMarkerGuide; // Reference to MiniMapMarkerGuide script
    public Camera PlayerMiniMapCamera;     // The mini-map camera for the dog



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("dog"))
        {
            FocusOnPlayer();
        }
    }

  

    public void FocusOnPlayer()
    {
        Player.SetActive(true);
       
        CanvaDoc.SetActive(true);
        vCam_Player.Priority = 40;
        vCam_Dog.Priority = 0;
       
        dogScript.EnableFollowing();
        playerScript.EnableMovement(true);  // Enable player movement
        miniMapMarkerGuide.miniMapCamera = PlayerMiniMapCamera;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) // Check if in zone and 'E' is pressed
        {
            Debug.Log("E key pressed");
            CanvaDoc.SetActive(false); // Hide the document UI
            MarkerCircle.SetActive(true); // Hide the document UI

            MoveMarkerToPosition(newMarkerPosition);



        }
    }

    private void MoveMarkerToPosition(Vector3 position)
    {
        if (Marker != null)
        {
            Marker.transform.position = position;
            Debug.Log("Marker moved to: " + position);
        }
        else
        {
            Debug.LogWarning("Marker object is not assigned!");
        }
    }
}
