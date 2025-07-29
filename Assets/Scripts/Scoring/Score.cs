using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Score : MonoBehaviour
{
    public int score;

    /// <summary>
    /// The HUD UI element for the aircraft display.
    /// </summary>
    [field: SerializeField]
    public AircraftDisplay DisplayToUpdate
    {  get; set; }

    private void Awake()
    {
        if (DisplayToUpdate == null)
        {
            DisplayToUpdate = GameObject.FindAnyObjectByType<AircraftDisplay>();

            if (DisplayToUpdate == null)
            {
                Debug.LogError("Unable to locate the aircraft displayI component.");
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Debug.Log(score);
    }

    /// <summary>
    /// Ensures that all requirements are met when the player score increases.
    /// </summary>
    /// <param name="amountToIncrease"></param>
    public void IncreaseScore(int amountToIncrease)
    {
        score += amountToIncrease;

        if (DisplayToUpdate != null)
        {
            DisplayToUpdate.UpdateScoreDisplay(score);
        }
    }
}
