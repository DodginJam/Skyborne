using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PathCreator))]
public class PathEditor : Editor
{
    [field: SerializeField]
    public PathCreator Creator
    {  get; private set; }

    public Path Path_
    {  get; private set; }

    private void OnSceneGUI()
    {
        Input();
        Draw();
    }

    void Input()
    {
        Event guiEvent = Event.current;

        Vector2 mousePosition = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;

        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
        {
            Undo.RecordObject(Creator, "Add Segment");
            Path_.AddSegment(mousePosition);
        }
    }

    void Draw()
    {
        for (int  i = 0; i < Path_.NumSegments; i++)
        {
            Vector2[] points = Path_.GetPointsInSegment(i);

            Handles.color = Color.black;

            Handles.DrawLine(points[1], points[0]);
            Handles.DrawLine(points[2], points[3]);


            Handles.color = Color.green;

            Handles.DrawBezier(
                points[0],
                points[3],
                points[1],
                points[2],
                Color.green,
                null,
                2.0f
                );
        }

        Handles.color = Color.red;
        for (int i = 0; i < Path_.NumPoints; i++)
        {
            Vector2 newPosition = Handles.FreeMoveHandle(Path_[i], 0.1f, Vector2.zero, Handles.CylinderHandleCap);

            if (Path_[i] != newPosition)
            {
                Undo.RecordObject(Creator, "Move Point");
                Path_.MovePoint(i, newPosition);
            }
        }
    }

    private void OnEnable()
    {
        Creator = (PathCreator)target;

        if (Creator.Path_ == null)
        {
            Creator.CreatePath();
        }

        Path_ = Creator.Path_;
    }
}
