using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Representing the local sound emission of an audio source within a gameobject.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class Sound_Local : MonoBehaviour
{
    public AudioSource AudioSourceComp
    {  get; private set; }

    [field: SerializeField]
    public SoundData_SO LocalAudioData
    { get; private set; }

    [field: SerializeField]
    public bool Allow2DSound
    { get; private set; }

    private void Awake()
    {
        if (TryGetComponent<AudioSource>(out AudioSource audioSource))
        {
            AudioSourceComp = audioSource;
        }
        else
        {
            Debug.LogError($"Unable to locate the audiosource on the same transform level for {gameObject.name} at global position {transform.position}");
            return;
        }
        
        InitialiseAudioSource(AudioSourceComp);
    }

    private void OnEnable()
    {
        // Add the local pausing of sound method to the scene handlers event for pausing.
        SceneHandler.Instance.ScenePause += SoundChangeOnPauseStatus;
    }
    private void OnDisable()
    {
        // Remove the local pausing of sound method to the scene handlers event for pausing.
        SceneHandler.Instance.ScenePause -= SoundChangeOnPauseStatus;
    }

    /// <summary>
    /// Tell the audio source to stop or play depending on the active status of the boolean passed through.
    /// </summary>
    /// <param name="activeStatus"></param>
    public void SoundChangeOnPauseStatus(bool activeStatus)
    {
        if (activeStatus)
        {
            AudioSourceComp.Pause();
        }
        else
        {
            AudioSourceComp.UnPause();
        }
    }

    /// <summary>
    /// For the local sound source, initialise the values of the audio source as expected for 3D sounds.
    /// </summary>
    /// <param name="audioSource"></param>
    void InitialiseAudioSource(AudioSource audioSource)
    {
        if (!Allow2DSound)
        {
            audioSource.spatialBlend = 1;
        }

        audioSource.clip = LocalAudioData.AudioClip;
        audioSource.Play();
    }

    public void AdjustPitch(float newPitch)
    {
        newPitch = Mathf.Clamp(newPitch, -3, 3);

        AudioSourceComp.pitch = newPitch;
    }

    public void AdjustVolume(float newVolume)
    {
        newVolume = Mathf.Clamp(newVolume, 0, 1);

        AudioSourceComp.volume = newVolume;
    }
}
