using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PointsData
{
    public PointsData(int xLength, int yLength, int zLength, Vector3 startLocation)
    {
        X_Dimension = xLength;
        Y_Dimension = yLength;
        Z_Dimension = zLength;

        GeneratePoints(X_Dimension, Y_Dimension, Z_Dimension, startLocation);
    }

    [field: SerializeField, Min(1)]
    public int X_Dimension
    { get; set; }

    [field: SerializeField, Min(1)]
    public int Y_Dimension
    { get; set; }

    [field: SerializeField, Min(1)]
    public int Z_Dimension
    { get; set; }

    public List<Vector3> PointPositions
    { get; set; } = new List<Vector3>();

    public void GeneratePoints(int x, int y, int z, Vector3 startLocation)
    {
        for(int i_x = 0; i_x < x; i_x ++)
        {
            for (int i_y = 0; i_y < x; i_y ++)
            {
                for (int i_z = 0; i_z < x; i_z++)
                {
                    PointPositions.Add(new Vector3(startLocation.x + i_x, startLocation.y + i_y, startLocation.z + i_z));
                }
            }
        }
    }
}
