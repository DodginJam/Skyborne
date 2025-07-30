using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [field: SerializeField]
    public GameManager GameManagerScript
    {  get; private set; }

    [field: SerializeField]
    public Transform PlayerTransform 
    { get; private set; }

    [field: SerializeField]
    public GateSpawning GateSpawner 
    { get; private set; }

    public bool HasMissed     
    {  get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.back);
        Vector3 distanceToPlane = Vector3.Normalize(PlayerTransform.position - transform.position);

        if (Vector3.Dot(forward, distanceToPlane) < 0)
        {
            HasMissed = true;
            GameManagerScript.IncreasePenelty(1);
            gameObject.SetActive(false);
        }

        if (GameManagerScript.PenaltyCounter >= GameManagerScript.PenaltyLimit)
        {
            GameManagerScript.SetGameOverState();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.gameObject.CompareTag("Player"))
        {
            GameManagerScript.IncreaseScore(1);
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.root.gameObject.CompareTag("Player"))
        {
            GameManagerScript.IncreaseScore(1);
            gameObject.SetActive(false);
        }
    }
}