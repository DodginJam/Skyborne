using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Path
{
    public Path(Vector2 centre)
    {
        Points = new List<Vector2>
        {
            centre + Vector2.left,
            centre + (Vector2.left + Vector2.up) * 0.5f,
            centre + (Vector2.right + Vector2.down) * 0.5f,
            centre + Vector2.right
        };
    }

    public List<Vector2> Points
    { get; set; }

    public Vector2 this[int i]
    {
        get
        {
            return Points[i];
        }
    }

    public int NumPoints
    { 
        get
        {
            return Points.Count;
        }
    }

    public int NumSegments
    {
        get
        {
            return (Points.Count - 4) / 3 + 1;
        }
    }

    public void AddSegment(Vector2 anchorPos)
    {
        Points.Add(Points[Points.Count - 1] * 2 - Points[Points.Count - 2]);
        Points.Add((Points[Points.Count - 1] + anchorPos) * 0.5f);
        Points.Add(anchorPos);
    }

    public Vector2[] GetPointsInSegment(int index)
    {
        return new Vector2[]
        {
            Points[index * 3],
            Points[index * 3 + 1],
            Points[index * 3 + 2],
            Points[index * 3 + 3]
        };
    }

    public void MovePoint(int index, Vector2 position)
    {
        Vector2 deltaMove = position - Points[index];
        Points[index] = position;

        if (index % 3 == 0)
        {
            if (index + 1 < Points.Count)
            {
                Points[index + 1] += deltaMove;
            }

            if (index - 1 >= 0)
            {
                Points[index - 1] += deltaMove;
            }
        }
        else
        {
            bool nextPointIsAnchor = (index + 1) % 3 == 0;
            int corrospondingControlIndex = (nextPointIsAnchor) ? index + 2 : index - 2;
            int anchorIndex = (nextPointIsAnchor) ? index + 1 : index - 1;

            if (corrospondingControlIndex >= 0 && corrospondingControlIndex < Points.Count)
            {
                float distance = (Points[anchorIndex] - Points[corrospondingControlIndex]).magnitude;
                Vector2 direction = (Points[anchorIndex] - position).normalized;

                Points[corrospondingControlIndex] = Points[anchorIndex] + direction * distance;
            }
        }
    }
}
