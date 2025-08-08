using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

public class ProceduralFlightPath : MonoBehaviour
{
    [Header("Spline Settings")]
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private float segmentLength = 100f; // Distance for each new segment
    [SerializeField] private int initialSegments = 5;    // Segments to create at start

    [Header("Debug")]
    [SerializeField] private bool generateOnStart = true;
    [SerializeField] private bool extendWithKey = true; // Press E to extend

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
        lastDirection = Vector3.forward; // Start flying forward

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

        // Calculate new point position
        Vector3 newPos = (Vector3)lastKnot.Position + lastDirection * segmentLength;

        // Add new knot
        spline.Add(new BezierKnot(newPos));

        Debug.Log($"Extended spline to point {spline.Count}");
    }
}
