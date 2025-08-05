using System.Collections;
using System.Collections.Generic;
using UnityEditor.Recorder.Input;
using UnityEngine;
using UnityEngine.InputSystem;

public class AircraftInput : MonoBehaviour
{
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



    private static int JoystickID
    { get; set; }

    private static int GamepadID
    { get; set; }

    private static int MouseKeyboardID
    { get; set; }



    public bool IsJoytickControl
    { get; private set; } = false;

    public ControlInputType CurrentInputType
    { get; private set; }

    public bool CameraTogglePressed
    { get; set; }

    public bool CameraFreeLookTogglePressed
    { get; set; }

    public Vector2 CameraInput
    { get; private set; }

    public bool IsFiring
    { get; private set; }

    public bool FireSafetyDisabled
    { get; private set; }

    private void Awake()
    {
        if (InputManager.Instance.InputActions != null)
        {
            AircraftActionMap = InputManager.Instance.InputActions.Aircraft;
        }
        else
        {
            Debug.LogError("Unable to assign class instance to InputActions");
        }

        SetUpInputListeners(AircraftActionMap);
    }

    // Start is called before the first frame update
    void Start()
    {
        JoystickID = Animator.StringToHash("Joystick");
        GamepadID = Animator.StringToHash("Gamepad");
        MouseKeyboardID = Animator.StringToHash("Keyboard&Mouse");

        Debug.Log($"JoystickID: {JoystickID}");
        Debug.Log($"GamepadID: {GamepadID}");
        Debug.Log($"MouseKeyboardID: {MouseKeyboardID}");

    }

    public void OnEnable()
    {
        ResetInputs();
        AircraftActionMap.Enable();
    }

    public void OnDisable()
    {
        ResetInputs();
        AircraftActionMap.Disable();
    }

    public void ResetInputs()
    {
        ThrottleInput = 0;
        ElevatorInput = 0;
        AileronInput = 0;
        RudderInput = 0;
        CameraInput = Vector2.zero;
        IsFiring = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Check to see if the input scheme has been changed and capture the string name of the new input type.
        if (CurrentControlSchemeID != Animator.StringToHash(InputManager.Instance.PlayerInputComponent.currentControlScheme))
        {
            CurrentControlSchemeID = Animator.StringToHash(InputManager.Instance.PlayerInputComponent.currentControlScheme);

            // Update the control type enum to reflect the current input when it has switched.
            switch (CurrentControlSchemeID)
            {
                case var value when value == MouseKeyboardID:
                    CurrentInputType = ControlInputType.MouseKeyboard;
                    break;
                case var value when value == JoystickID:
                    CurrentInputType = ControlInputType.Joystick;
                    break;
                case var value when value == GamepadID:
                    CurrentInputType = ControlInputType.Gamepad;
                    break;
                default:
                    CurrentInputType = ControlInputType.None;
                    break;
            }

            Debug.Log($"Input has switched to: {CurrentInputType.ToString()}");
        }

        Debug.Log($"CurrentInputType: {InputManager.Instance.PlayerInputComponent.currentControlScheme}");
        Debug.Log($"CurrentInputType marked: {CurrentInputType.ToString()}");

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

        // Process the input for the camera controls.
        CameraInput = AircraftActionMap.Look.ReadValue<Vector2>();
    }

    void SetUpInputListeners(InputActions_Skyborne.AircraftActions aircraftActions)
    {
        aircraftActions.CameraToggle.started += context =>
        {
            OnCameraToggle(context);
        };

        aircraftActions.Fire.started += context =>
        {
            OnFire(context);
        };

        aircraftActions.Fire.canceled += context =>
        {
            OnFire(context);
        };

        aircraftActions.FireSafety.started += context =>
        {
            OnFireSafety(context);
        };

        aircraftActions.CameraFreeLookToggle.started += context =>
        {
            OnCameraFreeLookToggle(context);
        };
    }

    public void OnCameraToggle(InputAction.CallbackContext context)
    {
        CameraTogglePressed = context.ReadValueAsButton();
    }

    public void OnCameraFreeLookToggle(InputAction.CallbackContext context)
    {
        CameraFreeLookTogglePressed = context.ReadValueAsButton();
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsFiring = true;
        }
        else if (context.canceled)
        {
            IsFiring = false;
        }
    }

    public void OnFireSafety(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            FireSafetyDisabled = !FireSafetyDisabled;
        }
    }

    public enum ControlInputType
    {
        None,
        Gamepad,
        Joystick,
        MouseKeyboard
    }
}