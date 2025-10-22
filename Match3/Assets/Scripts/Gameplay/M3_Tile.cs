using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M3_Tile : MonoBehaviour, M3_IGridCell
{
    public Vector2Int CellCoords { get; set; }
    public M3_GridCellContainer ParentContainer { get; set; }

    public void UnitEntered(M3_Unit Unit)
    {

    }

    public void UnitExited(M3_Unit Unit)
    {

    }

    public void Initialize(M3_TileData Data)
    {

    }
}
