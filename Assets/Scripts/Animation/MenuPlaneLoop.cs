using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPlaneLoop : MonoBehaviour
{
    [field: SerializeField]
    public GameObject ObjectToMove
    { get; private set; }

    [field: SerializeField]
    public MeshRenderer StartPositions
    { get; private set; }
    
    public Vector3 StartPosition
    { get; private set; }

    [field: SerializeField]
    public MeshRenderer EndPositions
    { get; private set; }

    public Vector3 EndPosition
    { get; private set; }

    [field: SerializeField, Min(1), Range(1, 60)]
    public float TimeBetweenFlyBy
    { get; private set; } = 15.0f;

    [field: SerializeField, Min(1)]
    public float MovementSpeed
    { get; private set; } = 10.0f;

    public Coroutine FlyByRoutine
    { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        ResetPlane();

        FlyByRoutine = StartCoroutine(FlyByLoop());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool VerifyVariables()
    {
        if (ObjectToMove == null)
        {
            return false;
        }

        if (StartPositions == null)
        {
            return false;
        }

        if (EndPositions == null)
        {
            return false;
        }

        return true;
    }

    public IEnumerator FlyByLoop()
    {
        float currentTimer = 0;
        bool endHit = false;

        while (VerifyVariables())
        {
            if (endHit == false)
            {
                Vector3 newPosition = Vector3.MoveTowards(ObjectToMove.transform.position, EndPosition, MovementSpeed * Time.deltaTime);

                if (Vector3.Distance(newPosition, EndPosition) <= 0.1f)
                {
                    ResetPlane();

                    endHit = true;
                    currentTimer = 0;
                }
                else
                {
                    ObjectToMove.transform.position = newPosition;
                }
            }
            else if (endHit == true)
            {
                if (currentTimer < TimeBetweenFlyBy)
                {
                    currentTimer += Time.deltaTime;
                }
                else
                {
                    currentTimer = 0;
                    endHit = false;
                }
            }

            yield return null;
        }
    }

    public Vector3 GetWorldPointPointOnMeshRendererAxis(MeshRenderer meshToSample)
    {
        Vector3 meshExtents = meshToSample.bounds.extents;

        Vector3 randomPointLocal = new Vector3(
            ReturnRandomPointFromAxis(meshExtents.x),
            ReturnRandomPointFromAxis(meshExtents.y),
            ReturnRandomPointFromAxis(meshExtents.z)
            );

        return meshToSample.transform.position + randomPointLocal;

        static float ReturnRandomPointFromAxis(float extentsOfAxis)
        {
            return Random.Range(-extentsOfAxis, extentsOfAxis);
        }
    }

    public void ResetPlane()
    {
        if (Random.Range(0, 2) == 0)
        {
            StartPosition = GetWorldPointPointOnMeshRendererAxis(StartPositions);
            EndPosition = GetWorldPointPointOnMeshRendererAxis(EndPositions);
        }
        else
        {
            EndPosition = GetWorldPointPointOnMeshRendererAxis(StartPositions);
            StartPosition = GetWorldPointPointOnMeshRendererAxis(EndPositions);
        }
        
        // Reset the start and end positions to random points.

        ObjectToMove.transform.position = StartPosition;

        ObjectToMove.transform.LookAt(EndPosition, Vector3.up);
    }
}
