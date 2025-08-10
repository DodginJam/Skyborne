using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// The input system for the UI.
    /// </summary>
    [field: SerializeField]
    public UIInput UI_Input
    { private get; set; }

    [field: SerializeField]
    public Button PlayGameButton
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
    public Button QuitButton
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

        SetUpListeners();
    }

    private void OnEnable()
    {
        if (UI_Input != null)
        {
            UI_Input.GamePause += OnBackInput;
        }
    }

    private void OnDisable()
    {
        if (UI_Input != null)
        {
            UI_Input.GamePause -= OnBackInput;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// To be called when the pause input is pressed - it either pauses the game, or it removes the open list of UI elements in order they were opened until the all are gone and game unpauses.
    /// </summary>
    void OnBackInput()
    {
        // If the only pause menu open is the actually base pause menu, close it and resume the game.
        if (OpenUIELements.Count > 0)
        {
            // Look at the list of open UI elements of the pause menu and remove the latest opened gameobject and close it.
            OpenUIELements[OpenUIELements.Count - 1].SetActive(false);
            OpenUIELements.RemoveAt(OpenUIELements.Count - 1);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayGame()
    {
        SceneManager.LoadScene("MainScene");
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
        PlayGameButton.onClick.AddListener(() => PlayGame());

        OptionsButton.onClick.AddListener(() => OptionsPress());

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
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
