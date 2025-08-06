using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(Collider))]
public class Ammo : MonoBehaviour
{
    /// <summary>
    /// Reference to the rigidbody of the ammo.
    /// </summary>
    public Rigidbody RigidBody
    {  get; private set; }

    /// <summary>
    /// The lifetime of the ammo before its gameobject is rendered inactive.
    /// </summary>
    public float LifetimeCounter
    { get; private set; }

    /// <summary>
    /// The transform location it should be parented under upon deactivation.
    /// </summary>
    public Transform ParentTranform
    { get; set; }

    /// <summary>
    /// The data used to inform the ammo class of certain data.
    /// </summary>
    public ArmamentData AssociatedArmamentData
    { get; set; }

    /// <summary>
    /// The trail renderer reference.
    /// </summary>
    public TrailRenderer TrailRenderer
    { get; private set; }

    private void Awake()
    {
        // Initialisation.
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
        // The timer for counting down the lifetime of the ammo before it is set to deactive.
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
        // Access information from the associated aramament data is it has been provided.
        if (AssociatedArmamentData != null)
        {
            UpdateFromArmamentManager(AssociatedArmamentData);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            // If the object hit is not parented to the current player, don't allow collision.
            if (!collision.transform.root.CompareTag("Player"))
            {
                SetToDisable(AssociatedArmamentData);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            // If the object hit is not parented to the current player, don't allow collision.
            if (!other.transform.root.CompareTag("Player"))
            {
                SetToDisable(AssociatedArmamentData);
            }
        }
    }

    /// <summary>
    /// Access the positional and force data from the armament data for when it is enabled.
    /// </summary>
    /// <param name="armamentData"></param>
    private void UpdateFromArmamentManager(ArmamentData armamentData)
    {
        transform.parent = null;

        transform.SetPositionAndRotation(armamentData.BarrelTransformAndPoolingParent.transform.position, armamentData.BarrelTransformAndPoolingParent.transform.rotation);

        RigidBody.AddRelativeForce(Vector3.forward * armamentData.LaunchForce, ForceMode.Impulse);
    }

    /// <summary>
    /// Before setting the gameobject flag to disable, update the elements of the ammo to reset it. 
    /// </summary>
    /// <param name="armamentData"></param>
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
