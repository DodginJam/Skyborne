using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The data for each point in a generated grid. Hold data beyond that of just position, such as if it on start or end of a grid.
/// </summary>
public class PointData
{
    /// <summary>
    /// Enum variable for whether the point is at the edge of the grid or not.
    /// </summary>
    public PointPositionalHelper PositionalHelper
    { get; set; }

    /// <summary>
    /// The world position that the point is located at.
    /// </summary>
    public Vector3 WorldPosition
    { get; set; }

    /// <summary>
    /// Constructor for a data point.
    /// </summary>
    /// <param name="worldPosition"></param>
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


