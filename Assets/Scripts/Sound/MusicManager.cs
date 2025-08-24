using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance
    { get; private set; }

    public AudioSource MusicAudioSource
    { get; private set; }

    [field: SerializeField]
    public SoundGroupings_SO CurrentSoundGrouping
    { get; private set; }

    private void Awake()
    {
        Initialise();
    }

    private void OnEnable()
    {
        SceneHandler.Instance.SceneTransition += MusicOnSceneChange;
    }
    private void OnDisable()
    {
        SceneHandler.Instance.SceneTransition -= MusicOnSceneChange;
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

        if (CurrentSoundGrouping == null)
        {
            Debug.LogError("No Sound Grouping reference provided.");
            return;
        }
        else
        {
            MusicAudioSource.clip = CurrentSoundGrouping.SoundData[0].SoundDataSO.AudioClip;
            MusicAudioSource.Play();
        }
    }

    public void MusicOnSceneChange(int newSceneIndex)
    {
        Debug.Log("Music Changed");
    }
}
