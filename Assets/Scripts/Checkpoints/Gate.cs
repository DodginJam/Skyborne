using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public Score playerScore;
    public Transform playerTransform;
    public GateSpawning spawner;
    public bool missed = false;
    public int missedCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.back);
        Vector3 distanceToPlane = Vector3.Normalize(playerTransform.position - transform.position);

        if (Vector3.Dot(forward, distanceToPlane) < 0)
        {
            missed = true;
            Debug.Log("missed");
            gameObject.SetActive(false);
        }

        if (missedCount >= 3f)
        {
            Debug.Log("Game Over!");
        }
    }


    private void OnTriggerEnter(Collider other)
    {

        playerScore.score += 1;
        gameObject.SetActive(false);
    }
}