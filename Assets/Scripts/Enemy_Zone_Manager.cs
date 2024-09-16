using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.Collections; // This is needed for IEnumerator
using UnityEngine.SceneManagement; // Import SceneManagement to reload the scene

public class Enemy_Zone_Manager : MonoBehaviour
{
    public static Action<Enemy_Zone_Manager> OnEnemyZoneEnter;
    public static Action<Enemy_Zone_Manager> OnEnemyZoneComplete;

    [SerializeField] private List<Enemy> enemies = new List<Enemy>();

    // Reference to the zone where the player can move (as a sphere)
    [SerializeField] private Transform zoneCenter;  // The center of the zone (often the position of the object)
    [SerializeField] private float zoneRadius = 10f;  // The radius of the allowed zone
    [SerializeField] private bool isLastZone = false; // Check if this is the last zone
    [Header("Video Player")]
    [SerializeField] private VideoPlayer videoPlayer; // Reference to the Video Player for the full-screen video

    private Transform playerTransform;  // Reference to the player to control their position

        private void Start()
    {
     

        // If it's the last zone, ensure the video player is set up
       if (isLastZone && videoPlayer != null)
    {
            /*if (videoPlayer.renderMode == VideoRenderMode.CameraNearPlane && videoPlayer.targetCamera == null)
            {
                videoPlayer.targetCamera = Camera.main; // Assign the main camera if it's not set
            }
            videoPlayer.gameObject.SetActive(false); // Ensure video is not playing initially*/
            SceneManager.LoadScene("Video Last Scene 1");
        }
    }
    private void OnEnable()
    {
        Enemy.OnEnemyDeath += OnEnemyDeath;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDeath -= OnEnemyDeath;
    }

    private void OnEnemyDeath(Enemy enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }

        if (enemies.Count == 0)
        {
            Debug.Log("All enemies defeated");
            

            // Notify listeners that the zone is complete
            OnEnemyZoneComplete?.Invoke(this);
            if (isLastZone)
            {
                HandleLastZoneCompletion();
            }
            else
            {
                // Deactivate this zone if it's not the last one
                this.gameObject.SetActive(false);
                playerTransform = null; // Allow the player to exit the zone
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered enemy zone");

            // Notify listeners that the player entered this zone
            OnEnemyZoneEnter?.Invoke(this);

            // Set the reference to the player
            playerTransform = other.transform;
        }
    }

    // Method to limit the player's position to the zone
    public Vector3 LimitPositionToZone(Vector3 playerPosition)
    {
        // Calculate the distance between the player and the center of the zone
        float distanceFromCenter = Vector3.Distance(playerPosition, zoneCenter.position);

        // If the player exceeds the zone boundary, bring them back to the boundary
        if (distanceFromCenter > zoneRadius)
        {
            Vector3 directionToCenter = (playerPosition - zoneCenter.position).normalized;
            return zoneCenter.position + directionToCenter * zoneRadius;
        }

        // If the player is still within the zone, return the original position
        return playerPosition;
    }
        private void HandleLastZoneCompletion()
{
    Debug.Log("Last zone completed. Playing video and pausing game.");
        SceneManager.LoadScene("videoLastScene 1");

    }

private IEnumerator PauseGameAfterVideoStarts()
{
    // Wait for a short moment to allow the video to start playing
    yield return new WaitForSecondsRealtime(0.2f);

    // Now pause the game
    Time.timeScale = 0f;
}
    private void OnDrawGizmos()
    {
        if (zoneCenter != null)
        {
            Gizmos.color = Color.red; // Choose a color for the gizmo
            Gizmos.DrawWireSphere(zoneCenter.position, zoneRadius); // Draw the wireframe sphere representing the zone
        }
    }
}
