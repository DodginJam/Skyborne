using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(AircraftCurrentValues)), RequireComponent(typeof(AircraftInput))]
public class AircraftController : MonoBehaviour
{
    /// <summary>
    /// Reference to the component where the player input is received and processed by the New Input System into normalised values for aircraft control.
    /// </summary>
    [field: SerializeField]
    public AircraftInput InputControls
    { get; private set; }

    /// <summary>
    /// The values of the aircraft, from it's Flight Controls to the acting forces on the aircraft.
    /// </summary>
    [field: SerializeField]
    public AircraftCurrentValues CurrentValues
    { get; private set; }

    /// <summary>
    /// The rigidbody to be manipulated by the calculated forces acting on the aircraft.
    /// </summary>
    [field: SerializeField]
    public Rigidbody PlaneRigidBody
    {  get; private set; }

    private void Awake()
    {
        // Rigidbody null checking.
        if (PlaneRigidBody == null)
        {
            PlaneRigidBody = GetComponent<Rigidbody>();

            if (PlaneRigidBody == null)
            {
                Debug.LogError("Unable to locate a rigidbody on the aircraft for physics control.");
                return;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        PlaneRigidBody.mass = CurrentValues.FlightForces.Weight;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FixedUpdate()
    {
        InputToFlightControls(InputControls, CurrentValues.FlightControls);
        FlightControlsToForces(CurrentValues.FlightControls, CurrentValues.FlightForces);
        ForcesToRigidBody(CurrentValues.FlightForces, PlaneRigidBody);
    }

    /// <summary>
    /// Transfers the players normalised input values to the elements of the flight control systems, such as throttle and control surfaces.
    /// </summary>
    /// <param name="inputControls"></param>
    /// <param name="flightControls"></param>
    public void InputToFlightControls(AircraftInput inputControls, PrimaryFlightControls flightControls)
    {
        // Calculating the throttles values after any changes in input by the player.
        float newThrottleValue = flightControls.ThrottleValue + (inputControls.ThrottleInput * Time.fixedDeltaTime * CurrentValues.BaseValues.ThrottleSpeedOfChange);

        flightControls.ThrottleValue = Mathf.Clamp01(newThrottleValue);

        // Debug.Log($"Throttle Value:{flightControls.ThrottleValue}");
    }

    /// <summary>
    /// Transfers the flight controls systems values to the forces being applied to the aircraft.
    /// </summary>
    /// <param name="flightControls"></param>
    /// <param name="flightForces"></param>
    public void FlightControlsToForces(PrimaryFlightControls flightControls, ForcesOnFlight flightForces)
    {
        // Converting the throttle value to the thrust output.
        flightForces.Thrust = ForcesOnFlight.CalculateThrustForce(flightControls.ThrottleValue, CurrentValues.BaseValues.ThrustMax);

        flightForces.Drag = ForcesOnFlight.CalculateDragVelocity(PlaneRigidBody.transform, PlaneRigidBody.velocity, CurrentValues.BaseValues.DragCoefficientValues);

        Debug.Log($"Thrust Value:{flightForces.Thrust}");
        Debug.Log($"Drag Value:{flightForces.Drag}");
    }

    /// <summary>
    /// Applies the calculated forces to the aircrafts rigidbody.
    /// </summary>
    /// <param name="flightForces"></param>
    /// <param name="rigidBody"></param>
    public void ForcesToRigidBody(ForcesOnFlight flightForces, Rigidbody rigidBody)
    {
        // Applying the thrust value to the aircraft rigidbody.
        rigidBody.AddForce(transform.forward * flightForces.Thrust, ForceMode.Force);
        rigidBody.AddForce(flightForces.Drag, ForceMode.Force);

        Debug.Log(rigidBody.velocity.magnitude);
    }
}
