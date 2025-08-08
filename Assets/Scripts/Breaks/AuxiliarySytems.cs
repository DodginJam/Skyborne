using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuxiliarySytems : MonoBehaviour
{
    [field: SerializeField]
    public AircraftInput Input
    { get; private set; }

    [field: SerializeField]
    public BrakeSystem BrakingSystem
    { get; private set; }

    private void Awake()
    {
        if (Input == null)
        {
            if (TryGetComponent<AircraftInput>(out AircraftInput input))
            {
                Input = input;
            }
            else
            {
                Debug.LogWarning("Unable to locate the aircraft input on this transform.");
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
        
    }

    private void FixedUpdate()
    {
        if (Input != null && BrakingSystem != null)
        {
            if (Input.BrakeInputHeld && BrakingSystem.AreBrakesActive == false)
            {
                BrakingSystem.SetBreaksActiveStatus(true);

                // Debug.Log("BreaksActive");
            }
            else if (!Input.BrakeInputHeld && BrakingSystem.AreBrakesActive == true)
            {
                BrakingSystem.SetBreaksActiveStatus(false);
                // Debug.Log("BreaksOff");
            }
        }
    }
}

[Serializable]
public class BrakeSystem
{
    public bool AreBrakesActive
    { get; private set; }

    [field: SerializeField]
    public List<BreakData> Brakes
    { get; private set; }

    public void SetBreaksActiveStatus(bool newStatus)
    {
        AreBrakesActive = newStatus;

        foreach (BreakData breakData in Brakes)
        {
            breakData.SetBreakStatus(newStatus);
        }
    }
}

[Serializable]
public class BreakData
{
    [field: SerializeField]
    public string Name
    { get; private set; }

    [field: SerializeField]
    public Collider BreakSurface
    { get; private set; }

    [field: SerializeField]
    public PhysicMaterial DefaultMat
    { get; private set; }

    [field: SerializeField]
    public PhysicMaterial BrakingMat
    { get; private set; }
    public bool IsBreakActive
    { get; private set; }


    public void SetBreakStatus(bool activeStatus)
    {
        IsBreakActive = activeStatus;

        if (IsBreakActive)
        {
            BreakSurface.material = BrakingMat;
        }
        else
        {
            BreakSurface.material = DefaultMat;
        }
    }
}
