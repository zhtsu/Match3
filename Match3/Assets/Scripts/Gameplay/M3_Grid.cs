using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class M3_Grid : MonoBehaviour
{
    [SerializeField]
    private GameObject _GridCellContainerPrefab;

    private int _Row = 0;
    public int Row { get { return _Row; } }

    private int _Column = 0;
    public int Column { get { return _Column; } }

    private float _TileSize = 0;
    public float TileSize { get { return _TileSize; } }

    private M3_GridCellContainer[,] _GridArray;

    public void Initialize(int Row, int Column, float TileSize)
    {
        _Row = Row;
        _Column = Column;
        _TileSize = TileSize;
        _GridArray = new M3_GridCellContainer[_Row, _Column];
    }

    public void AddCell(M3_IGridCell GridCell, int X, int Y, M3_FillMode FillMode = M3_FillMode.None)
    {
        if (X < _Row && Y < _Column)
        {
            if (GridCell != null)
            {
                GridCell.CellCoords = new Vector2Int(X, Y);
                GridCell.ParentGrid = this;
            }

            M3_GridCellContainer GridContainer = _GridArray[X, Y];
            if (GridContainer != null)
                GridContainer.AddCell(GridCell, FillMode);
        }
    }

    public M3_IGridCell GetCell(int X, int Y)
    {
        if (X < _Row && Y < _Column)
        {
            M3_GridCellContainer GridContainer = _GridArray[X, Y];
            if (GridContainer != null)
                return GridContainer.GetCell();
        }

        return null;
    }

    public void GenerateGrid()
    {
        ClearGrid();

        Vector3 Offset = new Vector3(-_Row * _TileSize * 0.5f, -_Column * _TileSize * 0.5f, 0);
        Offset += new Vector3(_TileSize * 0.5f, _TileSize * 0.5f, 0);
        for (int i = 0; i < _Row; i++)
        {
            for (int j = 0; j < _Column; j++)
            {
                Vector3 Position = new Vector3(i, j) * _TileSize + Offset;
                GameObject CellContainer = Instantiate(_GridCellContainerPrefab, Position, Quaternion.identity);
                CellContainer.transform.SetParent(this.transform);
                M3_GridCellContainer GCC = CellContainer.GetComponent<M3_GridCellContainer>();
                GCC.Initialize(_TileSize, _TileSize);
                _GridArray[i, j] = GCC;
            }
        }
    }

    public void ClearGrid()
    {
        while (transform.childCount > 0)
        {
            Transform Child = transform.GetChild(0);
#if UNITY_EDITOR
            DestroyImmediate(Child.gameObject);
#else
            Destroy(Child.gameObject);
#endif
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 Offset = new Vector3(-_Row * _TileSize * 0.5f, -_Column * _TileSize * 0.5f, 0);
        for (int i = 0; i <= _Row; i++)
        {
            Vector3 StartPos = transform.position + new Vector3(i * _TileSize, 0, 0) + Offset;
            Vector3 EndPos = transform.position + new Vector3(i * _TileSize, _Column * _TileSize, 0) + Offset;
            Gizmos.DrawLine(StartPos, EndPos);
        }
        for (int j = 0; j <= _Column; j++)
        {
            Vector3 StartPos = transform.position + new Vector3(0, j * _TileSize, 0) + Offset;
            Vector3 EndPos = transform.position + new Vector3(_Row * _TileSize, j * _TileSize, 0) + Offset;
            Gizmos.DrawLine(StartPos, EndPos);
        }
    }
}
