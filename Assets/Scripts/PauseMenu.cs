using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel; // Reference to the Pause Menu Panel
    private bool isPaused = false;    // To track if the game is paused

    void Update()
    {
        // Check if the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    // Function to pause the game and show the pause menu
    void PauseGame()
    {
        pauseMenuPanel.SetActive(true);   // Activate the pause menu panel
        Time.timeScale = 0f;              // Pause the game
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Function to resume the game and hide the pause menu
    void ResumeGame()
    {
        pauseMenuPanel.SetActive(false);  // Deactivate the pause menu panel
        Time.timeScale = 1f;              // Resume the game
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
