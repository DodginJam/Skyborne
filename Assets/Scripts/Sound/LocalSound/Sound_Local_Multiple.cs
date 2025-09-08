using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Representing the local sound emission of an audio source within a gameobject.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class Sound_Local_Multiple : MonoBehaviour
{
    public AudioSource AudioSourceComp
    {  get; private set; }

    [field: SerializeField]
    public SoundGroupings_SO LocalAudioData
    { get; private set; }

    [field: SerializeField]
    public bool Allow2DSound
    { get; private set; }

    [field: SerializeField, Min(1)]
    public float MinTimeBetweenSound
    { get; private set; }

    [field: SerializeField, Min(2)]
    public float MaxTimeBetweenSound
    { get; private set; }

    public List<int> SoundIndexOrder
    { get; private set; } = new List<int>();

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

    private void OnEnable()
    {
        // Add the local pausing of sound method to the scene handlers event for pausing.
        SceneHandler.Instance.ScenePause += SoundChangeOnPauseStatus;
    }
    private void OnDisable()
    {
        // Remove the local pausing of sound method to the scene handlers event for pausing.
        SceneHandler.Instance.ScenePause -= SoundChangeOnPauseStatus;
    }

    /// <summary>
    /// Tell the audio source to stop or play depending on the active status of the boolean passed through.
    /// </summary>
    /// <param name="activeStatus"></param>
    public void SoundChangeOnPauseStatus(bool activeStatus)
    {
        if (activeStatus)
        {
            AudioSourceComp.Pause();
        }
        else
        {
            AudioSourceComp.UnPause();
        }
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
        else
        {
            audioSource.spatialBlend = 0;
        }

        StartCoroutine(RadioPlayer());
    }

    /// <summary>
    /// Provide a random list of indexs for the multiple sounds so they are played in randomised orders.
    /// </summary>
    /// <param name="soundGroupings"></param>
    /// <returns></returns>
    public List<int> ReDrawIndexOrder(SoundGroupings_SO soundGroupings)
    {
        // Create a list of indices matching the original SoundData
        List<int> indexPool = new List<int>();

        for (int i = 0; i < soundGroupings.SoundData.Count; i++)
        {
            indexPool.Add(i);
        }

        // This will hold the randomized index order
        List<int> indexOrder = new List<int>();

        // Randomly select indices until none remain
        while (indexPool.Count > 0)
        {
            int randomIdx = UnityEngine.Random.Range(0, indexPool.Count);
            indexOrder.Add(indexPool[randomIdx]);
            indexPool.RemoveAt(randomIdx);
        }

        return indexOrder;
    }

    public IEnumerator RadioPlayer()
    {
        bool isAudioPlaying = false;
        float timerCurrent = 0;
        float timeToWait = UnityEngine.Random.Range(MinTimeBetweenSound, MaxTimeBetweenSound);

        while (true)
        {
            if (SoundIndexOrder.Count <= 0)
            {
                SoundIndexOrder = ReDrawIndexOrder(LocalAudioData);
            }

            if (isAudioPlaying == false)
            {
                if (timerCurrent < timeToWait)
                {
                    timerCurrent += Time.deltaTime;
                    yield return null;
                }
                else if (timerCurrent >= timeToWait)
                {
                    AudioSourceComp.clip = LocalAudioData.SoundData[SoundIndexOrder[0]].SoundDataSO.AudioClip;
                    AudioSourceComp.Play();

                    SoundIndexOrder.RemoveAt(0);

                    // Reset the time figures.
                    timerCurrent = 0;
                    timeToWait = UnityEngine.Random.Range(MinTimeBetweenSound, MaxTimeBetweenSound);

                    // Set audio playing to true.
                    isAudioPlaying = true;
                }
            }
            else if (isAudioPlaying == true)
            {
                // Wait for the length of the audio clip before returning to coroutine loop.
                yield return new WaitForSeconds(AudioSourceComp.clip.length);

                // After the length of the clip, set the booleon to false.
                isAudioPlaying = false;
                AudioSourceComp.clip = null;
            }
        }
    }
}
