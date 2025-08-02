using System;
using UnityEngine;
using UnityEngine.VFX;
using System.Collections.Generic;

public class AircraftVisualEffectsManager : MonoBehaviour
{
    [field: SerializeField]
    public List<LightEffectData> LightEffectData
    { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[Serializable]
public abstract class AircraftVFXData<T>
{
    [field: SerializeField]
    public string EffectName
    { get; private set; }

    [field: SerializeField]
    public T Effect
    { get; set; }
}

[Serializable]
public class LightEffectData : AircraftVFXData<Light>
{

}

[Serializable]
public class VisualEffectData : AircraftVFXData<VisualEffect>
{

}
