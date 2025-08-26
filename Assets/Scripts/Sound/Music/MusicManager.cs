using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public AudioSource MusicAudioSource
    { get; private set; }

    public PlayingAudioData AudioToResume
    { get; private set; }

    public SoundGroupings_SO CurrentSoundGrouping
    { get; private set; }

    [field: SerializeField]
    public SoundGroupings_SO MainMenuSoundGrouping
    { get; private set; }

    [field: SerializeField]
    public SoundGroupings_SO AmbientSoundGrouping
    { get; private set; }

    [field: SerializeField]
    public SoundGroupings_SO DangerSoundGrouping
    { get; private set; }

    [field: SerializeField]
    public SoundGroupings_SO PauseSoundGrouping
    { get; private set; }



    private void Awake()
    {
        Initialise();
    }

    private void OnEnable()
    {
        SceneHandler.Instance.SceneTransition += MusicOnSceneChange;
        SceneHandler.Instance.ScenePause += MusicChangeOnPauseStatus;
    }
    private void OnDisable()
    {
        SceneHandler.Instance.SceneTransition -= MusicOnSceneChange;
        SceneHandler.Instance.ScenePause -= MusicChangeOnPauseStatus;
    }

    void Initialise()
    {
        if (TryGetComponent<AudioSource>(out AudioSource audioSource))
        {
            MusicAudioSource = audioSource;
        }
        else
        {
            Debug.LogError("Unable to locate the required audio source component on object.");
        }
    }

    public void MusicOnSceneChange(int newSceneIndex)
    {
        switch (newSceneIndex)
        {
            case 0:
                CurrentSoundGrouping = MainMenuSoundGrouping;
                break;
            case 1:
                CurrentSoundGrouping = AmbientSoundGrouping;
                break;
            default:
                Debug.Log("Defaulted due to no case match.");
                break;
        }

        AudioToResume = null;
        PlaySoundClipFromGrouping(CurrentSoundGrouping);
    }

    public void MusicChangeOnPauseStatus(bool pauseStatus)
    {
        // Grab the data from the current playing clip before the audio is switched on the pause action.
        PlayingAudioData pausedAudioData = PlayingAudioData.SetPlayingAudioData(MusicAudioSource.clip, MusicAudioSource.time);

        switch (pauseStatus)
        {
            case true:
                CurrentSoundGrouping = PauseSoundGrouping;
                break;

            case false:

                CurrentSoundGrouping = AmbientSoundGrouping;
                break;
        }

        // On pause change, resume the audio to start from the last saved audio clip to resume if it is not null.
        if (AudioToResume != null)
        {
            AudioToResume.PlayAudioFromTime(MusicAudioSource, 0.1f);
        }
        else
        {
            PlaySoundClipFromGrouping(CurrentSoundGrouping);
        }

        // With the old audio now resumed, save the last playing audio to the class so it can now itself be resumed next time.
        AudioToResume = pausedAudioData;
    }

    public void PlaySoundClipFromGrouping(SoundGroupings_SO currentSoundGroup)
    {
        MusicAudioSource.clip = currentSoundGroup.SoundData[UnityEngine.Random.Range(0, currentSoundGroup.SoundData.Count)].SoundDataSO.AudioClip;
        MusicAudioSource.PlayDelayed(0.1f);
    }
}

public class PlayingAudioData
{
    public AudioClip ClipPlaying
    { get; private set; }

    public float TimeForResume
    { get; private set; }

    public static PlayingAudioData SetPlayingAudioData(AudioClip clip, float timeForResume)
    {
        PlayingAudioData newData = new PlayingAudioData();
        newData.ClipPlaying = clip;
        newData.TimeForResume = timeForResume;

        return newData;
    }

    public void PlayAudioFromTime(AudioSource audioSource, float delayPlayTime)
    {
        audioSource.clip = ClipPlaying;
        audioSource.PlayDelayed(0.1f);
        audioSource.time = TimeForResume;
    }
}
