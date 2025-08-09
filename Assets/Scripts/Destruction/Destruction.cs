using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Destruction : MonoBehaviour
{
    [field: SerializeField]
    public Rigidbody MainAircraft
    {  get; private set; }

    public List<DetectCollision> AirPlaneActiveColliders
    { get; private set; } = new List<DetectCollision>();

    public List<Rigidbody> InactiveRigidBodiesToRelease
    { get; private set; } = new List<Rigidbody>();

    public bool HasImpactOccured
    { get; private set; } = false;

    private void Awake()
    {
        // Get the planes colliders active as used during flight, and add the DetectCollision script to them.
        Collider[] activeAircraftColliders = MainAircraft.transform.GetComponentsInChildren<Collider>(false);

        foreach (Collider collider in activeAircraftColliders)
        {
            DetectCollision dtCollision = collider.transform.AddComponent<DetectCollision>();

            // Rigidbody rb = dtCollision.transform.AddComponent<Rigidbody>();

            dtCollision.DestructionComp = this;

            AirPlaneActiveColliders.Add(dtCollision);
        }

        // Grab reference to all the rigidbodies held as children under this transform which are the inactive debris of the aircraft.
        InactiveRigidBodiesToRelease = this.transform.GetComponentsInChildren<Rigidbody>(true).ToList();
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"{AirPlaneActiveColliders.Count}");
        Debug.Log($"{InactiveRigidBodiesToRelease.Count}");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Impact(Vector3.one * 50f);
        }
    }

    public void Impact(Vector3 impactForce)
    {
        if (HasImpactOccured == false)
        {
            HasImpactOccured = true;

            foreach(Rigidbody rb in InactiveRigidBodiesToRelease)
            {
                rb.transform.parent = null;
                rb.gameObject.SetActive(true);
                rb.AddForce(impactForce, ForceMode.Impulse);
            }

            MainAircraft.transform.gameObject.SetActive(false);
        }
    }
}
