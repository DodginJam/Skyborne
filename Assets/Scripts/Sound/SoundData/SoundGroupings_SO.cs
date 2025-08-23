using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new SoundGrouping", menuName = "Sound/SoundGrouping")]
public class SoundGroupings_SO : ScriptableObject
{
    [field: SerializeField]
    public List<SoundData> SoundData
    { get; private set; }
}

[Serializable]
public class SoundData
{
    [field: SerializeField]
    public string Name 
    { get; private set; }

    [field: SerializeField]
    public SoundData_SO SoundDataSO
    { get; private set; }
}
