using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Central point for drawning in the gameplay loop elements.
/// </summary>
public class GameManagerOld : GameManagerBase
{
    /// <summary>
    /// The reference to the GateScript spawning logic.
    /// </summary>
    [field: SerializeField]
    public GateSpawning GateSpawningScript
    { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        if (GateSpawningScript == null)
        {
            GateSpawningScript = GameObject.FindAnyObjectByType<GateSpawning>();

            if (GateSpawningScript == null)
            {
                Debug.LogWarning("Unable to locate the GateScript spawning component.");
            }
        }
    }
}
