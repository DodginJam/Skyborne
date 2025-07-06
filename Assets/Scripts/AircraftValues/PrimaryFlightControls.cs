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
    { get; private set; }

    /// <summary>
    /// The angle at which the elevator is set to.
    /// </summary>
    public float AileronDegree_Left
    { get; private set; }

    /// <summary>
    /// The angle at which the elevator is set to.
    /// </summary>
    public float AileronDegree_Right
    { get; private set; }

    /// <summary>
    /// The angle at which the elevator is set to.
    /// </summary>
    public float RudderDegree
    { get; private set; }
}
