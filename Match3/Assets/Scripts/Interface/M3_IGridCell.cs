using UnityEngine;

public interface M3_IGridCell
{
    public Vector2Int CellCoords { get; set; }
    public M3_Grid ParentGrid { get; set; }
}
