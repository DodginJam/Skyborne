using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The mprimary flight controls of a fixed wing aircraft which influence the forward flight angle and rotation of an aircraft.
/// </summary>
public class PrimaryFlightControls
{
    /// <summary>
    /// The angle at which the elevator is set to.
    /// </summary>
    public float ElevatorDegree
    { get; set; } = 0;

    /// <summary>
    /// The angle at which the elevator is set to.
    /// </summary>
    public float AileronDegree_Left
    { get; set; } = 0;

    /// <summary>
    /// The angle at which the elevator is set to.
    /// </summary>
    public float AileronDegree_Right
    { get; set; } = 0;

    /// <summary>
    /// The angle at which the elevator is set to.
    /// </summary>
    public float RudderDegree
    { get; set; } = 0;

    /// <summary>
    /// The value of the throttle which directs the power sent to the engine.
    /// </summary>
    public float ThrottleValue
    { get; set; } = 0;

    public static float CalculateCurrentRotation(float normalisedInput, float currentElevatorDegrees, float degreeLimitOfRotation, float speedOfRotation)
    {
        // Calculating the elevator normalised angles of degrees.
        float newDegreeTarget = normalisedInput * degreeLimitOfRotation;

        float newCurrentDegree = Mathf.MoveTowardsAngle(currentElevatorDegrees, newDegreeTarget, speedOfRotation * Time.fixedDeltaTime);

        return newCurrentDegree;
    }
}
