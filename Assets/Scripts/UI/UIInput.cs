using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIInput : MonoBehaviour
{
    public InputActions_Skyborne.UIActions UIActionMap
    { get; private set; }

    public event Action GamePause;

    private void Awake()
    {
        if (InputManager.Instance.InputActions != null)
        {
            UIActionMap = InputManager.Instance.InputActions.UI;
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
        UIActionMap.Enable();
    }

    public void OnDisable()
    {
        UIActionMap.Disable();
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
        GamePause?.Invoke();
    }
}
