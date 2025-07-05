using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    /// The position of the target from the last frame.
    /// </summary>
    public Vector3 LastFrameTargetPosition
    { get; private set; }

    /// <summary>
    /// The camera positional offset from the assigned target - used to maintain camera distance from target.
    /// </summary>
    [field: SerializeField]
    public Vector3 CameraPositionalOffset
    { get; private set; }

    /// <summary>
    /// The base speed of movement the camera has when moving towards the players direction of movement.
    /// </summary>
    [field: SerializeField, Header("Camera Movement")]
    public float CameraLerpSpeed
    { get; private set; } = 5.0f;

    /// <summary>
    /// Used to store the CalculatedCameraPosition and to be assigned to the Camera Transform Position at the end of the update loop.
    /// </summary>
    public Vector3 CalculatedCameraPosition
    { get; private set; }

    private void Awake()
    {
        if (PlayerCamera == null)
        {
            PlayerCamera = Camera.main;

            if (PlayerCamera == null)
            {
                Debug.LogError("Unable to locate a player camera in the scene.");
            }
        }

        if (AssignedTarget == null)
        {
            // If no target has been assigned, locate the first player character in the scene.
            AssignedTarget = GameObject.FindFirstObjectByType<AircraftController>().gameObject;

            if (AssignedTarget == null)
            {
                Debug.LogError("Unable to locate a gameobject for the camera to follow as no gameobject has the state manager script attached.");
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CalculatedCameraPosition = AssignedTarget.transform.position + CameraPositionalOffset;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        UpdateCameraPosition();
        UpdateCameraRotation();
    }

    /// <summary>
    /// Keep the cameras position updated relative to the assigned targets position and projected path.
    /// </summary>
    public void UpdateCameraPosition()
    {
        // Calculate the desired position relative to the plane's rotation
        Vector3 desiredPosition = AssignedTarget.transform.TransformPoint(CameraPositionalOffset);

        // Smoothly move the camera toward that position
        CalculatedCameraPosition = Vector3.Lerp(PlayerCamera.transform.position, desiredPosition, CameraLerpSpeed * Time.deltaTime);
        PlayerCamera.transform.position = CalculatedCameraPosition;
    }

    /// <summary>
    /// Update the cameras rotation if it is not equal to the provided coordinates.
    /// </summary>
    public void UpdateCameraRotation()
    {
        PlayerCamera.transform.rotation = AssignedTarget.transform.rotation;
    }
}
