using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointGenerator : MonoBehaviour
{
    [field: SerializeField]
    public PointsData Points
    { get; private set; }

    [field: SerializeField]
    public Transform StartLocation
    { get; private set; }

    public void CreatePoints(Vector3Int lengths, Vector3 spacings, Vector3 startLocation)
    {
        Points = new PointsData(lengths.x, lengths.y, lengths.z, spacings.x, spacings.y, spacings.z, startLocation);
    }

    public void CreatePoints(int xLength, int yLength, int zLength, float xSpacing, float ySpacing, float zSpacing, Vector3 startLocation)
    {
        Points = new PointsData(xLength, yLength, zLength, xSpacing, ySpacing, zSpacing, startLocation);
    }

    public void Awake()
    {
        if (StartLocation != null)
        {
            transform.position = StartLocation.position;
        }
        else
        {
            Debug.LogError("No start location has been passed through.");
        }
    }

    public void Start()
    {
        if (Points == null || Points.PointPositions == null)
        {
            CreatePoints(Points.GridDimension, Points.GridSpacing, transform.position);
        }

        /*
        foreach (PointData point in Points.PointPositions)
        {
            GameObject gameObjectExample = GameObject.CreatePrimitive(PrimitiveType.Cube);

            gameObjectExample.transform.position = point.WorldPosition;
        }
        */
    }
}
