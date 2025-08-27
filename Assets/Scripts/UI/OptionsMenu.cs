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

    public event Action<float> AdjustMasterSound;
    public event Action<float> AdjustMusicSound;

    private void Awake()
    {
        InitialiseSlider(MasterVolumeSlider);
        InitialiseSlider(MusicVolumeSlider);
        InitialseListeners();

        // Set the slider values to reflect the current audio levels of the sound mixer upon a new options menu being loaded in.
        if (SoundManager.Instance != null)
        {
            UI_SoundMixerControls.ChangeMixerValueToNormalised(SoundManager.Instance.AudioMixerAsset, SoundManager.Instance.MIXER_MASTER, MasterVolumeSlider);

            UI_SoundMixerControls.ChangeMixerValueToNormalised(SoundManager.Instance.AudioMixerAsset, SoundManager.Instance.MIXER_MUSIC, MusicVolumeSlider);
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
    }

    void InitialiseSlider(Slider sliderToAdjust)
    {
        sliderToAdjust.wholeNumbers = false;
        sliderToAdjust.minValue = 0.0001f;
        sliderToAdjust.maxValue = 1;
        sliderToAdjust.value = sliderToAdjust.maxValue;
    }
}
