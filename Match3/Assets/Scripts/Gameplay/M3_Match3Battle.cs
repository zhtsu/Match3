using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M3_Match3Battle
{
    private M3_Grid _Grid;

    public void StartBattle(M3_LevelData LevelData)
    {
        GameObject GridPrefab = M3_CommonHelper.GetPrefab("Grid");
        if (GridPrefab != null)
        {
            GameObject GridObject = GameObject.Instantiate(GridPrefab);
            _Grid = GridObject.GetComponent<M3_Grid>();
        }

        _Grid.Initialize(LevelData.Row, LevelData.Column, LevelData.TileSize);
        _Grid.GenerateGrid();

        for (int i = 0; i < _Grid.Row; i++)
        {
            for (int j = 0; j < _Grid.Column; j++)
            {
                M3_UnitData GemData = M3_CommonHelper.GetRandomGemData();
                M3_IGridCell NewCell = M3_CommonHelper.SpawnGem(GemData.BelongingModId, GemData.Id);
                _Grid.AddCell(NewCell, i, j, M3_FillMode.AspectFit, true);
            }
        }
    }
}
