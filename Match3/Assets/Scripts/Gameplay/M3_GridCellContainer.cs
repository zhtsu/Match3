using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public enum M3_FillMode
{
    None = 0,
    ScaleFill,
    AspectFill,
    AspectFit
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

            Vector3 BakedLocalScale = CellTransform.localScale;
            CellTransform.localScale = Vector3.one;

            Bounds bounds = CellTransform.GetComponent<Renderer>().bounds;
            CellTransform.localScale = CalFillScale(FillMode, bounds, BakedLocalScale);
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

    private Vector3 CalFillScale(M3_FillMode FillMode, Bounds InBounds, Vector3 BakedScale)
    {
        float OutputScaleX;
        float OutputScaleY;

        switch (FillMode)
        {
            case M3_FillMode.ScaleFill:
                {
                    OutputScaleX = _Width / InBounds.size.x;
                    OutputScaleY = _Height / InBounds.size.y;

                    break;
                }
            case M3_FillMode.AspectFill:
                {
                    float ScaleX = (_Width / (InBounds.size.x * BakedScale.x));
                    float ScaleY = (_Height / (InBounds.size.y * BakedScale.y));

                    OutputScaleX = Mathf.Max(ScaleX, ScaleY) * BakedScale.x;
                    OutputScaleY = Mathf.Max(ScaleX, ScaleY) * BakedScale.y;

                    break;
                }
            case M3_FillMode.AspectFit:
                {
                    float ScaleX = (_Width / InBounds.size.x);
                    float ScaleY = (_Height / InBounds.size.y);

                    OutputScaleX = Mathf.Min(ScaleX, ScaleY) * BakedScale.x;
                    OutputScaleY = Mathf.Min(ScaleX, ScaleY) * BakedScale.y;

                    break;
                }
            case M3_FillMode.None:
            default:
                {
                    OutputScaleX = BakedScale.x;
                    OutputScaleY = BakedScale.y;

                    break;
                }
        }

        return new Vector3(OutputScaleX, OutputScaleY, 1);
    }
}
