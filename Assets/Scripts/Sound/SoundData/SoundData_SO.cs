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
}
