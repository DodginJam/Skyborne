using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmamentsManager : MonoBehaviour
{
    /// <summary>
    /// Reference to the component where the player input is received and processed by the New Input System into normalised values for aircraft control.
    /// </summary>
    [field: SerializeField]
    public AircraftInput InputControls
    { get; private set; }

    /// <summary>
    /// List of the armaments that can be utilised by the armament manager.
    /// </summary>
    [field: SerializeField]
    public List<ArmamentData> ArmamentDatas
    { get; private set; }

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate all the armament data.
        if (ArmamentDatas != null && ArmamentDatas.Count > 0)
        {
            foreach (ArmamentData armData in ArmamentDatas)
            {
                // Begin the instantiation of all the ammo object pooling into their respective parents.
                for (int i = 0; i < armData.AmmoPooling.Length; i++)
                {
                    armData.AmmoPooling[i] = Instantiate(armData.AmmoPrefab, armData.BarrelTransformAndPoolingParent.position, armData.BarrelTransformAndPoolingParent.rotation, armData.BarrelTransformAndPoolingParent);

                    // Pass the ammo the reference to the armament data.
                    if (armData.AmmoPooling[i].TryGetComponent<Ammo>(out Ammo ammo))
                    {
                        ammo.AssociatedArmamentData = armData;
                        ammo.ParentTranform = armData.BarrelTransformAndPoolingParent;
                    }
                    else
                    {
                        Debug.LogError("Ammo type not found on object being used as ammo.");
                    }

                    armData.AmmoPooling[i].SetActive(false);

                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (InputControls.IsFiring == true && InputControls.FireSafetyDisabled)
        {
            // Cycle through all the armaments for firing checks.
            foreach (ArmamentData armData in ArmamentDatas)
            {
                // Count up the time passed in the frame.
                armData.FireTimeCounter += Time.deltaTime;

                // Calculate the time required to pass for this armament type to allow the next shot.
                float timeBetweenEachShot = 1f / armData.FireRatePerSecond;

                if (armData.FireTimeCounter >= timeBetweenEachShot)
                {
                    // Loop through the available ammo to find an inactive ammo to be able to use for firing. 
                    for (int i = 0; i < armData.AmmoPooling.Length; i++)
                    {
                        if (armData.AmmoPooling[i].activeInHierarchy == false)
                        {
                            armData.AmmoPooling[i].SetActive(true);
                            break;
                        }
                    }

                    // On a successful fire, reset the aramaments FireTimeCounter.
                    armData.FireTimeCounter = 0;
                }
            }
        }
    }

}

[Serializable]
public class ArmamentData
{
    [field: SerializeField]
    public string Name
    { get; private set; }

    /// <summary>
    /// Location that the ammo should fire from.
    /// </summary>
    [field: SerializeField]
    public Transform BarrelTransformAndPoolingParent
    { get; private set; }

    /// <summary>
    /// The game object for the ammo.
    /// </summary>
    [field: SerializeField]
    public GameObject AmmoPrefab
    { get; private set; }

    /// <summary>
    /// The liftime for the ammo before despawning.
    /// </summary>
    [field: SerializeField]
    public float FireRatePerSecond
    { get; private set; } = 10.0f;

    /// <summary>
    /// The liftime for the ammo before becoming inactive into the object pool.
    /// </summary>
    [field: SerializeField]
    public float Lifetime
    { get; private set; } = 10.0f;

    /// <summary>
    /// The force that the ammo should be launched with via its rigidibody componment.
    /// </summary>
    [field: SerializeField]
    public float LaunchForce
    { get; private set; } = 1.0f;

    /// <summary>
    /// Reference to all gameobjects being pooled during runtime.
    /// </summary>
    public GameObject[] AmmoPooling
    { get; private set; } = new GameObject[300];

    /// <summary>
    /// Used to track the time for firing between shot during runtime.
    /// </summary>
    public float FireTimeCounter
    { get; set; } = 0f;
}
