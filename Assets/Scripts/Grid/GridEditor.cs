using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(GridManager))]
public class GridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GridManager gridManager = (GridManager)target;
        if (GUILayout.Button("Create Grid"))
        {
            gridManager.CreateGrid();
            Debug.Log("Atakan");
        }

        if (GUILayout.Button("Clear Grid"))
        {
            gridManager.ClearGrid();
        }
    }
}