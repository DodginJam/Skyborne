using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AircraftStartingValues", menuName = "StartingValues/Aircraft")]
public class AircraftStartingValues : ScriptableObject
{
    [field: SerializeField]
    public float ThrustMaxSpeed
    { get; set; } = 1.0f;

    [field: SerializeField]
    public float PitchSpeed
    { get; set; } = 1.0f;

    [field: SerializeField]
    public float RollSpeed
    { get; set; } = 1.0f;

    [field: SerializeField]
    public float YawSpeed
    { get; set; } = 1.0f;
}
