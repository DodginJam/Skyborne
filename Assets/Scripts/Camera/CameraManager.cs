using System;
using System.Collections;
using System.Collections.Generic;
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
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ToggleCameraDataOnInput(InputControls);
    }

    private void LateUpdate()
    {
        UpdateCameraPosition(CurrentCameraData);
        UpdateCameraRotation(AssignedTarget.transform);
    }

    /// <summary>
    /// Keep the cameras position updated relative to the assigned targets position and projected path.
    /// </summary>
    public void UpdateCameraPosition(CameraData currentCameraData)
    {
        // Calculate the desired position relative to the plane's rotation
        Vector3 desiredPosition = AssignedTarget.transform.TransformPoint(currentCameraData.PositionalOffset);

        // Smoothly move the camera toward that position
        CalculatedCameraPosition = Vector3.Lerp(PlayerCamera.transform.position, desiredPosition, CameraLerpSpeed * Time.deltaTime);
        PlayerCamera.transform.position = CalculatedCameraPosition;
    }

    /// <summary>
    /// Update the cameras rotation if it is not equal to the provided coordinates.
    /// </summary>
    public void UpdateCameraRotation(Transform assignedTarget)
    {
        PlayerCamera.transform.rotation = assignedTarget.rotation;
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
        }
    }
}

[Serializable]
public class CameraData
{
    [field: SerializeField]
    public string CameraDataName
    { get; private set; }

    /// <summary>
    /// The camera positional offset from the assigned target - used to maintain camera distance from target.
    /// </summary>
    [field: SerializeField]
    public Vector3 PositionalOffset
    { get; private set; }

    [field: SerializeField]
    public Vector3 RotationalOffset
    { get; private set; }
}
