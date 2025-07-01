using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class AircraftInput : MonoBehaviour
{
    public InputActions_Skyborne InputActions
    { get; private set; }

    public InputActions_Skyborne.AircraftActions AircraftActionMap
    { get; private set; }

    public float ThrustInput
    { get; private set; }

    public float PitchInput
    { get; private set; }

    public float RollInput
    { get; private set; }

    public float YawInput
    { get; private set; }

    private void Awake()
    {
        InputActions = new InputActions_Skyborne();

        if (InputActions != null)
        {
            AircraftActionMap = InputActions.Aircraft;
        }
        else
        {
            Debug.LogError("Unable to assign class instance to InputActions");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnEnable()
    {
        InputActions.Enable();
    }

    public void OnDisable()
    {
        InputActions.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        // Process the input of the aircraft controls here through update polling.
        ThrustInput = AircraftActionMap.Throttle.ReadValue<float>();
        PitchInput = AircraftActionMap.PitchAndRoll.ReadValue<Vector2>().y;
        RollInput = -AircraftActionMap.PitchAndRoll.ReadValue<Vector2>().x;
        YawInput = AircraftActionMap.Yaw.ReadValue<float>();

        Debug.Log($"Thrust: {ThrustInput}, Pitch: {PitchInput}, Roll: {RollInput}, Yaw: {YawInput}");
    }
}
