using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CollisionDetection : MonoBehaviour
{
    /// <summary>
    /// Reference to the script that converts the aircraft to its destruction model.
    /// </summary>
    [field: SerializeField]
    public Destruction DestructionComp
    { get; set; }

    /// <summary>
    /// Holds the last time taken in the last FixedUpdate frame.
    /// </summary>
    public static float LastFixedDeltaTimeAmount
    { get; private set; }

    private void Awake()
    {
        if (DestructionComp == null)
        {
            DestructionComp = transform.root.GetComponentInChildren<Destruction>();

            if (DestructionComp == null)
            {
                Debug.LogError("Unable to locate the destruction component within this gameobject.");
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
        if (LastFixedDeltaTimeAmount != Time.fixedDeltaTime)
        {
            LastFixedDeltaTimeAmount = Time.fixedDeltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Projectile"))
        {
            Debug.Log($"Impluse: {(collision.impulse * LastFixedDeltaTimeAmount).sqrMagnitude}");

            if ((collision.impulse * LastFixedDeltaTimeAmount).sqrMagnitude >= DestructionComp.DestroyForce)
            {
                DestructionComp.Impact((collision.impulse * LastFixedDeltaTimeAmount));
            }
        }
    }
}
