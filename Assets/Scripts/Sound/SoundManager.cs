using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[DefaultExecutionOrder(-500)]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance
    { get; private set; }

    [field: SerializeField]
    public AudioMixer AudioMixerAsset
    { get; private set; }

    public MusicManager MusicManagerScript
    { get; private set; }

    public string MIXER_MASTER 
    { get; private set; }= "MasterVolume";

    public string MIXER_MUSIC
    { get; private set; } = "MusicVolume";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        MusicManagerScript = GetComponentInChildren<MusicManager>();
    }

    public void AdjustSoundMaster(float newValue)
    {
        UI_SoundMixerControls.ChangeNormalisedValueToMixer(newValue, AudioMixerAsset, MIXER_MASTER);
    }

    public void AdjustSoundMusic(float newValue)
    {
        UI_SoundMixerControls.ChangeNormalisedValueToMixer(newValue, AudioMixerAsset, MIXER_MUSIC);
    }

    public void AddOrRemoveUI_SoundListeners(UI_SoundMixerControls uiSound, bool addTrueRemoveFalse)
    {
        if (addTrueRemoveFalse)
        {
            uiSound.AdjustMasterSound += AdjustSoundMaster;
            uiSound.AdjustMusicSound += AdjustSoundMusic;
        }
        else
        {
            uiSound.AdjustMasterSound -= AdjustSoundMaster;
            uiSound.AdjustMusicSound -= AdjustSoundMusic;
        }
    }
}
