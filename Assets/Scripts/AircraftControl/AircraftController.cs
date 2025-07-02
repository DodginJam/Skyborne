using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftController : MonoBehaviour
{
    [field: SerializeField]
    public AircraftInput InputControls
    { get; private set; }

    [field: SerializeField]
    public AircraftValues Values
    { get; private set; }

    [field: SerializeField]
    public Rigidbody PlaneRigidBody
    {  get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rotationInput = new Vector3(InputControls.PitchInput * Values.PitchSpeed, InputControls.YawInput * Values.YawSpeed, InputControls.RollInput * Values.RollSpeed);

        transform.Rotate(Time.deltaTime * rotationInput);

        transform.Translate(Time.deltaTime * (Vector3.forward * Values.ThrustSpeed), Space.Self);
    }
}
