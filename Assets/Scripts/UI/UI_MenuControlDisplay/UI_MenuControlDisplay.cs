using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_MenuControlDisplay : MonoBehaviour
{
    [field: SerializeField]
    public string ControlDescription
    {  get; private set; }

    [field: SerializeField]
    public TextMeshProUGUI ControlDescription_Text
    { get; private set; }

    [field: SerializeField, Header("Input Text")]
    public string PC_Key
    { get; private set; }

    [field: SerializeField]
    public string ControllerKey
    { get; private set; }

    [field: SerializeField]
    public string JoystickKey
    { get; private set; }
    
    [field: SerializeField, Header("Text Elements UI")]
    public TextMeshProUGUI PC_InputTextDisplay
    { get; private set; }

    [field: SerializeField]
    public TextMeshProUGUI Controller_InputTextDisplay
    { get; private set; }

    [field: SerializeField]
    public TextMeshProUGUI Joystick_InputTextDisplay
    { get; private set; }

    public void Awake()
    {
        if (ControlDescription_Text != null)
        {
            ControlDescription_Text.text = ControlDescription;
        }

        if (PC_InputTextDisplay != null)
        {
            PC_InputTextDisplay.text = PC_Key;
        }

        if (Controller_InputTextDisplay != null)
        {
            Controller_InputTextDisplay.text = ControllerKey;
        }

        if (Joystick_InputTextDisplay != null)
        {
            Joystick_InputTextDisplay.text = JoystickKey;
        }
    }
}
