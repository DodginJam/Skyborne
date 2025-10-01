using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointGenerator : MonoBehaviour
{
    [field: SerializeField]
    public PointsData Points
    { get; private set; }

    public void CreatePoints(int xLength, int yLength, int zLength, float gridSpacing, Vector3 startLocation)
    {
        Points = new PointsData(xLength, yLength, zLength, gridSpacing, startLocation);
    }
}
