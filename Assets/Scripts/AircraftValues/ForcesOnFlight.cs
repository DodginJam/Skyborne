using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The representation of the four main forces acting on a fixed wing aircraft.
/// </summary>
public class ForcesOnFlight
{
    public float Weight
    { get; private set; }

    public float Lift
    { get; private set; }

    public float Thrust
    { get; private set; }

    public float Drag
    { get; private set; }
}
