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

    }

    // Update is called once per frame
    void Update()
    {
        foreach (ControlSurface surface in ControlSurfaces)
        {
            Vector3 euler = surface.ControlSurfaceObject.transform.localEulerAngles;

            switch (surface.RotationAxis)
            {
                case ControlSurface.LocalRotationAxis.X:
                    euler.x = surface.ReturnSurfaceInputValue(surface, FlightControls);
                    break;
                case ControlSurface.LocalRotationAxis.Y:
                    euler.y = surface.ReturnSurfaceInputValue(surface, FlightControls);
                    break;
                case ControlSurface.LocalRotationAxis.Z:
                    euler.z = surface.ReturnSurfaceInputValue(surface, FlightControls);
                    break;
                default:
                    Debug.LogWarning("Default used");
                    break;
            }

            surface.ControlSurfaceObject.transform.localRotation = Quaternion.Euler(euler);
        }
    }
}

[Serializable]
public class ControlSurface
{
    [field: SerializeField]
    public string NameOfSurface
    { get; private set; }

    [field: SerializeField]
    public GameObject ControlSurfaceObject
    { get; set; }

    [field: SerializeField]
    public LocalRotationAxis RotationAxis
    { get; private set; }

    [field: SerializeField]
    public ControlSurfaceType SurfaceType
    { get; private set; }

    public float ReturnSurfaceInputValue(ControlSurface surfaceType, PrimaryFlightControls flightControls)
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
        Throttle
    }
}
