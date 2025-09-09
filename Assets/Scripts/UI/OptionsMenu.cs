using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour, IUI_SoundMixerControls, IUI_Sound
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

    [field: SerializeField]
    public SoundData_SO InteractSound
    { get; set; }

    public event Action<float> AdjustMasterSound;
    public event Action<float> AdjustMusicSound;
    public event Action<float> AdjustSFXSound;

    private void Awake()
    {
        IUI_SoundMixerControls.InitialiseSlider(MasterVolumeSlider);
        IUI_SoundMixerControls.InitialiseSlider(MusicVolumeSlider);
        IUI_SoundMixerControls.InitialiseSlider(SFXVolumeSlider);

        InitialseListeners();

        // Set the slider values to reflect the current audio levels of the sound mixer upon a new options menu being loaded in.
        if (SoundManager.Instance != null)
        {
            IUI_SoundMixerControls.ChangeMixerValueToNormalised(SoundManager.Instance.AudioMixerAsset, SoundManager.Instance.MIXER_MASTER, MasterVolumeSlider);

            IUI_SoundMixerControls.ChangeMixerValueToNormalised(SoundManager.Instance.AudioMixerAsset, SoundManager.Instance.MIXER_MUSIC, MusicVolumeSlider);

            IUI_SoundMixerControls.ChangeMixerValueToNormalised(SoundManager.Instance.AudioMixerAsset, SoundManager.Instance.MIXER_SFX, SFXVolumeSlider);
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

    /// <summary>
    /// Initialise the listnerers of the sliders so that they can adjust the sound mixers they are associated with.
    /// </summary>
    public void InitialseListeners()
    {
        MasterVolumeSlider.onValueChanged.AddListener(value => AdjustMasterSound?.Invoke(value));
        MusicVolumeSlider.onValueChanged.AddListener(value => AdjustMusicSound?.Invoke(value));
        SFXVolumeSlider.onValueChanged.AddListener(value => AdjustSFXSound?.Invoke(value));

        if (this is IUI_Sound soundUI)
        {
            soundUI.ImplementSoundListeners(new List<Selectable>
            {
                MasterVolumeSlider,
                MusicVolumeSlider,
                SFXVolumeSlider,
            });
        }
    }
}
