using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Central point for drawning in the gameplay loop elements.
/// </summary>
public class GameManagerNew : GameManagerBase
{
    [field: SerializeField]
    public PointGenerator PointGenerator
    { get; set; }

    protected override void Awake()
    {
        base.Awake();

        if (PointGenerator == null)
        {
            PointGenerator = GameObject.FindAnyObjectByType<PointGenerator>();

            if (PointGenerator == null)
            {
                Debug.LogError("Unable to locate point generator in scene.");
            }
        }
    }

    protected override void Start()
    {
        base.Start();

        AircraftController.transform.TransformDirection(Vector3.forward);
    }
}
