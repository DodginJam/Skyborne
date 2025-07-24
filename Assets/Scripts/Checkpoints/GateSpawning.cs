using System.Collections;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class GateSpawning : MonoBehaviour
{
    [Header("Spawn Area Settings")]
    public float radius = 125f;
    [Range(10, 80)]
    public float angle = 10f;
    [Range(10, 80)]
    public float startingAngle = 10f;
    public float minDistanceFromPlayer = 50f;

    [Header("Timing Settings")]
    public float spawnDelay = 5f;

    [Header("References")]
    public Transform playerRef;
    public GameObject gateRef;
    public Gate gate;
    public Score playerScore;

    private void Start()
    {
        if (gateRef == null)
        {
            gateRef = GameObject.FindGameObjectWithTag("Gate");
            if (gateRef == null)
            {
                Debug.LogError("Gate reference not assigned or found!");
                return;
            }
        }
        StartCoroutine(GateRoutine());
    }

    private void FixedUpdate()
    {
        angle = startingAngle + (playerScore.score * 5f); // Temporary method of increasing angle
        if (angle > 80f)
        {
            angle = 80f;
        }
    }

    private void Update()
    {
        if (gate.missed == true)
        {
            gate.missed = false;
            angle -= 10f;
            if (angle < 10f)
            {
                angle = 10f;
            }
        }
    }

    private IEnumerator GateRoutine()
    {
        while (true)
        {
            if (!gateRef.activeSelf)
            {
                yield return new WaitForSeconds(spawnDelay);

                Vector3 newPos = FindValidSpawnPos();
                gateRef.transform.position = newPos;
                gateRef.transform.rotation = playerRef.rotation;
                gateRef.SetActive(true);
            }
            yield return null;
        }
    }

    private Vector3 FindValidSpawnPos()
    {
        for (int i = 0; i < 30; i++) // Try up to 30 times
        {
            float randAngle = Random.Range(-angle / 2f, angle / 2f);
            float randDistance = Random.Range(minDistanceFromPlayer, radius); // Might change to fixed distance later

            Vector3 direction = Quaternion.Euler(0, randAngle, 0) * transform.forward;
            Vector3 potentialPos = transform.position + direction.normalized * randDistance;

            float playerDistance = Vector3.Distance(potentialPos, playerRef.position);
            if (playerDistance >= minDistanceFromPlayer)
            {
                return potentialPos;
            }
        }
        Debug.LogWarning("Could not find valid spawn position after 30 attempts, using fallback.");
        return transform.position + transform.forward * minDistanceFromPlayer;
    }

    //Gizmos for viewing gate spawn area in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);

        Vector3 forward = transform.forward * radius;
        Vector3 leftBoundary = Quaternion.Euler(0, -angle / 2, 0) * forward;
        Vector3 rightBoundary = Quaternion.Euler(0, angle / 2, 0) * forward;

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, leftBoundary);
        Gizmos.DrawRay(transform.position, rightBoundary);
    }
}
