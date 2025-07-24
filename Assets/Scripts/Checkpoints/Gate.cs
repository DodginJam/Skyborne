using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public Score playerScore;
    public Transform playerTransform;
    public GateSpawning spawner;
    public bool missed = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 distanceToPlane = Vector3.Normalize(playerTransform.position - transform.position);

        if (Vector3.Dot(forward, distanceToPlane) < 0)
        {
            missed = true;
            Debug.Log("missed");
            gameObject.SetActive(false);
        }
    }


    private void OnTriggerEnter(Collider other)
    {

        playerScore.score += 1;
        gameObject.SetActive(false);
    }
}