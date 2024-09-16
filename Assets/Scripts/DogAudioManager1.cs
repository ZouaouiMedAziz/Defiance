using UnityEngine;

public class DogAudioManager1 : MonoBehaviour
{
    public AudioClip audioClip;      // The audio clip to play
    private AudioSource audioSource; // The AudioSource component

    private void Start()
    {
        // Get or add the AudioSource component
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // Ensure it doesn't play automatically
    }

    // This method is triggered when another object enters the trigger zone
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the zone has the tag "Player" or any other relevant tag
        if (other.CompareTag("Player")) // Adjust the tag to match your object's tag
        {
            // Play the audio clip
            audioSource.clip = audioClip;
            audioSource.Play();
            Debug.Log("Audio played upon entering the zone.");
        }
    }
}
