using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepsSound : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("FootSteps Sources")]
    [SerializeField] private AudioClip[] footStepSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private AudioClip GetRandomFootStep()
    {
        return footStepSound[UnityEngine.Random.Range(0, footStepSound.Length)];

    }
    private void Step()
    {
       /*AudioClip clip = GetRandomFootStep();
        audioSource.PlayOneShot(clip);*/
       PlayClip(audioSource, footStepSound);
    }
    private void PlayClip(AudioSource source, AudioClip[] clips)
    {
        if (clips.Length == 0) return;

        int index = Random.Range(0, clips.Length);

        source.clip = clips[index];
        source.pitch = Random.Range(.85f, 1.4f);
        source.PlayOneShot(clips[index]);
    }
}
