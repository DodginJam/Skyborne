using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftCurrentValues : MonoBehaviour
{
    /// <summary>
    /// The values of the aircraft being used for the flight calculations.
    /// </summary>
    [field: SerializeField]
    public AircraftBaseValues BaseValues
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

    /// <summary>
    /// Container for the various required calculated values used and reference multiple times in update loop.
    /// </summary>
    public AircraftValuesHolder ValuesHolder
    { get; private set; }

    private void Awake()
    {
        InitialiseAircraftValues();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitialiseAircraftValues()
    {
        // Null checks.
        if (BaseValues == null)
        {
            Debug.LogError("No Aircraft Base Values have been passed through.");
            return;
        }

        if (FlightForces == null) FlightForces = new ForcesOnFlight();

        if (FlightControls == null) FlightControls = new PrimaryFlightControls();

        if (ValuesHolder == null) ValuesHolder = new AircraftValuesHolder();

        FlightForces.Weight = BaseValues.Weight;
    }
}

public class AircraftValuesHolder
{
    /// <summary>
    /// Reference of the current Angle Of Attack of the aircraft.
    /// </summary>
    public float AngleOfAttack
    { get; set; }

    /// <summary>
    /// Reference of the current Angle Of Attack of the aircraft.
    /// </summary>
    public float AngleOfAttackYaw
    { get; set; }

    /// <summary>
    /// The velocity of the aircraft in local space.
    /// </summary>
    public Vector3 CurrentVelocityLocal
    { get; set; }

    /// <summary>
    /// The angular velocity of the aircraft in local space.
    /// </summary>
    public Vector3 CurrentAngularVelocityLocal
    { get; set; }

    /// <summary>
    /// The airspeed of the aircraft in the direction of travel.
    /// </summary>
    public float AirSpeed
    { get; set; }
}
