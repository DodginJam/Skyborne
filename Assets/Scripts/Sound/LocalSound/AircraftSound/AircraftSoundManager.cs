using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class AircraftSoundManager : Sound_Local_Manager
{
    [field: SerializeField, Header("Engine")]
    public Sound_Local EngineSound
    { get; private set; }

    [field: SerializeField]
    public float MaxEngineVolume
    { get; private set; }

    [field: SerializeField]
    public float MinEngineVolume
    { get; private set; }

    [field: SerializeField]
    public float MaxEnginePitch
    { get; private set; }

    [field: SerializeField]
    public float MinEnginePitch
    { get; private set; }
    
    public PrimaryFlightControls FlightValues
    { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        InitialiseEngineValues();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        FlightValues = GameObject.FindAnyObjectByType<AircraftCurrentValues>().FlightControls;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        float throttleValue = FlightValues.ThrottleValue;

        EngineSound.AdjustVolume(CalculateNewSoundValue(throttleValue, MinEngineVolume, MaxEngineVolume));
        EngineSound.AdjustPitch(CalculateNewSoundValue(throttleValue, MinEnginePitch, MaxEnginePitch));
    }

    public void InitialiseEngineValues()
    {
        EngineSound.AdjustPitch(MinEnginePitch);
        EngineSound.AdjustVolume(MinEngineVolume);
    }

    public float CalculateNewSoundValue(float currentNormalisedValue, float minSoundValue, float maxSoundValue)
    {
        float lerpedSoundValue = Mathf.Lerp(minSoundValue, maxSoundValue, currentNormalisedValue);

        return lerpedSoundValue;
    }
}
