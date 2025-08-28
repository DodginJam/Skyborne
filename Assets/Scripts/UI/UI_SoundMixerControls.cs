using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

/// <summary>
/// Interface for UI elements that have events for adjusting the sound mixer values.
/// </summary>
public interface UI_SoundMixerControls
{
    public event Action<float> AdjustMasterSound;

    public event Action<float> AdjustMusicSound;

    public event Action<float> AdjustSFXSound;

    public void InitialseListeners();

    public static void InitialiseSlider(Slider sliderToAdjust)
    {
        sliderToAdjust.wholeNumbers = false;
        sliderToAdjust.minValue = 0.0001f;
        sliderToAdjust.maxValue = 1;
        sliderToAdjust.value = sliderToAdjust.maxValue;
    }

    public static void ChangeMixerValueToNormalised(AudioMixer audioMixer, string floatToChange, Slider sliderToAdjust)
    {
        if (audioMixer.GetFloat(floatToChange, out float valueMaster))
        {
            float normalisedValue = Mathf.Pow(10f, valueMaster / 20f);
            sliderToAdjust.value = normalisedValue;
        }
        else
        {
            Debug.LogError("Unable to locate the float value to be changed on the audio mixer.");
        }
    }

    public static void ChangeNormalisedValueToMixer(float newValue, AudioMixer audioMixer, string floatToChange)
    {
        float logValue = Mathf.Log10(newValue) * 20;

        if (!audioMixer.SetFloat(floatToChange, logValue))
        {
            Debug.LogError("Unable to locate the float value to be changed on the audio mixer.");
        }
    }
}
