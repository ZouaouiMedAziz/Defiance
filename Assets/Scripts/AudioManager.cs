using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; // Singleton instance

    [Header("Audio Clips")]
    public AudioClip defaultClip;      // Background or default music
    public AudioClip enemyZoneClip;    // Music for enemy zone
    public AudioClip DogBark;          // Dog barking sound

    private AudioSource audioSource;

    private void Awake()
    {
        // Singleton pattern to ensure only one AudioManager exists
        if (instance == null)
        {
            instance = this;
            audioSource = GetComponent<AudioSource>(); // Ensure an AudioSource is attached to the same GameObject
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is not assigned.");
        }

        // Start by playing the default clip (background music)
        if (defaultClip != null)
        {
            audioSource.clip = defaultClip;
            audioSource.Play();  // Start with the default audio
        }
    }

    // Call this when entering an enemy zone to switch to the enemy zone clip
    public void SetAudioToEnemyZone(float fadeDuration = 1f)
    {
        if (audioSource != null && enemyZoneClip != null)
        {
            // Stop any currently playing sound, including DogBark
            StopAllSounds();

            // Start playing the enemy zone clip
            StartCoroutine(FadeOutAndChangeClip(enemyZoneClip, fadeDuration));
        }
    }

    // Call this when resetting the audio back to the default clip (after leaving the enemy zone)
    public void ResetAudioToDefault(float fadeDuration = 1f)
    {
        if (audioSource != null && defaultClip != null)
        {
            StartCoroutine(FadeOutAndChangeClip(defaultClip, fadeDuration));
        }
    }

    // Stops all currently playing sounds including dog bark
    public void StopAllSounds()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop(); // Stop the current clip playing in the audio source
        }

        // If DogBark is being played by a separate AudioSource, you can stop it here as well
        // You can add more AudioSource references to manage them properly
        // For example, if DogBark is played on another AudioSource, stop that one too
        // Example:
        // if (dogBarkAudioSource != null && dogBarkAudioSource.isPlaying)
        // {
        //     dogBarkAudioSource.Stop();
        // }
    }

    // Fades out the current clip and fades in the new clip
    private IEnumerator FadeOutAndChangeClip(AudioClip newClip, float fadeDuration)
    {
        // Fade out the current audio
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        // Change the clip and play the new one
        audioSource.clip = newClip;
        audioSource.Play();

        // Fade in the new audio
        while (audioSource.volume < startVolume)
        {
            audioSource.volume += startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }
    }

    // Call this method to play the DogBark sound and stop other sounds if necessary
    public void PlayDogBark()
    {
        if (audioSource != null && DogBark != null)
        {
            StopAllSounds(); // Stop other sounds before playing DogBark
            audioSource.clip = DogBark;
            audioSource.Play();
        }
    }
}
