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
    [SerializeField] private float maxTurnAngle = 10f;       // Degrees per segment left/right
    [SerializeField] private float maxPitchAngle = 5f;       // Degrees per segment up/down
    [SerializeField] private float horizontalBias = 0.5f;    // 0 = no left/right, 1 = full range
    [SerializeField] private float verticalBias = 0.5f;      // 0 = no climb/descent, 1 = full range

    [Header("Debug")]
    [SerializeField] private bool generateOnStart = true;
    [SerializeField] private bool extendWithKey = true;

    private Spline spline;
    private Vector3 lastDirection;

    private void Awake()
    {
        spline = splineContainer.Spline;

        if (generateOnStart)
        {
            GenerateInitialPath();
        }
    }

    private void Update()
    {
        if (extendWithKey && Input.GetKeyDown(KeyCode.E))
        {
            ExtendSpline();
        }
    }

    private void GenerateInitialPath()
    {
        spline.Clear();

        Vector3 startPos = Vector3.zero;
        lastDirection = Vector3.forward;

        spline.Add(new BezierKnot(startPos));

        for (int i = 0; i < initialSegments; i++)
        {
            ExtendSpline();
        }
    }

    private void ExtendSpline()
    {
        // Get last point on spline
        int lastIndex = spline.Count - 1;
        BezierKnot lastKnot = spline[lastIndex];

        // Random gentle yaw and pitch
        float yaw = UnityEngine.Random.Range(-maxTurnAngle * horizontalBias, maxTurnAngle * horizontalBias);
        float pitch = UnityEngine.Random.Range(-maxPitchAngle * verticalBias, maxPitchAngle * verticalBias);

        // Smoothly adjust direction
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        lastDirection = rotation * lastDirection;

        // New point
        Vector3 newPos = (Vector3)lastKnot.Position + lastDirection.normalized * segmentLength;

        // Add to spline
        spline.Add(new BezierKnot(newPos));
    }
}
