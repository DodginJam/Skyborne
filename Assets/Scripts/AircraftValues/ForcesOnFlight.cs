using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The representation of the four main forces acting on a fixed wing aircraft.
/// </summary>
public class ForcesOnFlight
{
    /// <summary>
    /// The weight of the aircraft - an unchanging value.
    /// </summary>
    public float Weight
    { get; set; } = 0;

    /// <summary>
    /// The calcualted lift the aircraft experiances as a result of calculated airflow under the wings.
    /// </summary>
    public float Lift
    { get; set; } = 0;

    /// <summary>
    /// The calulated forward force produced by the engine.
    /// </summary>
    public float Thrust
    { get; set; } = 0;

    /// <summary>
    /// The calulated resistance to forward thrust (a backwards acting force) determined by the speed at which the aircraft is moving through the air.
    /// </summary>
    public float Drag
    { get; set; } = 0;
}
