using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The mprimary flight controls of a fixed wing aircraft which influence the forward flight angle and rotation of an aircraft.
/// </summary>
public class PrimaryFlightControls
{
    public float ElevatorDegree
    { get; private set; }

    public float AileronDegree
    { get; private set; }

    public float RudderDegree
    { get; private set; }
}
