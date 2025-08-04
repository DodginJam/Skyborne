using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class UIInput : MonoBehaviour
{
    public InputActions_Skyborne InputActions
    { get; private set; }

    public InputActions_Skyborne.UIActions UIActionMap
    { get; private set; }

    public bool GamePauseInputted
    { get; set; }

    private void Awake()
    {
        InputActions = new InputActions_Skyborne();

        if (InputActions != null)
        {
            UIActionMap = InputActions.UI;
        }
        else
        {
            Debug.LogError("Unable to assign class instance to InputActions");
        }

        SetUpInputListeners(UIActionMap);
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

    }

    void SetUpInputListeners(InputActions_Skyborne.UIActions uiActions)
    {
        uiActions.Pause.started += context =>
        {
            OnPause(context);
        };
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        GamePauseInputted = context.ReadValueAsButton();
    }
}
