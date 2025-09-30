using System.Collections;
using System.Collections.Generic;
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

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Create New PointsData"))
        {
            PointsGenerator.CreatePoints(Points.X_Dimension, Points.Y_Dimension, Points.X_Dimension, PointsGenerator.transform.position);
            Points = PointsGenerator.Points;

            SceneView.RepaintAll();
        }
    }

    private void OnEnable()
    {
        PointsGenerator = (PointGenerator)target;

        if (PointsGenerator.Points == null)
        {
            PointsGenerator.CreatePoints(Points.X_Dimension, Points.Y_Dimension, Points.X_Dimension, PointsGenerator.transform.position);
            Points = PointsGenerator.Points;
        }

        Points = PointsGenerator.Points;
    }

    private void OnSceneGUI()
    {
        Draw();
    }

    void Draw()
    {
        Handles.color = Color.red;

        for (int i = 0; i < Points.PointPositions.Count; i++)
        {
            Handles.DrawWireCube(Points.PointPositions[i], new Vector3(1f, 1f, 1f));
        }
    }
}
