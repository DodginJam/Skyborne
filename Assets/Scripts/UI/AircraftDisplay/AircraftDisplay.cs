using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AircraftDisplay : MonoBehaviour
{
    [field: SerializeField, Header("Script References")]
    public AircraftController PlayerAircraftScript
    {  get; private set; }

    [field: SerializeField]
    public Score ScoreScript
    { get; private set; }

    [field: SerializeField, Header("Display Elements")]
    public TextMeshProUGUI ScoreDisplay
    { get; private set; }

    [field: SerializeField]
    public TextMeshProUGUI SpeedDisplay
    { get; private set; }

    [field: SerializeField]
    public TextMeshProUGUI AltitudeDisplay
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
                    SpeedDisplay.text = PlayerAircraftScript.CurrentValues.ValuesHolder.AirSpeed.ToString();
                }
                else
                {
                    Debug.LogError("SpeedDisplay not assigned.");
                }

                // Altitude Display
                if (AltitudeDisplay != null)
                {
                    AltitudeDisplay.text = PlayerAircraftScript.CurrentValues.ValuesHolder.HeightAboveSeaLevel.ToString();
                }
                else
                {
                    Debug.LogError("AltitudeDisplay not assigned.");
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

    public void UpdateScoreDisplay(int newValueToDisplay)
    {
        ScoreDisplay.text = newValueToDisplay.ToString();
    }
}
