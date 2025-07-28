using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(Collider))]
public class Ammo : MonoBehaviour
{
    public Rigidbody RigidBody
    {  get; private set; }

    public float LifetimeCounter
    { get; private set; }

    public Transform ParentTranform
    { get; set; }

    public ArmamentData AssociatedArmamentData
    { get; set; }

    public TrailRenderer TrailRenderer
    { get; private set; }

    private void Awake()
    {
        RigidBody = GetComponent<Rigidbody>();
        TrailRenderer = GetComponent<TrailRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (LifetimeCounter >= AssociatedArmamentData.Lifetime)
        {
            SetToDisable(AssociatedArmamentData);
        }
        else
        {
            LifetimeCounter += Time.deltaTime;
        }
    }

    private void OnEnable()
    {
        if (AssociatedArmamentData != null)
        {
            UpdateFromArmamentManager(AssociatedArmamentData);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            SetToDisable(AssociatedArmamentData);
        }
    }

    private void UpdateFromArmamentManager(ArmamentData armamentData)
    {
        transform.parent = null;

        transform.SetPositionAndRotation(armamentData.BarrelTransformAndPoolingParent.transform.position, armamentData.BarrelTransformAndPoolingParent.transform.rotation);

        RigidBody.AddRelativeForce(Vector3.forward * armamentData.LaunchForce, ForceMode.Impulse);
    }

    public void SetToDisable(ArmamentData armamentData)
    {
        LifetimeCounter = 0;

        transform.SetPositionAndRotation(armamentData.BarrelTransformAndPoolingParent.transform.position, armamentData.BarrelTransformAndPoolingParent.transform.rotation);

        gameObject.transform.parent = ParentTranform;

        RigidBody.velocity = Vector3.zero;

        TrailRenderer.Clear();

        gameObject.SetActive(false);
    }
}
