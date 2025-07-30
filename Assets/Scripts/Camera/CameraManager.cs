using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraManager : MonoBehaviour
{
    /// <summary>
    /// The camera used by the player to see the scene during gameplay.
    /// </summary>
    [field: SerializeField, Header("Camera and Target")]
    public Camera PlayerCamera
    { get; private set; }

    /// <summary>
    /// The gameobject that is assigned to the camera as the target to follow.
    /// </summary>
    [field: SerializeField]
    public GameObject AssignedTarget
    { get; private set; }

    /// <summary>
    /// The base speed of movement the camera has when moving towards the players direction of movement.
    /// </summary>
    [field: SerializeField, Header("Camera Movement")]
    public float CameraLerpSpeed
    { get; private set; } = 5.0f;

    /// <summary>
    /// The list of available camera that the player can access and their positional and rotational data.
    /// </summary>
    [field: SerializeField, Header("Available Cameras")]
    public List<CameraData> CameraDataList
    { get; private set; }

    /// <summary>
    /// The current data being used to control the camera.
    /// </summary>
    public CameraData CurrentCameraData
    { get; private set; }

    /// <summary>
    /// Used to store the CalculatedCameraPosition and to be assigned to the Camera Transform Position at the end of the update loop.
    /// </summary>
    public Vector3 CalculatedCameraPosition
    { get; private set; }

    /// <summary>
    /// Reference to the component where the player input is received and processed by the New Input System into normalised values for aircraft control.
    /// </summary>
    [field: SerializeField]
    public AircraftInput InputControls
    { get; private set; }

    /// <summary>
    /// The sensitivity multiplyer used by the player camera/
    /// </summary>
    [field: SerializeField, Range(0.1f, 20f)]
    public float CameraSensitivity
    { get; private set; } = 5.0f;

    /// <summary>
    /// The transform component to be rotated for controlling the ThirdPerson / Orbit cameras orbit around the assigned target.
    /// </summary>
    [field: SerializeField]
    public Transform OrbitCameraHolder
    { get; private set; }

    /// <summary>
    /// The list of available UI displays in the game scene - to be managed when visable upon camera change.
    /// </summary>
    public List<AircraftDisplay> AircraftUIDisplays
    { get; private set; } 

    private void Awake()
    {
        // Error checks for the camera exisiting in scene.
        if (PlayerCamera == null)
        {
            PlayerCamera = Camera.main;

            if (PlayerCamera == null)
            {
                Debug.LogError("Unable to locate a player camera in the scene.");
            }
        }

        // Error checks for assigned target / the player controllable object.
        if (AssignedTarget == null)
        {
            // If no target has been assigned, locate the first player character in the scene.
            AssignedTarget = GameObject.FindFirstObjectByType<AircraftController>().gameObject;

            if (AssignedTarget == null)
            {
                Debug.LogError("Unable to locate a gameobject for the camera to follow as no gameobject has the state manager script attached.");
            }
        }

        // Error checks for camera data list.
        if (CameraDataList == null || CameraDataList.Count == 0 || CameraDataList[0] == null)
        {
            Debug.LogError("Error with the camera data list");
        }
        else
        {
            // Assign the first camera data in the list to the current camera data being used.
            CurrentCameraData = CameraDataList[0];
        }

        AircraftUIDisplays = GameObject.FindObjectsByType<AircraftDisplay>(FindObjectsSortMode.None).ToList();

        if (AircraftUIDisplays == null || AircraftUIDisplays.Count() == 0 || AircraftUIDisplays[0] == null)
        {
            Debug.LogWarning("Error with the search for the Aircraft Display UI scripts.");
        }

    }

    void Start()
    {
        // Set the mouse state during flight.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Ensure that the camera is facing the same direction as the plane by setting the camera pitch and yaw angles.
        CurrentCameraData.CameraPitchAngle = AssignedTarget.transform.rotation.x;
        CurrentCameraData.CameraYawAngle = AssignedTarget.transform.rotation.y;

        // Toggle the UI displays to enable the ones intended for the new camera view.
        ToggleUIForCurrentCamera();
    }

    void Update()
    {
        ToggleCameraDataOnInput(InputControls);
    }

    private void LateUpdate()
    {
        UpdateCameraPosition(CurrentCameraData);
        UpdateCameraRotation(AssignedTarget.transform, CurrentCameraData.RotationType);
    }

    /// <summary>
    /// Keep the cameras position updated relative to the assigned targets position and projected path.
    /// </summary>
    public void UpdateCameraPosition(CameraData currentCameraData)
    {
        if (currentCameraData.RotationType == CameraData.RotationMode.POV)
        {
            // Calculate the desired position relative to the plane's rotation
            Vector3 desiredPosition = AssignedTarget.transform.TransformPoint(currentCameraData.PositionalOffset);

            // Smoothly move the camera toward that position
            CalculatedCameraPosition = Vector3.Lerp(PlayerCamera.transform.position, desiredPosition, CameraLerpSpeed * Time.deltaTime);
        }
        else if (currentCameraData.RotationType == CameraData.RotationMode.Orbit)
        {
            // Orbit mode camera uses a seperate 

            currentCameraData.CameraPitchAngle += -(InputControls.CameraInput.y * Time.deltaTime * ProcessSensitivity(InputControls.CurrentInputType));
            currentCameraData.CameraYawAngle += (InputControls.CameraInput.x * Time.deltaTime * ProcessSensitivity(InputControls.CurrentInputType));

            Quaternion baseRotation = AssignedTarget.transform.rotation;

            Quaternion pitchRotation = Quaternion.Euler(-currentCameraData.CameraPitchAngle, 0, 0);

            Quaternion yawRotation = Quaternion.Euler(0, currentCameraData.CameraYawAngle, 0);

            Quaternion finalRotation = yawRotation * pitchRotation;

            OrbitCameraHolder.transform.rotation = finalRotation;

            CalculatedCameraPosition = OrbitCameraHolder.transform.position + (-OrbitCameraHolder.transform.forward * CurrentCameraData.CameraDistanceFromTarget);

            Debug.DrawLine(OrbitCameraHolder.transform.position, CalculatedCameraPosition);
        }
        else
        {
            Debug.Log("Unable to assign position update to camera");
        }

        PlayerCamera.transform.position = CalculatedCameraPosition;
    }

    /// <summary>
    /// Update the cameras rotation by combining the planes rotation with the camera input rotation along the planes local pitch and yaw axis for POV, or...
    /// </summary>
    public void UpdateCameraRotation(Transform assignedTarget, CameraData.RotationMode rotationMode)
    {
        if (rotationMode == CameraData.RotationMode.POV)
        {
            CurrentCameraData.CameraPitchAngle += -(InputControls.CameraInput.y * Time.deltaTime * ProcessSensitivity(InputControls.CurrentInputType));
            CurrentCameraData.CameraYawAngle += (InputControls.CameraInput.x * Time.deltaTime * ProcessSensitivity(InputControls.CurrentInputType));

            Quaternion baseRotation = assignedTarget.rotation;

            Quaternion pitchRotation = Quaternion.AngleAxis(CurrentCameraData.CameraPitchAngle, assignedTarget.right);

            Quaternion yawRotation = Quaternion.AngleAxis(CurrentCameraData.CameraYawAngle, assignedTarget.up);

            Quaternion finalRotation = yawRotation * pitchRotation * baseRotation;

            PlayerCamera.transform.rotation = finalRotation;
        }
        else if (rotationMode == CameraData.RotationMode.Orbit)
        {
            PlayerCamera.transform.LookAt(AssignedTarget.transform.position + CurrentCameraData.PositionalOffset);
        }
    }

    /// <summary>
    /// On the input being received as true for the camera toggle, move through the list and then reset the boolean.
    /// </summary>
    /// <param name="aircraftInput"></param>
    public void ToggleCameraDataOnInput(AircraftInput aircraftInput)
    {
        if (aircraftInput != null && aircraftInput.CameraTogglePressed == true)
        {
            int currentIndex = CameraDataList.IndexOf(CurrentCameraData);
            int newIndex = currentIndex + 1;

            if (newIndex >= CameraDataList.Count)
            {
                newIndex = 0;
            }

            CurrentCameraData = CameraDataList[newIndex];

            aircraftInput.CameraTogglePressed = false;

            // Reset the camera angles to face the direction the aircraft is moving in depending onthe set up of the given rotation mode of the currently used camera data.
            if (CurrentCameraData.RotationType == CameraData.RotationMode.Orbit)
            {
                CurrentCameraData.CameraPitchAngle = AssignedTarget.transform.localEulerAngles.x;
                CurrentCameraData.CameraYawAngle = AssignedTarget.transform.eulerAngles.y - 180;
            }
            else if (CurrentCameraData.RotationType == CameraData.RotationMode.POV)
            {
                CurrentCameraData.CameraPitchAngle = 0;
                CurrentCameraData.CameraYawAngle = 0;
            }

            // Toggle the UI displays to enable the ones intended for the new camera view.
            ToggleUIForCurrentCamera();
        }
    }

    /// <summary>
    /// Toggle the UI displays to enable the ones intended for the new camera view.
    /// </summary>
    public void ToggleUIForCurrentCamera()
    {
        // Toggle the UI displays to enable the ones intended for the new camera view.
        if (AircraftUIDisplays != null && AircraftUIDisplays.Count() > 0)
        {
            foreach (AircraftDisplay display in AircraftUIDisplays)
            {
                if (display.CameraTypeDisplay == CurrentCameraData.RotationType)
                {
                    display.gameObject.SetActive(true);
                }
                else
                {
                    display.gameObject.SetActive(false);
                }
            }
        }
    }

    /// <summary>
    /// Controls the current applied sensitvity to the current control type - makes up for difference in using M&K, GamePad and joysticks inherient sensitivity differences.
    /// </summary>
    /// <param name="controlType"></param>
    /// <returns></returns>
    float ProcessSensitivity(AircraftInput.ControlInputType controlType)
    {
        float sensitivityLevel = 0;

        switch (controlType)
        {
            case AircraftInput.ControlInputType.MouseKeyboard:
                sensitivityLevel = CameraSensitivity;
                break;
            case AircraftInput.ControlInputType.Joystick:
                sensitivityLevel = Mathf.Pow(CameraSensitivity, 2);
                break;
            case AircraftInput.ControlInputType.Gamepad:
                sensitivityLevel = Mathf.Pow(CameraSensitivity, 2);
                break;
            default:
                sensitivityLevel = CameraSensitivity;
                break;
        }

        return sensitivityLevel;
    }
}


