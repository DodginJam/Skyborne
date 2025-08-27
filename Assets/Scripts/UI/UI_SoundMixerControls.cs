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

    public void InitialseListeners();

    public static void ChangeMixerValueToNormalised(AudioMixer audioMixer, string floatToChange, Slider sliderToAdjust)
    {
        if (audioMixer.GetFloat(floatToChange, out float valueMaster))
        {
            float normalisedValue = Mathf.Pow(10f, valueMaster / 20f);
            sliderToAdjust.value = normalisedValue;
        }
    }

    public static void ChangeNormalisedValueToMixer(float newValue, AudioMixer audioMixer, string floatToChange)
    {
        float logValue = Mathf.Log10(newValue) * 20;

        audioMixer.SetFloat(floatToChange, logValue);
    }
}
