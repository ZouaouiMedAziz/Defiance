using UnityEngine;
using UnityEngine.UI; // For UI components
using UnityEngine.SceneManagement; // For scene reloading
using TMPro;

public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 30f; // Speed of camera rotation
    public float rotationRange = 45f; // Maximum Y-axis rotation angle
    public Transform[] detectableObjects; // Array of objects (e.g., player, dog) to detect
    public GameObject blackScreenPanel; // The black screen UI panel
    public TMP_Text loseText; // Text to display "You Lose"
    public Button playAgainButton; // The play again button
    public GameObject coneObject; // The detection cone visual

    public float detectionRange = 10f; // The distance the camera can detect the objects
    public Collider detectionTrigger; // The trigger collider that detects objects

    private bool gameOver = false;
    private float currentRotation = 0f; // Current Y-axis rotation

    // Store the camera's initial position and local rotation
    private Quaternion initialRotation;

    private void Start()
    {
        // Save the initial local rotation of the camera
        initialRotation = transform.localRotation;

        // Hide the game-over screen at the start
        blackScreenPanel.SetActive(false);

        // Ensure the detectionTrigger is a trigger collider
        if (detectionTrigger != null)
        {
            detectionTrigger.isTrigger = true; // Set it as a trigger collider
        }
        else
        {
            Debug.Log("No trigger collider assigned for detection!");
        }

        // Adjust the cone scale to match the detection range
        AdjustConeScale();
    }

    private void Update()
    {
        if (!gameOver)
        {
            // Rotate the camera and cone
            RotateCameraAndCone();
        }
    }

    private void RotateCameraAndCone()
    {
        // Calculate smooth oscillation for the Y-axis rotation using Mathf.PingPong
        currentRotation = Mathf.PingPong(Time.time * rotationSpeed, rotationRange * 2) - rotationRange;

        // Apply the rotation only on the Y-axis relative to the initial local rotation
        transform.localRotation = initialRotation * Quaternion.Euler(0, currentRotation, 0);

        // Ensure the cone always points forward relative to the camera
        if (coneObject != null)
        {
            // Reset the local rotation of the cone to make it follow the camera's forward direction
            coneObject.transform.localRotation = Quaternion.identity;
        }
    }

    // Called when another object enters the detection area (trigger)
    private void OnTriggerEnter(Collider other)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Check if the other object is one of the detectable objects
        foreach (Transform detectable in detectableObjects)
        {
            if (!gameOver && other.transform == detectable)
            {
                // Detected object (e.g., player or dog), trigger game over
                TriggerGameOver();
                return;
            }
        }
    }

    // Adjust the cone scale (optional visual effect)
    private void AdjustConeScale()
    {
        if (coneObject != null)
        {
            // Adjust the cone to fit the detection range, if needed
            Vector3 originalScale = coneObject.transform.localScale;
            coneObject.transform.localScale = new Vector3(originalScale.x, originalScale.y, detectionRange);
        }
    }

    private void TriggerGameOver()
    {
        gameOver = true; // Stop camera rotation and detection

        // Display the game-over screen
        blackScreenPanel.SetActive(true);
        loseText.text = "You Lose!"; // Show the "You Lose" message

        // Add listener to the play again button
        playAgainButton.onClick.AddListener(RestartGame);
    }

    private void RestartGame()
    {
        // Reload the current scene to restart the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
