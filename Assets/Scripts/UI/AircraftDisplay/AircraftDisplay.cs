using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static AircraftDisplay;

public class AircraftDisplay : MonoBehaviour
{
    [field: SerializeField, Header("Script References")]
    public AircraftController PlayerAircraftScript
    {  get; private set; }

    [field: SerializeField]
    public GameManager GameManagerScript
    { get; private set; }

    /// <summary>
    /// Based on the prefence displayed below, this controls whether this aircraft display is displayed to the game world.
    /// </summary>
    [field: SerializeField, Header("Display To Camera Mode")]
    public CameraData.RotationMode CameraTypeDisplay
    { get; private set; }

    [field: SerializeField, Header("Display Elements")]
    public UIAndCachedDisplay<TextMeshProUGUI, int> ScoreDisplay
    { get; private set; }

    [field: SerializeField]
    public UIAndCachedDisplay<TextMeshProUGUI, string> PenaltyDisplay
    { get; private set; }

    [field: SerializeField]
    public UIAndCachedDisplay<TextMeshProUGUI, int> SpeedDisplay
    { get; private set; }

    [field: SerializeField]
    public UIAndCachedDisplay<TextMeshProUGUI, int> AltitudeDisplay
    { get; private set; }

    [field: SerializeField] 
    public UIAndCachedDisplay<Slider, float> ThrottleDisplay
    { get; private set; }

    private void Awake()
    {

        // Update the UI display to get reference to the player camera if it has not been assigned as the UI render mode is in world space.
        Canvas uiCanvas = transform.root.GetComponentInChildren<Canvas>();

        if (uiCanvas != null && uiCanvas.renderMode == RenderMode.WorldSpace)
        {
            CameraManager cameraManager = GameObject.FindAnyObjectByType<CameraManager>();

            Camera playerCam = cameraManager.PlayerCamera;

            if (playerCam != null)
            {
                uiCanvas.worldCamera = playerCam;
            }
        }

        if (PlayerAircraftScript == null)
        {
            PlayerAircraftScript = GameObject.FindAnyObjectByType<AircraftController>();

            if (PlayerAircraftScript == null)
            {
                Debug.LogError("Unable to locate a playable character in the scene.");
            }
        }

        if (GameManagerScript == null)
        {
            GameManagerScript = GameObject.FindAnyObjectByType<GameManager>();

            if (GameManagerScript == null)
            {
                Debug.LogError("Unable to locate a playable character in the scene.");
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Update the player display values from the value holder.
        if (PlayerAircraftScript != null)
        {
            if (PlayerAircraftScript.CurrentValues != null && PlayerAircraftScript.CurrentValues.ValuesHolder != null)
            {
                // Speed Display.
                if (SpeedDisplay != null)
                {
                    if (SpeedDisplay.CompareDataToDisplayCache(Mathf.RoundToInt(PlayerAircraftScript.CurrentValues.ValuesHolder.AirSpeed), out int data))
                    {
                        SpeedDisplay.UpdateCachedData(data);
                        SpeedDisplay.UpdateDisplayElement();
                    }
                }
                else
                {
                    Debug.LogError("SpeedDisplay not assigned.");
                }

                // Altitude Display
                if (AltitudeDisplay != null)
                {
                    if (AltitudeDisplay.CompareDataToDisplayCache(Mathf.RoundToInt(PlayerAircraftScript.CurrentValues.ValuesHolder.HeightAboveSeaLevel), out int data))
                    {
                        AltitudeDisplay.UpdateCachedData(data);
                        AltitudeDisplay.UpdateDisplayElement();
                    }
                }
                else
                {
                    Debug.LogError("AltitudeDisplay not assigned.");
                }

                if (ThrottleDisplay != null)
                {
                    if (ThrottleDisplay.CompareDataToDisplayCache(PlayerAircraftScript.CurrentValues.FlightControls.ThrottleValue, out float data))
                    {
                        ThrottleDisplay.UpdateCachedData(data);
                        ThrottleDisplay.UpdateDisplayElement();
                    }
                }
            }
            else
            {
                Debug.LogError("PlayerAircraftScript.CurrentValues or PlayerAircraftScript.CurrentValues.ValuesHolder not assigned.");
            }
        }
        else
        {
            Debug.LogError("PlayerAircraftScript not assigned.");
        }
    }

    /// <summary>
    /// Method to be called to update the ScoreCount onto the Airacrft UI display.
    /// </summary>
    /// <param name="newValueToDisplay"></param>
    public void UpdateScoreDisplay(int newValueToDisplay)
    {
        ScoreDisplay.UpdateCachedData(Mathf.RoundToInt(newValueToDisplay));
        ScoreDisplay.UpdateDisplayElement();
    }

    /// <summary>
    /// Called when the display for the penalty needs to be updated.
    /// </summary>
    /// <param name="newValueToDisplay"></param>
    public void UpdatePenaltyDisplay(int newValueToDisplay)
    {
        PenaltyDisplay.UpdateCachedData($"{newValueToDisplay} / {GameManagerScript.PenaltyLimit}");
        PenaltyDisplay.UpdateDisplayElement();
    }

    /// <summary>
    /// Class for containing generic data types representing the UI component being used to display a given element of information, and the data that is to be cached for display purposes.
    /// </summary>
    /// <typeparam name="UiType"></typeparam>
    /// <typeparam name="CachedDataType"></typeparam>
    [Serializable]
    public class UIAndCachedDisplay<UiType, CachedDataType>
    {
        /// <summary>
        /// The generic type representing the UI elements being displayed via Unity UI canvas.
        /// </summary>
        [field: SerializeField]
        public UiType UIElement
        { get; private set; }

        /// <summary>
        /// The data type used to cache the display data, to avoid updates every frame.
        /// </summary>
        public CachedDataType CachedDisplayElement
        { get; private set; }

        /// <summary>
        /// Ensures that the data being passed equals the data type of the UIAndCacheDisplay CacheDataType.
        /// </summary>
        /// <param name="newData"></param>
        /// <param name="newDataReturn"></param>
        /// <returns></returns>
        public bool CompareDataToDisplayCache(CachedDataType newData, out CachedDataType newDataReturn)
        {
            newDataReturn = newData;

            return !EqualityComparer<CachedDataType>.Default.Equals(newData, CachedDisplayElement);
        }

        /// <summary>
        /// Updates the cached display data to the new passed data.
        /// </summary>
        /// <param name="newData"></param>
        public void UpdateCachedData(CachedDataType newData)
        {
            CachedDisplayElement = newData;
        }

        /// <summary>
        /// Update the display UI element throgh checking the data of the UI element.
        /// </summary>
        public void UpdateDisplayElement()
        {
            if (UIElement is TextMeshProUGUI textMeshProDisplay)
            {
                textMeshProDisplay.text = CachedDisplayElement.ToString();
            }
            else if (UIElement is Slider sliderDisplay)
            {
                if (CachedDisplayElement is int newDataInt)
                {
                    sliderDisplay.value = newDataInt;
                }
                else if (CachedDisplayElement is float newDataFloat)
                {
                    sliderDisplay.value = newDataFloat;
                }
                else
                {
                    Debug.LogWarning("Unable to pass data type to the slider value.");
                }
            }
        }
    }
}
