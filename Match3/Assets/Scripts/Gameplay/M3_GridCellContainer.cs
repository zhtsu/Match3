using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum M3_FillMode
{
    None = 0,
    Fill,
    Fit
}

public class M3_GridCellContainer : MonoBehaviour
{
    private float _Width = 0;
    private float _Height = 0;
    private M3_IGridCell _GridCell;

    public void Initialize(float Width, float Height)
    {
        _Width = Width;
        _Height = Height;
    }

    public void AddCell(M3_IGridCell GridCell, M3_FillMode FillMode = M3_FillMode.None)
    {
        _GridCell = GridCell;

        if (_GridCell != null)
        {
            Transform CellTransform = ((MonoBehaviour)_GridCell).transform;
            CellTransform.SetParent(transform);
            CellTransform.localPosition = Vector3.zero;
            CellTransform.localRotation = Quaternion.identity;
            Bounds bounds = CellTransform.GetComponent<Renderer>().bounds;
            if (FillMode == M3_FillMode.Fill)
            {
                float ScaleX = bounds.size.x / _Width;
                float ScaleY = bounds.size.y / _Height;
                CellTransform.localScale = new Vector3(ScaleX, ScaleY, 1);
            }
            else if (FillMode == M3_FillMode.Fit)
            {
                float ScaleX = bounds.size.x / _Width;
                float ScaleY = bounds.size.y / _Height;
                float Scale = Mathf.Min(ScaleX, ScaleY);
                CellTransform.localScale = new Vector3(Scale, Scale, 1);
            }
        }
    }

    public M3_IGridCell GetCell()
    {
        return _GridCell;
    }

    public void ClearCell()
    {
        _GridCell = null;
    }
}
