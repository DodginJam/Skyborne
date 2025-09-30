using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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

    public bool IsClosed
    { get; set; }

    public bool autoSetControlsPoints;
    public bool AutoSetControlsPoints
    {
        get
        {
            return autoSetControlsPoints;
        }
        set
        {
            if (autoSetControlsPoints != value)
            {
                autoSetControlsPoints = value;

                if (autoSetControlsPoints)
                {
                    AutoSetAllControlsPoints();
                }
            }
        }
    }

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
            return Points.Count / 3;
        }
    }

    public void AddSegment(Vector2 anchorPos)
    {
        Points.Add(Points[Points.Count - 1] * 2 - Points[Points.Count - 2]);
        Points.Add((Points[Points.Count - 1] + anchorPos) * 0.5f);
        Points.Add(anchorPos);

        if (AutoSetControlsPoints)
        {
            AutoSetAllAffectedControlPoints(Points.Count - 1);
        }
    }

    public Vector2[] GetPointsInSegment(int index)
    {
        return new Vector2[]
        {
            Points[index * 3],
            Points[index * 3 + 1],
            Points[index * 3 + 2],
            Points[LoopIndex(index * 3 + 3)]
        };
    }

    public void MovePoint(int index, Vector2 position)
    {
        Vector2 deltaMove = position - Points[index];

        if (index % 3 == 0 || !AutoSetControlsPoints)
        {
            Points[index] = position;

            if (AutoSetControlsPoints)
            {
                AutoSetAllAffectedControlPoints(index);
            }
            else
            {
                if (index % 3 == 0)
                {
                    if (index + 1 < Points.Count || IsClosed)
                    {
                        Points[LoopIndex(index + 1)] += deltaMove;
                    }

                    if (index - 1 >= 0 || IsClosed)
                    {
                        Points[LoopIndex(index - 1)] += deltaMove;
                    }
                }
                else
                {
                    bool nextPointIsAnchor = (index + 1) % 3 == 0;
                    int corrospondingControlIndex = (nextPointIsAnchor) ? index + 2 : index - 2;
                    int anchorIndex = (nextPointIsAnchor) ? index + 1 : index - 1;

                    if (corrospondingControlIndex >= 0 && corrospondingControlIndex < Points.Count || IsClosed)
                    {
                        float distance = (Points[LoopIndex(anchorIndex)] - Points[LoopIndex(corrospondingControlIndex)]).magnitude;
                        Vector2 direction = (Points[LoopIndex(anchorIndex)] - position).normalized;

                        Points[LoopIndex(corrospondingControlIndex)] = Points[LoopIndex(anchorIndex)] + direction * distance;
                    }
                }
            }
        }
        
    }

    public void ToggleClosed()
    {
        IsClosed = !IsClosed;

        if (IsClosed)
        {
            Points.Add(Points[Points.Count - 1] * 2 - Points[Points.Count - 2]);
            Points.Add(Points[0] * 2 - Points[1]);

            if (AutoSetControlsPoints)
            {
                AutoSetAnchorControlsPoints(0);
                AutoSetAnchorControlsPoints(Points.Count - 3);
            }
        }
        else
        {
            Points.RemoveRange(Points.Count - 2, 2);

            if (AutoSetControlsPoints)
            {
                AutoSetStartAndEndControls();
            }
        }
    }

    void AutoSetAllAffectedControlPoints(int updatedAnchorIndex)
    {
        for (int i = updatedAnchorIndex - 3; i <= updatedAnchorIndex + 3; i+= 3)
        {
            if (i >= 0 && i < Points.Count || IsClosed)
            {
                AutoSetAnchorControlsPoints(LoopIndex(i));
            }
        }

        AutoSetStartAndEndControls();
    }

    void AutoSetAllControlsPoints()
    {
        for (int i = 0; i < Points.Count; i += 3)
        {
            AutoSetAnchorControlsPoints(i);
        }

        AutoSetStartAndEndControls();
    }

    void AutoSetAnchorControlsPoints(int anchorIndex)
    {
        Vector2 anchorPos = Points[anchorIndex];
        Vector2 direction = Vector2.zero;
        float[] neighbourDistances = new float[2];

        if (anchorIndex - 3 >= 0 || IsClosed)
        {
            Vector2 offset = Points[LoopIndex(anchorIndex - 3)] - anchorPos;
            direction += offset.normalized;
            neighbourDistances[0] = offset.magnitude;
        }

        if (anchorIndex + 3 >= 0 || IsClosed)
        {
            Vector2 offset = Points[LoopIndex(anchorIndex + 3)] - anchorPos;
            direction -= offset.normalized;
            neighbourDistances[1] = -offset.magnitude;
        }

        direction.Normalize();

        for (int i = 0; i < 2; i++)
        {
            int controlIndex = anchorIndex + i * 2 - 1;
            if (controlIndex <= 0 && controlIndex < Points.Count || IsClosed)
            {
                Points[LoopIndex(controlIndex)] = anchorPos + direction * neighbourDistances[i] * 0.5f;
            }
        }
    }

    void AutoSetStartAndEndControls()
    {
        if (!IsClosed)
        {
            Points[1] = (Points[0] + Points[2]) * 0.5f;
            Points[Points.Count - 2] = (Points[Points.Count - 1] + Points[Points.Count - 3]) * 0.5f;
        }
    }

    private int LoopIndex(int i)
    {
        return (i + Points.Count) % Points.Count;
    }
}
