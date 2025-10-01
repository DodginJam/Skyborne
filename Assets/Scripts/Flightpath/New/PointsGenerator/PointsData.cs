using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PointsData
{
    public PointsData(int xLength, int yLength, int zLength, float gridSpacing, Vector3 startLocation)
    {
        X_Dimension = xLength;
        Y_Dimension = yLength;
        Z_Dimension = zLength;
        GridSpacing = gridSpacing;

        GeneratePoints(X_Dimension, Y_Dimension, Z_Dimension, GridSpacing, startLocation);
    }

    [field: SerializeField, Min(1)]
    public int X_Dimension
    { get; set; } = 1;

    [field: SerializeField, Min(1)]
    public int Y_Dimension
    { get; set; } = 1;

    [field: SerializeField, Min(1)]
    public int Z_Dimension
    { get; set; } = 1;

    [field: SerializeField, Min(1)]
    public float GridSpacing
    { get; set; } = 1;

    public List<PointData> PointPositions
    { get; set; } = new List<PointData>();

    public void GeneratePoints(int x, int y, int z, float gridSpacing, Vector3 startLocation)
    {
        PointPositions.Clear();

        for (int i_x = 0; i_x < x; i_x ++)
        {
            for (int i_y = 0; i_y < y; i_y ++)
            {
                for (int i_z = 0; i_z < z; i_z++)
                {
                    Vector3 worldPosition = new Vector3(startLocation.x + (i_x * gridSpacing), startLocation.y + (i_y * gridSpacing), startLocation.z + (i_z * gridSpacing));

                    PointData newPoint = new PointData(worldPosition);

                    if (i_x == 0 || i_y == 0 || i_z == 0)
                    {
                        newPoint.PositionalHelper = PointData.PointPositionalHelper.Start;
                    }
                    else if (i_x == x - 1 || i_y == y - 1 || i_z == z - 1)
                    {
                        newPoint.PositionalHelper = PointData.PointPositionalHelper.End;
                    }

                    PointPositions.Add(newPoint);
                }
            }
        }
    }
}
