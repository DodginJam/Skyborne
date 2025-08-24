using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem;

public class ReApplyActionAsset : MonoBehaviour
{
    public InputSystemUIInputModule InputSystemUIInputModule
    {  get; private set; }

    private void Awake()
    {
        InputSystemUIInputModule = GetComponent<InputSystemUIInputModule>();
    }

    // Start is called before the first frame update
    void Start()
    {
        InputSystemUIInputModule.actionsAsset = InputManager.Instance.InputActions.asset;
        InputSystemUIInputModule.enabled = false;
        InputSystemUIInputModule.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
