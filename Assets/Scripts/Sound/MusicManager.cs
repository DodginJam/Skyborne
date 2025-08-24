using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance
    { get; private set; }

    public AudioSource MusicAudioSource
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Initialise()
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

        PlaySoundClipFromGrouping(CurrentSoundGrouping);
    }

    public void MusicChangeOnPauseStatus(bool pauseStatus)
    {
        switch (pauseStatus)
        {
            case true:
                CurrentSoundGrouping = PauseSoundGrouping;
                break;
            case false:
                CurrentSoundGrouping = AmbientSoundGrouping;
                break;
        }

        PlaySoundClipFromGrouping(CurrentSoundGrouping);
    }

    public void PlaySoundClipFromGrouping(SoundGroupings_SO currentSoundGroup)
    {
        MusicAudioSource.clip = currentSoundGroup.SoundData[Random.Range(0, currentSoundGroup.SoundData.Count)].SoundDataSO.AudioClip;
        MusicAudioSource.PlayDelayed(0.1f);
    }
}
