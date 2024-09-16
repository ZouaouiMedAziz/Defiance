using UnityEngine;

public class test : MonoBehaviour
{
    void Start()
    {
        // Check if there is at least one connected microphone
        if (Microphone.devices.Length > 0)
        {
            // Request microphone access by starting a recording (without saving audio)
            Microphone.Start(null, true, 1, 44100);
            Debug.Log("Microphone access requested.");
        }
        else
        {
            Debug.LogWarning("No microphones found on this device.");
        }
    }
   
}
