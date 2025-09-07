using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class SFXManager : MonoBehaviour
{
    public AudioSource SFXAudioSource
    { get; private set; }

    private void Awake()
    {
        Initialise();
    }

    private void OnEnable()
    {

    }
    private void OnDisable()
    {

    }

    void Initialise()
    {
        if (TryGetComponent<AudioSource>(out AudioSource audioSource))
        {
            SFXAudioSource = audioSource;
        }
        else
        {
            Debug.LogError("Unable to locate the required audio source component on object.");
        }
    }

    public void PlayOneShotSFX(AudioClip audioClip)
    {
        if (SFXAudioSource != null)
        {
            SFXAudioSource.PlayOneShot(audioClip, 2.0f);
        }
        else
        {
            Debug.LogError("The audio source has not been assigned.");
        }
    }
}
