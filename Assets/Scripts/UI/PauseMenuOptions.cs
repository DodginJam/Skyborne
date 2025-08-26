using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuOptions : MonoBehaviour
{
    /// <summary>
    /// The input system for the UI.
    /// </summary>
    [field: SerializeField]
    public UIInput UI_Input
    { private get; set; }

    /// <summary>
    /// The input system for the player aircraft controller.
    /// </summary>
    [field: SerializeField]
    public AircraftInput AircraftPlayerInput
    {  private get; set; }

    [field: SerializeField]
    public Button ResumeButton
    { private get; set; }

    [field: SerializeField]
    public Button RestartButton
    { private get; set; }

    [field: SerializeField]
    public Button OptionsButton
    { private get; set; }

    /// <summary>
    /// The gameobject holding the options menu elements.
    /// </summary>
    [field: SerializeField]
    public GameObject OptionsMenu
    { private get; set; }

    [field: SerializeField]
    public Button MenuButton
    { private get; set; }

    [field: SerializeField]
    public Button QuitButton
    { private get; set; }
    
    /// <summary>
    /// The gameobject holding the pause menu elements.
    /// </summary>
    [field: SerializeField]
    public GameObject PauseMenuElements
    { private get; set; }

    /// <summary>
    /// The list of open UI elements to be tracked for menu control.
    /// </summary>
    public List<GameObject> OpenUIELements
    { private get; set; } = new List<GameObject>();

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
            Debug.LogError("Unable to locate the PauseMenuElements game object from the root transform component.");
        }

        if (AircraftPlayerInput == null)
        {
            AircraftPlayerInput = GameObject.FindAnyObjectByType<AircraftInput>();

            if (AircraftPlayerInput == null)
            {
                Debug.LogError("Unable to locate the AircraftPlayerInput system from the root transform component.");
            }
        }

        SetUpListeners();
    }

    private void OnEnable()
    {
        if (UI_Input != null)
        {
            UI_Input.GamePause += OnPauseInput;
        }
    }

    private void OnDisable()
    {
        if (UI_Input != null)
        {
            UI_Input.GamePause -= OnPauseInput;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// To be called when the pause input is pressed - it either pauses the game, or it removes the open list of UI elements in order they were opened until the all are gone and game unpauses.
    /// </summary>
    void OnPauseInput()
    {
        if (PauseMenuElements.activeSelf)
        {
            // If the only pause menu open is the actually base pause menu, close it and resume the game.
            if (OpenUIELements.Count == 1 && OpenUIELements[0] == PauseMenuElements)
            {
                ResumeGame();
            }
            else
            {
                // Look at the list of open UI elements of the pause menu and remove the latest opened gameobject and close it.
                OpenUIELements[OpenUIELements.Count - 1].SetActive(false);
                OpenUIELements.RemoveAt(OpenUIELements.Count - 1);
            }
        }
        else
        {
            PauseGame();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ResumeGame()
    {
        SceneHandler.Instance.ResumeScene();
        
        if (!AircraftPlayerInput.isActiveAndEnabled)
        {
            AircraftPlayerInput.enabled = true;
        }

        // Set the mouse state to back to state for flight.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Remove the pause menu elements from the active list of UI elements and disable it.
        ModifyOpenUIElements(false, PauseMenuElements);
    }

    public void PauseGame()
    {
        SceneHandler.Instance.PauseScene();

        if (AircraftPlayerInput.isActiveAndEnabled)
        {
            AircraftPlayerInput.enabled = false;
        }

        // Set the mouse state during pause state.
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        // Add the pause menu to the list of open UI elements.
        ModifyOpenUIElements(true, PauseMenuElements);
    }

    public void OptionsPress()
    {
        if (OptionsMenu != null)
        {
            // Add the options menu to the list of open UI elements.
            ModifyOpenUIElements(true, OptionsMenu);
        }
        else
        {
            Debug.LogError("OptionsPress Menu is not assigned.");
        }
    }

    /// <summary>
    /// Initialisation check for button events.
    /// </summary>
    public void SetUpListeners()
    {
        ResumeButton.onClick.AddListener(() => ResumeGame());

        RestartButton.onClick.AddListener(() => RestartGame());

        OptionsButton.onClick.AddListener(() => OptionsPress());

        MenuButton.onClick.AddListener(() => ChangeScene(0));

        QuitButton.onClick.AddListener(() => QuitGame());
    }

    /// <summary>
    /// Set a supplied reference to a gameobject as being either active or deactive and then add or remove it from the list of the UI elements. dee
    /// </summary>
    /// <param name="add"></param>
    /// <param name="UIelement"></param>
    public void ModifyOpenUIElements(bool add, GameObject UIelement)
    {
        if (add)
        {
            OpenUIELements.Add(UIelement);
            UIelement.SetActive(true);
        }
        else
        {
            OpenUIELements.Remove(UIelement);
            UIelement.SetActive(false);
        }
    }

    /// <summary>
    /// Quit the game depending on the game running envionment.
    /// </summary>
    void QuitGame()
    {
        SceneHandler.Instance.QuitGame();
    }

    void ChangeScene(int newSceneIndex)
    {
        SceneHandler.Instance.LoadScene(newSceneIndex);
    }

    /// <summary>
    /// Reload the exisiting scene after resetting the timescale.
    /// </summary>
    void RestartGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneHandler.Instance.RestartScene();
    }
}
