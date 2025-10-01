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
    private float _TileSize;
    private M3_FillMode _FillMode = M3_FillMode.None;
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
        _TileSize = EditorGUILayout.FloatField("TileSize", _TileSize);
        _FillMode = (M3_FillMode)EditorGUILayout.EnumPopup("Fill Mode", _FillMode);
        _TestTile = (GameObject)EditorGUILayout.ObjectField("Test Tile", _TestTile, typeof(GameObject), false);
        
        GUILayout.Space(10);
        if (GUILayout.Button("Generate Grid"))
        {
            _Target.Initialize(_Row, _Column, _TileSize);
            _Target.GenerateGrid();
            _Target.AddCell(
                (M3_Gem)Instantiate(_TestTile).GetComponent<M3_IGridCell>(), 0, 0, _FillMode);
        }

        if (GUILayout.Button("Clear Grid"))
        {
            _Target.ClearGrid();
        }
    }
}
#endif