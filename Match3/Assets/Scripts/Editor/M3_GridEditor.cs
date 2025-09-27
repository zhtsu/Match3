#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(M3_Grid))]
public class M3_GridEditor : Editor
{
    private M3_Grid _Target;

    private int _Row;
    private int _Column;
    private int _TileSize;
    private GameObject _TestTile;

    private void OnEnable()
    {
        _Target = (M3_Grid)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(10);
        GUILayout.Label("M3_GridEditor", EditorStyles.boldLabel);

        GUILayout.Space(10);
        _Row = EditorGUILayout.IntField("Row", _Row);
        _Column = EditorGUILayout.IntField("Column", _Column);
        _TileSize = EditorGUILayout.IntField("TileSize", _TileSize);
        _TestTile = (GameObject)EditorGUILayout.ObjectField("Test Tile", _TestTile, typeof(GameObject), false);

        GUILayout.Space(10);
        if (GUILayout.Button("Generate Grid"))
        {
            _Target.Initialize(_Row, _Column, _TileSize);
            _Target.GenerateGrid();
        }

        if (GUILayout.Button("Clear Grid"))
        {
            _Target.ClearGrid();
        }
    }
}
#endif