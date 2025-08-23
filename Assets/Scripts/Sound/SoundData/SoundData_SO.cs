using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "new SoundData", menuName = "Sound/SoundData")]
public class SoundData_SO : ScriptableObject
{
    [field: SerializeField]
    public AudioClip AudioClip
    { get; private set; }

    [field: SerializeField, Range(0, 1)]
    public float BaseVolume
    { get; private set; }

    [field: SerializeField, Range(0, 1)]
    public float StartingVolume
    { get; private set; }

    [field: SerializeField, Range(0, 10)]
    public float FadeInDuration
    { get; private set; }

    [field: SerializeField, Range(0, 1)]
    public float EndingVolume
    { get; private set; }

    [field: SerializeField, Range(0, 10)]
    public float FadeOutDuration
    { get; private set; }

    [field: SerializeField]
    public bool IsLooping
    { get; private set; }
}
