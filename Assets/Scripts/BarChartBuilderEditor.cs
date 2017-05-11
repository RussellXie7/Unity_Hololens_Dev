using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(BarChartBuilder))]
public class BarChartBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        BarChartBuilder bcb = (BarChartBuilder)target;
        if (GUILayout.Button("Build Chart"))
        {
            bcb.ClearAllbars();
            bcb.BuildBarChart();
        }

        if (GUILayout.Button("Clear All"))
        {
            bcb.ClearAllbars();
        }
    }
}