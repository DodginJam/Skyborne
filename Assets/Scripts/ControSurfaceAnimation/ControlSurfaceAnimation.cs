using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(20)]
public class ControlSurfaceAnimation : MonoBehaviour
{
    [field: SerializeField]
    public PrimaryFlightControls FlightControls
    { get; private set; }

    [field: SerializeField]
    public List<ControlSurface> ControlSurfaces
    { get; private set; }

    private void Awake()
    {
        if (FlightControls == null)
        {
            if (TryGetComponent(out AircraftCurrentValues aircraftCurrentValues))
            {
                if (aircraftCurrentValues.FlightControls != null)
                {
                    FlightControls = aircraftCurrentValues.FlightControls;
                }
                else
                {
                    Debug.LogError("Unable to locate the flight controls script for the aircraft");
                    return;
                }
            }
            else
            {
                Debug.LogError("Unable to locate the flight controls script for the aircraft");
                return;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (ControlSurface surface in ControlSurfaces)
        {
            surface.InitialLocalRotation = surface.ControlSurfaceObject.transform.localRotation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Loop through all the control surfaces class items in the list so that each surface can be adjusted for animation purposes.
        foreach (ControlSurface surface in ControlSurfaces)
        {
            float input = surface.ReturnInputValue(surface, FlightControls);

            Vector3 axis = GetAxisVector(surface.RotationAxis);

            if (surface.RotationSettingMode == ControlSurface.RotationSetType.HardSet)
            {
                Quaternion rotationOffset = Quaternion.AngleAxis(input, axis);
                surface.ControlSurfaceObject.transform.localRotation = surface.InitialLocalRotation * rotationOffset;
            }
            else if (surface.RotationSettingMode == ControlSurface.RotationSetType.CumulativeSet)
            {
                Quaternion delta = Quaternion.AngleAxis(input, axis);
                surface.ControlSurfaceObject.transform.localRotation *= delta;
            }
        }
    }

    private Vector3 GetAxisVector(ControlSurface.LocalRotationAxis axis)
    {
        return axis switch
        {
            ControlSurface.LocalRotationAxis.X => Vector3.right,
            ControlSurface.LocalRotationAxis.Y => Vector3.up,
            ControlSurface.LocalRotationAxis.Z => Vector3.forward,
            _ => Vector3.zero
        };
    }
}

/// <summary>
/// Representation of the control surface and how it should be moved to simulate movement of planes surfaces.
/// </summary>
[Serializable]
public class ControlSurface
{
    /// <summary>
    /// The name of the surface for display in the Unity Editor.
    /// </summary>
    [field: SerializeField]
    public string NameOfSurface
    { get; private set; }

    /// <summary>
    /// The gameobject of the control surface.
    /// </summary>
    [field: SerializeField]
    public GameObject ControlSurfaceObject
    { get; set; }

    /// <summary>
    /// The local axis at which the gameobject should be rotated around.
    /// </summary>
    [field: SerializeField]
    public LocalRotationAxis RotationAxis
    { get; private set; }

    /// <summary>
    /// The type of the control surface reflects which part of the plane the rotation should effect.
    /// </summary>
    [field: SerializeField]
    public ControlSurfaceType SurfaceType
    { get; private set; }

    /// <summary>
    /// The rotation should either be reset to a new value, or added over time.
    /// </summary>
    [field: SerializeField]
    public RotationSetType RotationSettingMode
    { get; private set; }

    /// <summary>
    /// The original rotation of the control surface at game start, so it can be referenced too to revert to it's original rotation when being activilely rotated.
    /// </summary>
    public Quaternion InitialLocalRotation
    { get; set; }

    /// <summary>
    /// Returns the Angle of degress that a control surface is rotated to.
    /// </summary>
    /// <param name="surfaceType"></param>
    /// <param name="flightControls"></param>
    /// <returns></returns>
    public float ReturnInputValue(ControlSurface surfaceType, PrimaryFlightControls flightControls)
    {
        float rotationValue = 0;

        if (flightControls == null)
        {
            Debug.LogError("Flight Controls script passed through is null");
            return default;
        }

        switch (surfaceType.SurfaceType)
        {
            case ControlSurfaceType.Elevator:
                rotationValue = -flightControls.ElevatorDegree;
                break;
            case ControlSurfaceType.Aileron_Left:
                rotationValue = -flightControls.AileronDegree_Left;
                break;
            case ControlSurfaceType.Aileron_Right:
                rotationValue = -flightControls.AileronDegree_Right;
                break;
            case ControlSurfaceType.Rudder:
                rotationValue = -flightControls.RudderDegree;
                break;
            case ControlSurfaceType.Throttle:
                rotationValue = flightControls.ThrottleValue;
                break;
            case ControlSurfaceType.Propeller:
                // Convert RPS to degrees per frame
                rotationValue = flightControls.PropellerRotationsPerSecond * 360f * Time.deltaTime; 
                break;
            default:
                Debug.LogWarning("Default used");
                break;
        }

        return rotationValue;
    }

    public enum LocalRotationAxis
    {
        X, Y, Z
    }

    public enum ControlSurfaceType
    {
        Elevator,
        Aileron_Left,
        Aileron_Right,
        Rudder,
        Throttle,
        Propeller
    }

    public enum RotationSetType
    {
        HardSet,
        CumulativeSet
    }
}
