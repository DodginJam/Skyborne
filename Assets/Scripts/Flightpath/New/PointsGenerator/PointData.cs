using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointData
{
    public PointPositionalHelper PositionalHelper
    { get; set; }

    public Vector3 WorldPosition
    { get; set; }

    public PointData(Vector3 worldPosition)
    {
        WorldPosition = worldPosition;
    }

    public enum PointPositionalHelper
    {
        Normal,
        Start,
        End
    }
}


