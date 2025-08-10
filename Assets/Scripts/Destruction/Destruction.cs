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

    public List<Collider> AirPlaneActiveColliders
    { get; private set; } = new List<Collider>();

    public List<Rigidbody> InactiveRigidBodiesToRelease
    { get; private set; } = new List<Rigidbody>();

    public bool HasImpactOccured
    { get; private set; } = false;

    [field: SerializeField]
    public float DestroyForce 
    { get; private set; }

    [field: SerializeField]
    public CameraManager CameraManagerScript
    { get; private set; }

    public GameManager GameManagerScript
    { get; private set; }

    private void Awake()
    {
        if (CameraManagerScript == null)
        {
            CameraManagerScript = GameObject.FindAnyObjectByType<CameraManager>();
            
            if (CameraManagerScript == null)
            {
                Debug.LogError("Unable to locate camera manager in scene.");
            }
        }

        if (GameManagerScript == null)
        {
            GameManagerScript = GameObject.FindAnyObjectByType<GameManager>();

            if (GameManagerScript == null)
            {
                Debug.LogError("Unable to locate the GameManagerScript.");
            }
        }

        // Get the planes colliders active as used during flight, and add the DetectCollision script to them.
        Collider[] activeAircraftColliders = MainAircraft.transform.GetComponentsInChildren<Collider>(false);

        foreach (Collider collider in activeAircraftColliders)
        {
            AirPlaneActiveColliders.Add(collider);
        }

        // Grab reference to all the rigidbodies held as children under this transform which are the inactive debris of the aircraft.
        InactiveRigidBodiesToRelease = this.transform.GetComponentsInChildren<Rigidbody>(true).ToList();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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

            CameraManagerScript.CreateEmptyCameraTransform(MainAircraft.transform.position);

            MainAircraft.transform.gameObject.SetActive(false);

            GameManagerScript.SetGameOverState();
        }
    }
}
