using System.Collections;
using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

public class GateSpawning : MonoBehaviour
{
    [field: Header("Spawn Area Settings")] 
    [field: SerializeField] public float Radius { get; private set; } = 125f;
    [field: SerializeField, Range(10, 80)] public float Angle { get; private set; } = 10f;
    [field: SerializeField, Range(10, 80)] public float StartingAngle { get; private set; } = 10f;
    [field: SerializeField] public float MinDistanceFromPlayer { get; private set; } = 50f;

    [field: Header("Timing Settings")]
    [field: SerializeField] public float SpawnDelay { get; private set; } = 5f;

    [field: Header("References")]
    [field: SerializeField] public AircraftController AircraftController { get; private set; }
    [field: SerializeField] public GameObject GateInstance { get; private set; }
    [field: SerializeField] public Gate GateScript { get; private set; }
    [field: SerializeField] public GameManager GameManagerScript { get; private set; }

    [Header("Spline Path")]
    [SerializeField] private SplineContainer flightPath;
    [SerializeField] private float gateSpacingT = 0.05f;
    [SerializeField] private float lateralOffsetRange = 15f;
    [SerializeField] private float verticalOffsetRange = 10f;

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
        Angle = StartingAngle + (GameManagerScript.ScoreCount * 5f);
        if (Angle > 80f)
        {
            Angle = 80f;
        }
    }

    private void Update()
    {
        if (GateScript.HasMissed)
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

                Vector3 spawnPos;
                Quaternion spawnRot;

                GetGateSpawnFromSpline(out spawnPos, out spawnRot);

                GateInstance.transform.position = spawnPos;
                GateInstance.transform.rotation = spawnRot;
                GateInstance.SetActive(true);
            }

            yield return null;
        }
    }

    private void GetGateSpawnFromSpline(out Vector3 position, out Quaternion rotation)
    {
        Spline spline = flightPath.Spline;
        Transform splineTransform = flightPath.transform;

        // Project player position onto spline to get current T
        float3 playerLocalPos = splineTransform.InverseTransformPoint(AircraftController.transform.position);
        SplineUtility.GetNearestPoint(spline, playerLocalPos, out float3 _, out float nearestT);

        // Spawn ahead of the player
        float spawnT = math.clamp(nearestT + gateSpacingT, 0f, 1f);

        // Evaluate the spline at the spawn point
        SplineUtility.Evaluate(spline, spawnT, out float3 pos, out float3 tangent, out float3 up);

        Vector3 worldPos = splineTransform.TransformPoint((Vector3)pos);
        Vector3 worldForward = splineTransform.TransformDirection((Vector3)tangent).normalized;
        Vector3 worldUp = splineTransform.TransformDirection((Vector3)up).normalized;
        Vector3 worldRight = Vector3.Cross(worldUp, worldForward).normalized;

        float lateralOffset = UnityEngine.Random.Range(-lateralOffsetRange, lateralOffsetRange);
        float verticalOffset = UnityEngine.Random.Range(-verticalOffsetRange, verticalOffsetRange);

        position = worldPos + worldRight * lateralOffset + worldUp * verticalOffset;
        rotation = Quaternion.LookRotation(worldForward, worldUp);
    }

    private void OnDrawGizmosSelected()
    {
        if (AircraftController == null) return;

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
