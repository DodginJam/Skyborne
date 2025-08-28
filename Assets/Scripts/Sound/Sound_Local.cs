using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Sound_Local : MonoBehaviour
{
    public AudioSource AudioSourceComp
    {  get; private set; }

    [field: SerializeField]
    public AudioClip LocalAudio
    { get; private set; }

    [field: SerializeField]
    public bool Allow2DSound
    { get; private set; }

    private void Awake()
    {
        if (TryGetComponent<AudioSource>(out AudioSource audioSource))
        {
            AudioSourceComp = audioSource;
        }
        else
        {
            Debug.LogError($"Unable to locate the audiosource on the same transform level for {gameObject.name} at global position {transform.position}");
            return;
        }
        
        InitialiseAudioSource(AudioSourceComp);
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
    /// For the local sound source, initialise the values of the audio source as expected for 3D sounds.
    /// </summary>
    /// <param name="audioSource"></param>
    void InitialiseAudioSource(AudioSource audioSource)
    {
        if (!Allow2DSound)
        {
            audioSource.spatialBlend = 1;
        }
    }
}
