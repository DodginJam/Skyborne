using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

public class ProceduralFlightPath : MonoBehaviour
{
    [Header("Spline Settings")]
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private float segmentLength = 100f;
    [SerializeField] private int initialSegments = 5;

    [Header("Flight Path Constraints")]
    [SerializeField] private float maxTurnAngle = 10f;
    [SerializeField] private float maxPitchAngle = 5f;
    [SerializeField, Range(0f, 1f)] private float horizontalBias = 0.5f;
    [SerializeField, Range(0f, 1f)] private float verticalBias = 0.5f;

    [Header("Auto-Extension")]
    [SerializeField] private Transform player;
    [Tooltip("How far (world units) the generated path should remain ahead of the player")]
    [SerializeField] private float keepAheadDistance = 500f;

    [Header("Debug")]
    [SerializeField] private bool generateOnStart = true;

    private Spline spline;
    [SerializeField] private SplineExtrude splineExtrude;
    private Vector3 lastDirection;


    private void Awake()
    {
        if (splineContainer == null)
        {
            Debug.LogError("ProceduralFlightPath: splineContainer not assigned.");
            enabled = false;
            return;
        }

        spline = splineContainer.Spline;

        if (generateOnStart)
            GenerateInitialPath();
    }

    private void Update()
    {
        AutoExtendIfNeeded();
    }

    private void GenerateInitialPath()
    {
        spline.Clear();

        // Start in local space of the spline container
        Vector3 startPos = Vector3.zero;
        spline.Add(new BezierKnot(startPos));

        // create a handful of straight segments first
        lastDirection = Vector3.forward; // local forward
        for (int i = 0; i < initialSegments; i++)
            ExtendSpline();

        // Recompute lastDirection based on the last two knots (safer)
        if (spline.Count >= 2)
        {
            float3 p1 = spline[spline.Count - 2].Position;
            float3 p2 = spline[spline.Count - 1].Position;
            Vector3 dirLocal = ((Vector3)p2 - (Vector3)p1).normalized;
            if (dirLocal.sqrMagnitude > 0f)
                lastDirection = dirLocal;
        }
    }

    private void ExtendSpline()
    {
        if (spline.Count == 0)
        {
            spline.Add(new BezierKnot(Vector3.zero));
            lastDirection = Vector3.forward;
            splineExtrude.Rebuild();
            return;
        }

        int lastIndex = spline.Count - 1;
        float3 lastPosF3 = spline[lastIndex].Position;
        Vector3 lastPos = (Vector3)lastPosF3;

        // gentle yaw/pitch around current local forward
        float yaw = UnityEngine.Random.Range(-maxTurnAngle * horizontalBias, maxTurnAngle * horizontalBias);
        float pitch = UnityEngine.Random.Range(-maxPitchAngle * verticalBias, maxPitchAngle * verticalBias);

        Quaternion rot = Quaternion.Euler(pitch, yaw, 0f);
        lastDirection = rot * lastDirection;
        lastDirection = lastDirection.normalized;

        Vector3 newLocalPos = lastPos + lastDirection * segmentLength;

        spline.Add(new BezierKnot(newLocalPos));

        splineExtrude.Rebuild();

        // Mark dirty in editor so changes persist
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(splineContainer);
#endif
    }

    private void AutoExtendIfNeeded()
    {
        if (player == null || spline.Count == 0 || splineContainer == null) return;

        // Get last knot position (local) and convert to world
        float3 lastKnotLocal = spline[spline.Count - 1].Position;
        Vector3 lastKnotWorld = splineContainer.transform.TransformPoint((Vector3)lastKnotLocal);

        float distanceAhead = Vector3.Distance(player.position, lastKnotWorld);

        // If the player is closer than keepAheadDistance to the end, extend until it isn't (with safety cap)
        int safety = 0;
        const int maxExtensionsPerFrame = 20; // avoid infinite loops
        while (distanceAhead < keepAheadDistance && safety < maxExtensionsPerFrame)
        {
            ExtendSpline();

            // recompute last world pos and distance
            lastKnotLocal = spline[spline.Count - 1].Position;
            lastKnotWorld = splineContainer.transform.TransformPoint((Vector3)lastKnotLocal);
            distanceAhead = Vector3.Distance(player.position, lastKnotWorld);

            safety++;
        }

        if (safety >= maxExtensionsPerFrame)
            Debug.LogWarning("ProceduralFlightPath: reached max extensions per frame; increase maxExtensionsPerFrame if needed.");
    }

#if UNITY_EDITOR
    // Convenience editor helper to regenerate quickly
    [ContextMenu("Regenerate Path (editor)")]
    private void EditorRegeneratePath()
    {
        GenerateInitialPath();
    }
#endif
}
