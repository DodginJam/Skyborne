using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class representing the intrinsic values of an aircraft that affect its flight peformance.
/// </summary>
[CreateAssetMenu(fileName = "AircraftBaseValues", menuName = "AircraftBaseValues/Aircraft")]
public class AircraftBaseValues : ScriptableObject
{
    /// <summary>
    /// The maximum amount of thrust that the engine is able to produce.
    /// </summary>
    [field: SerializeField, Min(0)]
    public float ThrustMax
    { get; private set; } = 50.0f;

    /// <summary>
    /// The rate at which the the throttle value changes in response to player input.
    /// </summary>
    [field: SerializeField, Range(0.1f, 10.0f), Min(0.1f)]
    public float ThrottleSpeedOfChange
    { get; private set; } = 1.0f;

    /// <summary>
    /// The weight of the plane.
    /// </summary>
    [field: SerializeField, Min(0)]
    public float Weight
    { get; private set; }

    /// <summary>
    /// The max degree of rotation the elevator can be turned (absolute value).
    /// </summary>
    [field: SerializeField]
    public float ElevatorDegreeLimit
    { get; private set; }

    /// <summary>
    /// The max degree of rotation the aileron can be turned (absolute value).
    /// </summary>
    [field: SerializeField]
    public float AileronDegreeLimit
    { get; private set; }

    /// <summary>
    /// The max degree of rotation the rudder can be turned (absolute value).
    /// </summary>
    [field: SerializeField]
    public float RudderDegreeLimit
    { get; private set; }

    /// <summary>
    /// The drag coefficent values for the aircraft axis - ideally forward (z positive) should provide the least amount of drag, with the other values providing increased drag.
    /// </summary>
    [field: SerializeField]
    public DragCoefficient DragCoefficientValues
    { get; private set; }

    /// <summary>
    /// Animation Curve used to represent the Lift coefficent relationship with the angle of attack of the plane.
    /// </summary>
    [field: SerializeField]
    public AnimationCurve LiftCurve
    { get; private set; }
}

/// <summary>
/// Class containing the coefficent values for the drag of an aircraft based upon the axis the aircraft is facing upon travelling.
/// </summary>
[Serializable]
public class DragCoefficient
{
    [field: SerializeField]
    public float AxisX_Pos
    { get; private set; } = 2;

    [field: SerializeField]
    public float AxisX_Neg
    { get; private set; } = 2;


    [field: SerializeField]
    public float AxisY_Pos
    { get; private set; } = 2;

    [field: SerializeField]
    public float AxisY_Neg
    { get; private set; } = 2;


    [field: SerializeField]
    public float AxisZ_Pos
    { get; private set; } = 1;

    [field: SerializeField]
    public float AxisZ_Neg
    { get; private set; } = 2;
}
