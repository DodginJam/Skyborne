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

    public int CurrentControlSchemeID
    { get; private set; }


    public int JoystickID
    { get; private set; }


    public bool IsJoytickControl
    { get; private set; } = false;

    public PlayerInput PlayerInputComponent
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

        if (TryGetComponent<PlayerInput>(out PlayerInput playerInputComponent))
        {
            PlayerInputComponent = playerInputComponent;
        }
        else
        {
            Debug.LogError("Unable to locate a player input component");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        JoystickID = Animator.StringToHash("Joystick");
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
        // Check to see if the input scheme has been changed and capture the string name of the new input type.
        if (CurrentControlSchemeID != Animator.StringToHash(PlayerInputComponent.currentControlScheme))
        {
            CurrentControlSchemeID = Animator.StringToHash(PlayerInputComponent.currentControlScheme);
        }

        // Check for joystick being used as control so that throttle input can be swapped to a different binding setup.
        if (CurrentControlSchemeID != JoystickID)
        {
            if (IsJoytickControl == true)
            {
                IsJoytickControl = false;
            }

            ThrottleInput = AircraftActionMap.ThrottleComposite.ReadValue<float>();
        }
        else
        {
            if (IsJoytickControl == false)
            {
                IsJoytickControl = true;
            }

            ThrottleInput = AircraftActionMap.ThrottleSlider.ReadValue<float>();
        }

        // Process the input of the aircraft controls here through update polling.
        ElevatorInput = AircraftActionMap.PitchAndRoll.ReadValue<Vector2>().y;
        AileronInput = -AircraftActionMap.PitchAndRoll.ReadValue<Vector2>().x;
        RudderInput = AircraftActionMap.Yaw.ReadValue<float>();

        // Debug.Log($"Throttle: {ThrottleInput}\tElevator: {ElevatorInput}\tAileron:{AileronInput}\tRudder: {RudderInput}");
    }
}
