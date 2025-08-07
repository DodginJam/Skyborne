using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFlightTracker : MonoBehaviour
{
    [field: SerializeField]
    public GameManager GameManagerScript
    {  get; private set; }

    /// <summary>
    /// How often the score counter for the level flight should be tracked and updated to reflect the players position and rotation.
    /// </summary>
    [field: SerializeField, Min(0.1f)]
    public float ScoreTickDuration
    { get; private set; } = 1f;

    /// <summary>
    /// The current ticker that track the time passed since the last score update.
    /// </summary>
    public float ScoreTicker
    { get; private set; } = 0;

    [field: SerializeField, Min(1)]
    public float MaxScoreAllowedPerTick
    { get; private set; } = 10f;

    /// <summary>
    /// Modifies the score awarded to player based on the alltitude the player is flying at.
    /// </summary>
    [field: SerializeField, Header("Altitude Score Modify")]
    public AnimationCurve AltitudeScoreMultiplier
    { get; private set; }

    [field: SerializeField]
    public float MinAltitudeValue
    { get; private set; } = 243.84f;

    [field: SerializeField]
    public float MaxAltitudeValue
    { get; private set; } = 365.76f;

    /// <summary>
    /// Modifies the score awarded to player based on how parallel the player is flying at respective to the ground plane.
    /// </summary>
    [field: SerializeField, Header("Level Score Modify")]
    public AnimationCurve LevelRotationMultiplier
    { get; private set; }

    [field: SerializeField]
    public float MinRotationValue
    { get; private set; } = 0;

    [field: SerializeField]
    public float MaxRotationValue
    { get; private set; } = 0;

    private void Awake()
    {
        if (TryGetComponent<GameManager>(out GameManager gameManagerScript))
        {
            GameManagerScript = gameManagerScript;
        }
        else
        {
            Debug.LogError("Unable to locate the game manager script on the same tranform point as this.");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTicker();
    }

    void UpdateTicker()
    {
        if (ScoreTicker >= ScoreTickDuration)
        {
            UpdateScore();

            ScoreTicker = 0;
        }
        else
        {
            ScoreTicker += Time.deltaTime;
        }
    }

    void UpdateScore()
    {
        if (GameManagerScript != null)
        {
            // Update the altitude performance score for the aircraft.
            GameManagerScript.IncreaseScore(ScoreFromNormalisedRange(GameManagerScript.AircraftController.CurrentValues.ValuesHolder.HeightAboveSeaLevel, MinAltitudeValue, MaxAltitudeValue, AltitudeScoreMultiplier, MaxScoreAllowedPerTick));

        }
        else
        {
            Debug.LogError("GameManagerScript is not assigned within this script.");
        }
    }

    int ScoreFromNormalisedRange(float currentValue, float minForScore, float maxForScore, AnimationCurve animationCurve, float maxScoreAllowedPerTick)
    {
        // Get the normalised value of the current altitude as compared to the minimum and maximum allowed altitude values.
        float normalisedValue = (currentValue - minForScore) / (maxForScore - minForScore);

        // Get the score modifier for this normalised value.
        float currentScoreModifier = animationCurve.Evaluate(normalisedValue);

        return Mathf.RoundToInt(maxScoreAllowedPerTick * currentScoreModifier);
    }
}
