using PrimeTween;
using System.Collections;
using UnityEngine;

public enum M3_FillMode
{
    None = 0,
    ScaleFill,
    AspectFill,
    AspectFit
}

public class M3_GridCellContainer : MonoBehaviour
{
    [SerializeField]
    GameObject _TileArea;

    [SerializeField]
    GameObject _CellArea;

    private float _Width = 0;
    private float _Height = 0;
    private M3_IGridCell _GemCell;
    private M3_IGridCell _GemCellWaitToDestroy;
    private M3_IGridCell _TileCell;
    private Color _SavedTileColor;
    public M3_Grid ParentGrid { get; set; }
    public Vector2Int GridCoords { get; set; }

    public void Initialize(float Width, float Height)
    {
        _Width = Width;
        _Height = Height;
    }

    public void AddTile(M3_IGridCell GridCell, M3_FillMode FillMode = M3_FillMode.None)
    {
        _TileCell = GridCell;

        M3_Tile Tile = GridCell as M3_Tile;
        if (Tile != null)
        {
            int sum = GridCoords.x + GridCoords.y;
            if (sum % 2 == 0)
                Tile.GetComponent<SpriteRenderer>().color = M3_CommonHelper.GetCommonColor(M3_ColorType.TileWhite);
            else
                Tile.GetComponent<SpriteRenderer>().color = M3_CommonHelper.GetCommonColor(M3_ColorType.TileBlack);

            _SavedTileColor = Tile.GetComponent<SpriteRenderer>().color;
        }

        if (_TileCell != null)
        {
            _TileCell.ParentContainer = this;

            Transform TileTransform = ((MonoBehaviour)_TileCell).transform;
            TileTransform.SetParent(_TileArea.transform);

            TileTransform.localPosition = Vector3.zero;
            TileTransform.localRotation = Quaternion.identity;

            Vector3 BakedLocalScale = TileTransform.localScale;
            TileTransform.localScale = Vector3.one;

            Bounds bounds = TileTransform.GetComponent<Renderer>().bounds;
            TileTransform.localScale = CalFillScale(FillMode, bounds, BakedLocalScale, 0.0f);
        }
    }

    public void AddCell(M3_IGridCell GridCell, M3_FillMode FillMode = M3_FillMode.None, bool ScaleFollowParent = true)
    {
        _GemCell = GridCell;

        if (_GemCell != null)
        {
            _GemCell.ParentContainer = this;

            Transform CellTransform = ((MonoBehaviour)_GemCell).transform;
            CellTransform.SetParent(_CellArea.transform);

            CellTransform.localPosition = Vector3.zero;
            CellTransform.localRotation = Quaternion.identity;

            Vector3 BakedLocalScale = ScaleFollowParent ? CellTransform.localScale : Vector3.one;
            CellTransform.localScale = Vector3.one;

            Bounds bounds = CellTransform.GetComponent<Renderer>().bounds;
            CellTransform.localScale = CalFillScale(FillMode, bounds, BakedLocalScale, 0.1f);

            M3_Gem Gem = _GemCell as M3_Gem;
            if (Gem != null)
                Gem.SetSavedScale(CellTransform.localScale);
        }
    }

    public void SetGemCell(M3_IGridCell InGridCell)
    {
        _GemCell = InGridCell;
    }

    public M3_IGridCell GetTileCell()
    {
        return _TileCell;
    }

    public M3_IGridCell GetGemCell()
    {
        return _GemCell;
    }

    public void ClearGemCell()
    {
        _GemCell = null;
    }

    public void RemoveGemCell()
    {
        _GemCellWaitToDestroy = _GemCell;
        _GemCell = null;

        if (_GemCellWaitToDestroy != null)
        {
            M3_Gem Gem = ((M3_Gem)_GemCellWaitToDestroy);
            Gem.transform.parent = null;
            Gem.DestroySelf();
        }
    }

    private void EndRemoveGemCell()
    {
        SetTileColor(_SavedTileColor);
    }

    private Vector3 CalFillScale(M3_FillMode FillMode, Bounds InBounds, Vector3 BakedScale, float Padding = 0.0f)
    {
        if (InBounds.size.x == 0 || InBounds.size.y == 0 || InBounds.size.z == 0)
            return Vector3.one;

        float Width = _Width - Padding * 2;
        float Height = _Height - Padding * 2;

        float OutputScaleX;
        float OutputScaleY;

        switch (FillMode)
        {
            case M3_FillMode.ScaleFill:
                {
                    OutputScaleX = Width / InBounds.size.x;
                    OutputScaleY = Height / InBounds.size.y;

                    break;
                }
            case M3_FillMode.AspectFill:
                {
                    float ScaleX = (Width / (InBounds.size.x * BakedScale.x));
                    float ScaleY = (Height / (InBounds.size.y * BakedScale.y));

                    OutputScaleX = Mathf.Max(ScaleX, ScaleY) * BakedScale.x;
                    OutputScaleY = Mathf.Max(ScaleX, ScaleY) * BakedScale.y;

                    break;
                }
            case M3_FillMode.AspectFit:
                {
                    float ScaleX = (Width / InBounds.size.x);
                    float ScaleY = (Height / InBounds.size.y);

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

    public void SetTileColor(Color InColor)
    {
        if (_TileCell != null)
        {
            M3_Tile Tile = _TileCell as M3_Tile;
            if (Tile != null)
            {
                Tile.GetComponent<SpriteRenderer>().color = InColor;
            }
        }
    }

    public void ResetTileColor()
    {
        if (_TileCell != null)
        {
            M3_Tile Tile = _TileCell as M3_Tile;
            if (Tile != null)
            {
                Tile.GetComponent<SpriteRenderer>().color = _SavedTileColor;
            }
        }
    }

    public bool IsEmpty()
    {
        return _GemCell == null;
    }
}
