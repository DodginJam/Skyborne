using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreUpdate : MonoBehaviour
{
    public TMP_Text scoreCounter;
    public Score score;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateScore()
    {
        scoreCounter.text = "Score: " + score.score;
    }
}
