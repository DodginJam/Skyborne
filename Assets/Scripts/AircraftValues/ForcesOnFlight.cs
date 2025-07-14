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
    public Vector3 Lift
    { get; set; }

    /// <summary>
    /// The calulated forward force produced by the engine.
    /// </summary>
    public float Thrust
    { get; set; } = 0;

    /// <summary>
    /// The calulated resistance to forward thrust (a backwards acting force) determined by the speed at which the aircraft is moving through the air.
    /// </summary>
    public Vector3 Drag
    { get; set; }

    /// <summary>
    /// Calculate the thrust force output from the inputted throttle value and the maximum available thrust.
    /// </summary>
    /// <param name="throttleValue"></param>
    /// <param name="maximumThrust"></param>
    /// <returns></returns>
    public static float CalculateThrustForce(float throttleValue, float maximumThrust)
    {
        // Converting the throttle value to the thrust output.
        return throttleValue * maximumThrust;
    }

    /// <summary>
    /// Calculate the drag on the given transform object based on it's current world velocity 
    /// </summary>
    /// <param name="planeTransform"></param>
    /// <param name="currentWorldVelocity"></param>
    /// <returns></returns>
    public static Vector3 CalculateDragVelocity(Transform planeTransform, Vector3 currentWorldVelocity, DragCoefficient dragCoefficient, AircraftValuesHolder valuesHolder)
    {
        Vector3 dragForceLocal = new Vector3(
            valuesHolder.CurrentVelocityLocal.x * ((valuesHolder.CurrentVelocityLocal.x > 0) ? dragCoefficient.AxisX_Pos : dragCoefficient.AxisX_Neg),
            valuesHolder.CurrentVelocityLocal.y * ((valuesHolder.CurrentVelocityLocal.y > 0) ? dragCoefficient.AxisY_Pos : dragCoefficient.AxisY_Neg),
            valuesHolder.CurrentVelocityLocal.z * ((valuesHolder.CurrentVelocityLocal.z > 0) ? dragCoefficient.AxisZ_Pos : dragCoefficient.AxisZ_Neg)
        );

        float dragForceLength = dragForceLocal.magnitude;

        if (dragForceLength < 0.0001f)
        {
            return Vector3.zero;
        }

        Vector3 dragDirectionLocal = -dragForceLocal / dragForceLength;
        float dragMagnitude = dragForceLocal.sqrMagnitude; // more stable than v^3

        Vector3 dragDirectionWorld = planeTransform.TransformDirection(dragDirectionLocal);
        Vector3 totalDragForce = dragDirectionWorld * dragMagnitude;

        return totalDragForce;
    }

    /// <summary>
    /// Calculating the lift through taking the calculated AngleOfAttack and sampling it's position on the aircrafts starting values AngleOfAttack to Lift Coefficent Animation Curve.
    /// </summary>
    /// <param name="rightAxis"></param>
    /// <param name="liftPower"></param>
    /// <param name="angleOfAttackCurve"></param>
    /// <param name="planeTransform"></param>
    /// <param name="valuesHolder"></param>
    /// <returns></returns>
    public static Vector3 CalculateLift(Vector3 rightAxis, float liftPower, AnimationCurve angleOfAttackCurve, Transform planeTransform, AircraftValuesHolder valuesHolder)
    {
        /*
        Vector3 liftVelocity = Vector3.ProjectOnPlane(valuesHolder.CurrentVelocityLocal, rightAxis);
        float liftSquared = liftVelocity.sqrMagnitude;

        float liftCoefficent = angleOfAttackCurve.Evaluate(Mathf.Clamp(angleOfAttack * Mathf.Rad2Deg, -90f, 90f));
        float liftForce = liftSquared * liftCoefficent * liftPower;

        Vector3 lifeDirection = Vector3.Cross(liftVelocity.normalized, rightAxis);

        return lifeDirection * liftForce;
        */

        // Last working implementation.
        /*
        float cl = angleOfAttackCurve.Evaluate(Mathf.Clamp(valuesHolder.AngleOfAttack, -90f, 90f));
        float liftForce = 0.5f * cl * valuesHolder.CurrentVelocityLocal.sqrMagnitude * liftPower;

        Vector3 liftDir = Vector3.Cross(valuesHolder.CurrentVelocityLocal.normalized, planeTransform.right);
        Vector3 lift = liftDir.normalized * liftForce;

        return lift;
        */

        // Project velocity onto plane perpendicular to the wing
        Vector3 liftVelocity = Vector3.ProjectOnPlane(valuesHolder.CurrentVelocityLocal, rightAxis);

        // Evaluation of the animation curve taking place - clamped in the region of possible degrees of rotation in a given direction.
        float cl = angleOfAttackCurve.Evaluate(Mathf.Clamp(valuesHolder.AngleOfAttack, -90f, 90f));

        // The lift equation here calculates the force being generated from lift.
        float liftForce = 0.5f * cl * liftVelocity.sqrMagnitude * liftPower;

        // The lift direction is applied correctly.
        Vector3 liftDir = Vector3.Cross(liftVelocity.normalized, rightAxis);

        return liftDir.normalized * liftForce;
    }
}
