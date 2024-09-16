using UnityEngine;
using UnityEngine.SceneManagement; // For scene loading
using System.Collections;
using System.Collections.Generic;
public class LightedCircleTrigger : MonoBehaviour
{
    public string nextMissionSceneName; // The name of the next mission scene
    private IEnumerator LoadNextMissionWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(nextMissionSceneName);
    }
    private void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the trigger
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the lighted circle, loading next mission...");

            // Load the next mission scene
            StartCoroutine(LoadNextMissionWithDelay(3f)); // 3-second delay
        }
    }

  
}
