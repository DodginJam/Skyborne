using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Central point for drawning in the gameplay loop elements.
/// </summary>
public class GameManagerBase : MonoBehaviour
{
    /// <summary>
    /// The list of HUD UI element for the aircraft display.
    /// </summary>
    public List<AircraftDisplay> DisplaysToUpdate
    { get; set; }

    /// <summary>
    /// The score tracker for updating the game loop.
    /// </summary>
    public int ScoreCount
    { get; private set; }

    /// <summary>
    /// The score for tracking the misses made by the player.
    /// </summary>
    public int PenaltyCounter
    { get; private set; }

    /// <summary>
    /// The total limit of penalitys allowed by the player before game over.
    /// </summary>
    [field: SerializeField]
    public int PenaltyLimit
    { get; private set; } = 3;

    /// <summary>
    /// Used to track the status of the player in the game to help provide control flow of logic.
    /// </summary>
    public GameStatus GameState
    { get; private set; }

    public GamePlayingStatus GamePlayStatus
    { get; private set; }

    /// <summary>
    /// Reference to the player controller script and providing access to it's gameobject too.
    /// </summary>
    [field: SerializeField]
    public AircraftController AircraftController
    { get; private set; }

    public enum GameStatus
    {
        Playing,
        Failure,
    }

    public enum GamePlayingStatus
    {
        Start,
        Gameloop,
        End
    }

    protected virtual void Awake()
    {
        // Initialisation.
        if (DisplaysToUpdate == null)
        {
            DisplaysToUpdate = GameObject.FindObjectsByType<AircraftDisplay>(FindObjectsSortMode.None).ToList();

            if (DisplaysToUpdate == null)
            {
                Debug.LogWarning("Unable to locate the aircraft display component.");
            }
        }
    }

    protected virtual void Start()
    {
        
    }

    protected virtual void FixedUpdate()
    {

    }

    /// <summary>
    /// Ensures that all requirements are met when the player ScoreCount increases.
    /// </summary>
    /// <param name="amountToIncrease"></param>
    public void IncreaseScore(int amountToIncrease)
    {
        // Prevent game progress on game over.
        if (GameState != GameStatus.Playing)
        {
            return;
        }

        // Increase the score counter and update UI is available.
        ScoreCount += amountToIncrease;

        if (DisplaysToUpdate != null)
        {
            foreach (AircraftDisplay display in DisplaysToUpdate)
            {
                display.UpdateScoreDisplay(ScoreCount);
            }
        }
    }

    public void IncreasePenalty(int amountToIncrease)
    {
        // Prevent game progress on game over.
        if (GameState != GameStatus.Playing)
        {
            return;
        }

        // Increase the penalty counter.
        PenaltyCounter += amountToIncrease;

        if (DisplaysToUpdate != null)
        {
            foreach (AircraftDisplay display in DisplaysToUpdate)
            {
                display.UpdatePenaltyDisplay(PenaltyCounter);
            }
        }

        if (PenaltyCounter >= PenaltyLimit && GameState == GameManagerBase.GameStatus.Playing)
        {
            SetGameOverState();
        }
    }

    public void SetGameOverState()
    {
        GameState = GameStatus.Failure;
        Debug.LogWarning("Game Over State Set");
    }
}
