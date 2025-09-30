using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightPeformanceTracker : MonoBehaviour
{
    [field: SerializeField]
    public GameManagerBase GameManagerScript
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

    /// <summary>
    /// The rotation value is limited to the range of a normalised dot product calculation.
    /// </summary>
    [field: SerializeField, Range(-1, 1)]
    public float MinRotationValue
    { get; private set; } = 0;

    /// <summary>
    /// The rotation value is limited to the range of a normalised dot product calculation.
    /// </summary>
    [field: SerializeField, Range(-1, 1)]
    public float MaxRotationValue
    { get; private set; } = 0;

    private void Awake()
    {
        if (TryGetComponent<GameManagerBase>(out GameManagerBase gameManagerScript))
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
        if (GameManagerScript != null && GameManagerScript.GameState == GameManagerBase.GameStatus.Playing)
        {
            UpdateTicker();
        }
    }

    /// <summary>
    /// How often the score should tick up is controlled here.
    /// </summary>
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

    /// <summary>
    /// Updates the game managers scores.
    /// </summary>
    void UpdateScore()
    {
        if (GameManagerScript != null)
        {
            // Update the altitude performance score for the aircraft.
            GameManagerScript.IncreaseScore(ScoreFromNormalisedRange(GameManagerScript.AircraftController.CurrentValues.ValuesHolder.HeightAboveSeaLevel, MinAltitudeValue, MaxAltitudeValue, AltitudeScoreMultiplier, MaxScoreAllowedPerTick));

            // Update the level peformance score for the aircraft.
            GameManagerScript.IncreaseScore(ScoreFromNormalisedRange(GameManagerScript.AircraftController.CurrentValues.ValuesHolder.LevelOfFlight, MinRotationValue, MaxRotationValue, LevelRotationMultiplier, MaxScoreAllowedPerTick));
        }
        else
        {
            Debug.LogError("GameManagerScript is not assigned within this script.");
        }
    }

    /// <summary>
    /// Takes the max and min range and uses that to create a normalised value for the current value inputted in, sampled from the animation curve (which should be a range of 0 to 1).
    /// </summary>
    /// <param name="currentValue"></param>
    /// <param name="minForScore"></param>
    /// <param name="maxForScore"></param>
    /// <param name="animationCurve"></param>
    /// <param name="maxScoreAllowedPerTick"></param>
    /// <returns></returns>
    int ScoreFromNormalisedRange(float currentValue, float minForScore, float maxForScore, AnimationCurve animationCurve, float maxScoreAllowedPerTick)
    {
        // Get the normalised value of the current altitude as compared to the minimum and maximum allowed altitude values.
        float normalisedValue = (currentValue - minForScore) / (maxForScore - minForScore);

        // Get the score modifier for this normalised value.
        float currentScoreModifier = animationCurve.Evaluate(normalisedValue);

        int score = Mathf.RoundToInt(maxScoreAllowedPerTick * currentScoreModifier);

        // Debug.Log(score + " for " + animationCurve.ToString());

        return score;
    }
}
