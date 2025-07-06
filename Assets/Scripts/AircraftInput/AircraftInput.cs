using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class AircraftInput : MonoBehaviour
{
    public InputActions_Skyborne InputActions
    { get; private set; }

    public InputActions_Skyborne.AircraftActions AircraftActionMap
    { get; private set; }

    public float ThrottleInput
    { get; private set; }

    public float ElevatorInput
    { get; private set; }

    public float AileronInput
    { get; private set; }

    public float RudderInput
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
        ThrottleInput = AircraftActionMap.Throttle.ReadValue<float>();
        ElevatorInput = AircraftActionMap.PitchAndRoll.ReadValue<Vector2>().y;
        AileronInput = -AircraftActionMap.PitchAndRoll.ReadValue<Vector2>().x;
        RudderInput = AircraftActionMap.Yaw.ReadValue<float>();

        // Debug.Log($"Throttle: {ThrottleInput}\tElevator: {ElevatorInput}\tAileron:{AileronInput}\tRudder: {RudderInput}");
    }
}
