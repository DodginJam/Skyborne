using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision : MonoBehaviour
{
    public Collider ColliderComp
    {  get; private set; }

    public Destruction DestructionComp
    { get; set; }

    public static float LastFixedDeltaTimeAmount
    { get; private set; }

    private void Awake()
    {
        if (TryGetComponent<Collider>(out Collider collider))
        {
            ColliderComp = collider;
        }
        else
        {
            Debug.LogWarning($"No collider found on this transfrom object {transform.name}");
        }
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

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
        // Issue with the rigidbody contained in the main aircraft not enabling the collision for these colliders this script is attached too. The collisions seems to happen only when a rigidbody is attached directly to them.

        DestructionComp.Impact((collision.impulse * LastFixedDeltaTimeAmount));


        if ((collision.impulse * LastFixedDeltaTimeAmount).sqrMagnitude >= Vector3.zero.sqrMagnitude)
        {
            DestructionComp.Impact((collision.impulse * LastFixedDeltaTimeAmount));
        }
    }
}
