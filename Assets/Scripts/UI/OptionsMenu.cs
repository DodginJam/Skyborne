using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
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
            if (SoundManager.Instance.AudioMixerAsset.GetFloat("MasterVolume", out float valueMaster))
            {
                float normalisedValue = Mathf.Pow(10f, valueMaster / 20f);
                MasterVolumeSlider.value = normalisedValue;
            }

            if (SoundManager.Instance.AudioMixerAsset.GetFloat("MusicVolume", out float valueMusic))
            {
                float normalisedValue = Mathf.Pow(10f, valueMusic / 20f);
                MusicVolumeSlider.value = normalisedValue;
            }
        }
    }

    private void OnEnable()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.AddOrRemoveOptionsMenu(this, true);
        }
        else
        {
            Debug.LogError("Sound Manager Instance is null");
        }
    }

    private void OnDisable()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.AddOrRemoveOptionsMenu(this, false);
        }
        else
        {
            Debug.LogError("Sound Manager Instance is null");
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

    void InitialseListeners()
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
