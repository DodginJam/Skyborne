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
    public Score ScoreScript
    { get; private set; }

    [field: SerializeField, Header("Display Elements")]
    public UIAndCachedDisplay<TextMeshProUGUI, int> ScoreDisplay
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
        if (PlayerAircraftScript == null)
        {
            PlayerAircraftScript = GameObject.FindAnyObjectByType<AircraftController>();

            if (PlayerAircraftScript == null)
            {
                Debug.LogError("Unable to locate a playable character in the scene.");
            }
        }

        if (ScoreScript == null)
        {
            ScoreScript = GameObject.FindAnyObjectByType<Score>();

            if (ScoreScript == null)
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
    /// Method to be called to update the score onto the Airacrft UI display.
    /// </summary>
    /// <param name="newValueToDisplay"></param>
    public void UpdateScoreDisplay(int newValueToDisplay)
    {
        ScoreDisplay.UpdateCachedData(Mathf.RoundToInt(newValueToDisplay));
        ScoreDisplay.UpdateDisplayElement();
    }

    [Serializable]
    public class UIAndCachedDisplay<UiType, CachedDataType>
    {
        [field: SerializeField]
        public UiType UIElement
        { get; private set; }

        public CachedDataType CachedDisplayElement
        { get; private set; }

        public bool CompareDataToDisplayCache(CachedDataType newData, out CachedDataType newDataReturn)
        {
            newDataReturn = newData;

            return !EqualityComparer<CachedDataType>.Default.Equals(newData, CachedDisplayElement);
        }

        public void UpdateCachedData(CachedDataType newData)
        {
            CachedDisplayElement = newData;
        }

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
