using System.Collections;
using UnityEngine;

public class GateSpawning : MonoBehaviour
{
    [field: SerializeField, Header("Spawn Area Settings")] 
    public float Radius 
    { get; private set; } = 125f;

    [field: SerializeField, Range(10, 80)]
    public float Angle
    { get; private set; } = 10f;

    [field: SerializeField, Range(10, 80)]
    public float StartingAngle 
    { get; private set; } = 10f;

    [field: SerializeField]
    public float MinDistanceFromPlayer 
    { get; private set; } = 50f;


    [field: SerializeField, Header("Timing Settings")]
    public float SpawnDelay 
    { get; private set; } = 5f;


    [field: SerializeField, Header("References")]
    public AircraftController AircraftController
    { get; private set; }

    [field: SerializeField]
    public GameObject GateInstance
    { get; private set; }

    [field: SerializeField]
    public Gate GateScript
    { get; private set; }

    [field: SerializeField]
    public GameManager GameManagerScript
    { get; private set; }


    private void Start()
    {
        if (GateInstance == null)
        {
            GateInstance = GameObject.FindGameObjectWithTag("Gate");
            if (GateInstance == null)
            {
                Debug.LogError("Gate reference not assigned or found!");
                return;
            }
        }
        StartCoroutine(GateRoutine());
    }

    private void FixedUpdate()
    {
        Angle = StartingAngle + (GameManagerScript.ScoreCount * 5f); // Temporary method of increasing Angle
        if (Angle > 80f)
        {
            Angle = 80f;
        }
    }

    private void Update()
    {
        if (GateScript.HasMissed == true)
        {
            GateScript.HasMissed = false;
            Angle -= 10f;
            if (Angle < 10f)
            {
                Angle = 10f;
            }
        }
    }

    private IEnumerator GateRoutine()
    {
        while (true)
        {
            if (!GateInstance.activeSelf)
            {
                yield return new WaitForSeconds(SpawnDelay);

                Vector3 newPos = FindValidSpawnPos();
                GateInstance.transform.position = newPos;
                GateInstance.transform.rotation = AircraftController.transform.rotation;
                GateInstance.SetActive(true);
            }
            yield return null;
        }
    }

    private Vector3 FindValidSpawnPos()
    {
        for (int i = 0; i < 30; i++) // Try up to 30 times
        {
            float randAngle = Random.Range(-Angle / 2f, Angle / 2f);
            float randDistance = Random.Range(MinDistanceFromPlayer, Radius); // Might change to fixed distance later

            Vector3 direction = Quaternion.Euler(0, randAngle, 0) * AircraftController.transform.forward;
            Vector3 potentialPos = AircraftController.transform.position + direction.normalized * randDistance;

            float playerDistance = Vector3.Distance(potentialPos, AircraftController.transform.position);
            if (playerDistance >= MinDistanceFromPlayer)
            {
                return potentialPos;
            }
        }
        Debug.LogWarning("Could not find valid spawn position after 30 attempts, using fallback.");
        return AircraftController.transform.position + AircraftController.transform.forward * MinDistanceFromPlayer;
    }

    //Gizmos for viewing GateScript spawn area in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(AircraftController.transform.position, Radius);

        Vector3 forward = AircraftController.transform.forward * Radius;
        Vector3 leftBoundary = Quaternion.Euler(0, -Angle / 2, 0) * forward;
        Vector3 rightBoundary = Quaternion.Euler(0, Angle / 2, 0) * forward;

        Gizmos.color = Color.green;
        Gizmos.DrawRay(AircraftController.transform.position, leftBoundary);
        Gizmos.DrawRay(AircraftController.transform.position, rightBoundary);
    }
}
