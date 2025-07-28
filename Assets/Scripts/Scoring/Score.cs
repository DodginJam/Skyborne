using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Score : MonoBehaviour
{
    public int score;

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

    public void IncreaseScore(int amountToIncrease)
    {
        score += amountToIncrease;

        if (DisplayToUpdate != null)
        {
            DisplayToUpdate.UpdateScoreDisplay(score);
        }
    }
}
