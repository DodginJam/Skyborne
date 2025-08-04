using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuOptions : MonoBehaviour
{
    [field: SerializeField]
    public UIInput UI_Input
    { private get; set; }

    [field: SerializeField]
    public AircraftInput AircraftPlayerInput
    {  private get; set; }

    [field: SerializeField]
    public Button ResumeButton
    { private get; set; }

    [field: SerializeField]
    public GameObject PauseMenuElements
    { private get; set; }

    void Awake()
    {
        if (UI_Input == null)
        {
            if (transform.root.TryGetComponent<UIInput>(out UIInput ui_input))
            {
                UI_Input = ui_input;
            }
            else
            {
                Debug.LogError("Unable to locate the UIInput system from the root transform component.");
            }
        }

        if (PauseMenuElements == null)
        {
            PauseMenuElements = GetComponentInChildren<GameObject>();

            if (PauseMenuElements == null)
            {
                Debug.LogError("Unable to locate the UIInput system from the root transform component.");
            }
        }

        if (AircraftPlayerInput == null)
        {
            AircraftPlayerInput = GameObject.FindAnyObjectByType<AircraftInput>();

            if (AircraftPlayerInput == null)
            {
                Debug.LogError("Unable to locate the UIInput system from the root transform component.");
            }
        }

        SetUpListeners();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Polling for pause 
        if (UI_Input.GamePauseInputted)
        {
            UI_Input.GamePauseInputted = false;

            if (PauseMenuElements.activeSelf)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        if (Time.timeScale != 1)
        {
            Time.timeScale = 1;
        }
        
        if (!AircraftPlayerInput.isActiveAndEnabled)
        {
            AircraftPlayerInput.enabled = true;
        }

        // Set the mouse state to back to state for flight.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        PauseMenuElements.SetActive(false);
    }

    public void PauseGame()
    {
        if (Time.timeScale != 0)
        {
            Time.timeScale = 0;
        }
        
        if (AircraftPlayerInput.isActiveAndEnabled)
        {
            AircraftPlayerInput.enabled = false;
        }

        // Set the mouse state during pause state.
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        PauseMenuElements.SetActive(true);
    }

    public void SetUpListeners()
    {
        ResumeButton.onClick.AddListener(() => ResumeGame());
    }
}
