using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftCurrentValues : MonoBehaviour
{
    [field: SerializeField]
    public AircraftStartingValues StartingValues
    {  get; private set; }

    /// <summary>
    /// The forces acting on the current aircraft.
    /// </summary>
    public ForcesOnFlight FlightForces
    { get; private set; }

    /// <summary>
    /// The current degress of rotation that the Primary Flight Controls are being adjusted to via player input.
    /// </summary>
    public PrimaryFlightControls FlightControls
    { get; private set; }

    [field: SerializeField]
    public float ThrustSpeed
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

    private void Awake()
    {
        InitialiseStartingValues();
        FlightForces = new ForcesOnFlight();
        FlightControls = new PrimaryFlightControls();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitialiseStartingValues()
    {
        ThrustSpeed = StartingValues.ThrustMaxSpeed;
        PitchSpeed = StartingValues.PitchSpeed;
        RollSpeed = StartingValues.RollSpeed;
        YawSpeed = StartingValues.YawSpeed;
    }
}
