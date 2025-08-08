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
    public BreakSystem BreakingSystem
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
        if (Input != null && BreakingSystem != null)
        {
            if (Input.BreakInputHeld && BreakingSystem.AreBreaksActive == false)
            {
                BreakingSystem.SetBreaksActiveStatus(true);

                Debug.Log("BreaksActive");
            }
            else if (!Input.BreakInputHeld && BreakingSystem.AreBreaksActive == true)
            {
                BreakingSystem.SetBreaksActiveStatus(false);
                Debug.Log("BreaksOff");
            }
        }
    }
}

[Serializable]
public class BreakSystem
{
    public bool AreBreaksActive
    { get; private set; }

    [field: SerializeField]
    public List<BreakData> Breaks
    { get; private set; }

    public void SetBreaksActiveStatus(bool newStatus)
    {
        AreBreaksActive = newStatus;

        foreach (BreakData breakData in Breaks)
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
    public PhysicMaterial BreakingtMat
    { get; private set; }
    public bool IsBreakActive
    { get; private set; }


    public void SetBreakStatus(bool activeStatus)
    {
        IsBreakActive = activeStatus;

        if (IsBreakActive)
        {
            BreakSurface.material = BreakingtMat;
        }
        else
        {
            BreakSurface.material = DefaultMat;
        }
    }
}
