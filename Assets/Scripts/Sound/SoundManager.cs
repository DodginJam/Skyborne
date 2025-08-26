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

    const string MIXER_MASTER = "MasterVolume";
    const string MIXER_MUSIC = "MusicVolume";

    public List<OptionsMenu> OptionsMenusForEvents
    { get; private set; } = new List<OptionsMenu>();

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
        float logValue = Mathf.Log10(newValue) * 20;

        AudioMixerAsset.SetFloat(MIXER_MASTER, logValue);
    }

    public void AdjustSoundMusic(float newValue)
    {
        float logValue = Mathf.Log10(newValue) * 20;

        AudioMixerAsset.SetFloat(MIXER_MUSIC, logValue);
    }

    public void AddOrRemoveOptionsMenu(OptionsMenu optionsMenu, bool addTrueRemoveFalse)
    {
        if (addTrueRemoveFalse)
        {
            OptionsMenusForEvents.Add(optionsMenu);

            optionsMenu.AdjustMasterSound += AdjustSoundMaster;
            optionsMenu.AdjustMusicSound += AdjustSoundMusic;
        }
        else
        {
            OptionsMenusForEvents.Remove(optionsMenu);

            optionsMenu.AdjustMasterSound -= AdjustSoundMaster;
            optionsMenu.AdjustMusicSound -= AdjustSoundMusic;
        }
    }
}
