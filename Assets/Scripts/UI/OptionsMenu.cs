using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour, UI_SoundMixerControls
{
    [field: SerializeField]
    public Slider MasterVolumeSlider
    { get; private set; }

    [field: SerializeField]
    public Slider MusicVolumeSlider
    { get; private set; }

    [field: SerializeField]
    public Slider SFXVolumeSlider
    { get; private set; }

    public event Action<float> AdjustMasterSound;
    public event Action<float> AdjustMusicSound;
    public event Action<float> AdjustSFXSound;

    private void Awake()
    {
        UI_SoundMixerControls.InitialiseSlider(MasterVolumeSlider);
        UI_SoundMixerControls.InitialiseSlider(MusicVolumeSlider);
        UI_SoundMixerControls.InitialiseSlider(SFXVolumeSlider);

        InitialseListeners();

        // Set the slider values to reflect the current audio levels of the sound mixer upon a new options menu being loaded in.
        if (SoundManager.Instance != null)
        {
            UI_SoundMixerControls.ChangeMixerValueToNormalised(SoundManager.Instance.AudioMixerAsset, SoundManager.Instance.MIXER_MASTER, MasterVolumeSlider);

            UI_SoundMixerControls.ChangeMixerValueToNormalised(SoundManager.Instance.AudioMixerAsset, SoundManager.Instance.MIXER_MUSIC, MusicVolumeSlider);

            UI_SoundMixerControls.ChangeMixerValueToNormalised(SoundManager.Instance.AudioMixerAsset, SoundManager.Instance.MIXER_SFX, SFXVolumeSlider);
        }
    }

    private void OnEnable()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.AddOrRemoveUI_SoundListeners(this, true);
        }
    }

    private void OnDisable()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.AddOrRemoveUI_SoundListeners(this, false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitialseListeners()
    {
        MasterVolumeSlider.onValueChanged.AddListener(value => AdjustMasterSound?.Invoke(value));
        MusicVolumeSlider.onValueChanged.AddListener(value => AdjustMusicSound?.Invoke(value));
        SFXVolumeSlider.onValueChanged.AddListener(value => AdjustSFXSound?.Invoke(value));
    }
}
