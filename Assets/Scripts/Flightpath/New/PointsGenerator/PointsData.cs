using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PointsData
{
    public PointsData(int xLength, int yLength, int zLength, float gridSpacingX, float gridSpacingY, float gridSpacingZ, Vector3 startLocation)
    {
        GridDimension = new Vector3Int(xLength, yLength, zLength);

        GridSpacing = new Vector3(gridSpacingX, gridSpacingY, gridSpacingZ);

        GeneratePoints(GridDimension, GridSpacing, startLocation);
    }

    [field: SerializeField, Min(1)]
    public Vector3Int GridDimension
    { get; set; } = Vector3Int.one;

    [field: SerializeField, Min(1)]
    public Vector3 GridSpacing
    { get; set; } = Vector3.one;


    public List<PointData> PointPositions
    { get; set; } = new List<PointData>();

    public void GeneratePoints(Vector3Int dimensions, Vector3 spacings, Vector3 startLocation)
    {
        PointPositions.Clear();

        startLocation = new Vector3(startLocation.x - ((dimensions.x - 1) * spacings.x / 2), startLocation.y, startLocation.z - ((dimensions.z - 1) * spacings.z / 2));

        for (int i_x = 0; i_x < dimensions.x; i_x ++)
        {
            for (int i_y = 0; i_y < dimensions.y; i_y ++)
            {
                for (int i_z = 0; i_z < dimensions.z; i_z++)
                {
                    Vector3 worldPosition = new Vector3(startLocation.x + (i_x * spacings.x), startLocation.y + (i_y * spacings.y), startLocation.z + (i_z * spacings.z));

                    PointData newPoint = new PointData(worldPosition);

                    if (i_x == 0 || i_y == 0 || i_z == 0)
                    {
                        newPoint.PositionalHelper = PointData.PointPositionalHelper.Start;
                    }
                    else if (i_x == dimensions.x - 1 || i_y == dimensions.y - 1 || i_z == dimensions.z - 1)
                    {
                        newPoint.PositionalHelper = PointData.PointPositionalHelper.End;
                    }

                    PointPositions.Add(newPoint);
                }
            }
        }
    }
}
