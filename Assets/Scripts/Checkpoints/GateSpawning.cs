using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

public class GateSpawning : MonoBehaviour
{
    [Header("Spawn Area Settings")]
    [SerializeField] private float Radius = 125f;
    [SerializeField, Range(10, 80)] private float Angle = 10f;
    [SerializeField, Range(10, 80)] private float StartingAngle = 10f;

    [Header("Timing Settings")]
    [SerializeField] private float SpawnDelay = 0.5f; // shorter for smooth reposition

    [Header("References")]
    [SerializeField] private AircraftController AircraftController;
    [SerializeField] private List<GameObject> Gates; // Assign 3 gate objects in Inspector
    [SerializeField] private GameManagerBase GameManagerScript;

    [Header("Spline Path")]
    [SerializeField] private SplineContainer flightPath;
    [SerializeField] private float gateSpacingT = 0.05f;
    [SerializeField] private float lateralOffsetRange = 15f;
    [SerializeField] private float verticalOffsetRange = 10f;

    private float nextGateT; // Where to spawn the next gate on the spline

    private void Start()
    {
        if (Gates == null || Gates.Count == 0)
        {
            Debug.LogError("Assign 3 gates in the inspector.");
            return;
        }

        // Start just ahead of the player
        float3 playerLocalPos = flightPath.transform.InverseTransformPoint(AircraftController.transform.position);
        SplineUtility.GetNearestPoint(flightPath.Spline, playerLocalPos, out _, out float nearestT);
        nextGateT = nearestT + gateSpacingT;

        // Place initial gates
        for (int i = 0; i < Gates.Count; i++)
        {
            Vector3 pos;
            Quaternion rot;
            GetGateSpawnFromSpline(nextGateT + gateSpacingT * i, out pos, out rot);
            Gates[i].transform.SetPositionAndRotation(pos, rot);
            Gates[i].SetActive(true);
        }

        StartCoroutine(GateRoutine());
    }

    private void FixedUpdate()
    {
        Angle = StartingAngle + (GameManagerScript.ScoreCount * 5f);
        Angle = Mathf.Clamp(Angle, 10f, 80f);
    }

    private IEnumerator GateRoutine()
    {
        while (true)
        {
            // Check if any gates have been passed/disabled, then move them forward
            for (int i = 0; i < Gates.Count; i++)
            {
                if (!Gates[i].activeSelf)
                {
                    nextGateT += gateSpacingT;
                    Vector3 pos;
                    Quaternion rot;
                    GetGateSpawnFromSpline(nextGateT + (gateSpacingT * (Gates.Count - 1)), out pos, out rot);
                    Gates[i].transform.SetPositionAndRotation(pos, rot);
                    Gates[i].SetActive(true);
                }
            }
            yield return new WaitForSeconds(SpawnDelay);
        }
    }

    private void GetGateSpawnFromSpline(float tValue, out Vector3 position, out Quaternion rotation)
    {
        Spline spline = flightPath.Spline;
        Transform splineTransform = flightPath.transform;

        float spawnT = math.clamp(tValue, 0f, 1f);
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