/// <summary>
/// Data type for the current usage of the player camera.
/// </summary>
[Serializable]
public class CameraData
{
    /// <summary>
    /// The name of the camera - for Unity Editor.
    /// </summary>
    [field: SerializeField]
    public string CameraDataName
    { get; private set; }

    /// <summary>
    /// The camera positional offset from the assigned target - used to maintain camera distance from target.
    /// </summary>
    [field: SerializeField]
    public Vector3 PositionalOffset
    { get; private set; }

    /// <summary>
    /// The distance that the camera should be spaced from the current assigned target.
    /// </summary>
    [field: SerializeField]
    public float CameraDistanceFromTarget
    { get; private set; }

    /// <summary>
    /// Controls the type of rotation applied to the camera controls.
    /// </summary>
    [field: SerializeField]
    public RotationMode RotationType
    { get; private set; }

    /// <summary>
    /// The currently tracked pitch Angle that the current camera should be set to - provided through calculation.
    /// </summary>
    public float CameraPitchAngle
    { get; set; }

    /// <summary>
    /// The currently tracked yaw Angle that the current camera should be set to - provided through calculation.
    /// </summary>
    public float CameraYawAngle
    { get; set; }

    /// <summary>
    /// Rotation mode options.
    /// </summary>
    public enum RotationMode
    {
        POV,
        Orbit
    }
}
