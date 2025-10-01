using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PointGenerator))]
public class PointGeneratorEditor : Editor
{
    public PointsData Points
    { get; set; }

    public PointGenerator PointsGenerator
    { get; set; }

    public float PointDisplaySize
    { get; set; } = 1f;

    public bool LiveUpdatePoints
    { get; private set; } = false;

    public bool ShowPointsVisual
    { get; private set; } = false;

    public bool ShowSpacingVisual
    { get; private set; } = false;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(20);
        GUILayout.Label("Editor Options");
        GUILayout.Space(10);

        bool newLiveUpdate = GUILayout.Toggle(LiveUpdatePoints, "Live Update Points");
        if (newLiveUpdate != LiveUpdatePoints)
        {
            LiveUpdatePoints = newLiveUpdate;
            SceneView.RepaintAll();
        }

        bool newShowPoints = GUILayout.Toggle(ShowPointsVisual, "Toggle Points Visual");
        if (newShowPoints != ShowPointsVisual)
        {
            ShowPointsVisual = newShowPoints;
            SceneView.RepaintAll();
        }

        bool newShowSpacing = GUILayout.Toggle(ShowSpacingVisual, "Show Spacing Visual");
        if (newShowSpacing != ShowSpacingVisual)
        {
            ShowSpacingVisual = newShowSpacing;
            SceneView.RepaintAll();
        }

        float newPointSize = EditorGUILayout.Slider(PointDisplaySize, 0.01f, 100.0f);
        if (newPointSize != PointDisplaySize)
        {
            PointDisplaySize = newPointSize;
            SceneView.RepaintAll();
        }

        if (GUILayout.Button("Create New PointsData"))
        {
            GeneratePoints();

            SceneView.RepaintAll();
        }

    }

    private void OnEnable()
    {
        PointsGenerator = (PointGenerator)target;

        if (PointsGenerator.Points == null)
        {
            PointsGenerator.CreatePoints(1, 1, 1, 1, 1, 1, PointsGenerator.transform.position);
            Points = PointsGenerator.Points;
        }

        Points = PointsGenerator.Points;
    }

    private void OnSceneGUI()
    {
        DrawPoints();
    }

    void DrawPoints()
    {
        if (Points == null || Points.PointPositions == null) 
        {
            return;
        }

        if (ShowPointsVisual)
        {
            for (int i = 0; i < Points.PointPositions.Count; i++)
            {
                if (Points.PointPositions[i].PositionalHelper != PointData.PointPositionalHelper.Normal)
                {
                    Handles.color = Color.yellow;
                }
                else
                {
                    Handles.color = Color.red;
                }

                Handles.SphereHandleCap(i, Points.PointPositions[i].WorldPosition, Quaternion.identity, PointDisplaySize, EventType.Repaint);
            }
        }

        if (ShowSpacingVisual)
        {
            for (int i = 0; i < Points.PointPositions.Count; i++)
            {
                if (Points.PointPositions[i].PositionalHelper != PointData.PointPositionalHelper.Normal)
                {
                    Handles.color = Color.yellow;
                    Handles.DrawWireCube(Points.PointPositions[i].WorldPosition, Points.GridSpacing);
                }
                else
                {
                    Handles.color = Color.red;
                }
            }
        }

        if (LiveUpdatePoints)
        {
            GeneratePoints();
        }

        SceneView.RepaintAll();
    }

    public void GeneratePoints()
    {
        PointsGenerator.CreatePoints(Points.GridDimension, Points.GridSpacing, PointsGenerator.transform.position);
        Points = PointsGenerator.Points;
    }
}
