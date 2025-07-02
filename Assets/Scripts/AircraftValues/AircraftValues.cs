using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftValues : MonoBehaviour
{
    [field: SerializeField]
    public AircraftStartingValues StartingValues
    {  get; private set; }

    [field: SerializeField]
    public float ThrustSpeed
    { get; set; } = 1.0f;

    [field: SerializeField]
    public float PitchSpeed
    { get; set; } = 1.0f;

    [field: SerializeField]
    public float RollSpeed
    { get; set; } = 1.0f;

    [field: SerializeField]
    public float YawSpeed
    { get; set; } = 1.0f;

    private void Awake()
    {
        InitialiseStartingValues();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitialiseStartingValues()
    {
        ThrustSpeed = StartingValues.ThrustMaxSpeed;
        PitchSpeed = StartingValues.PitchSpeed;
        RollSpeed = StartingValues.RollSpeed;
        YawSpeed = StartingValues.YawSpeed;
    }
}
