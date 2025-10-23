using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M3_Match3Battle
{
    private M3_Grid _Grid;
    private M3_AIController _AIController;

    public void StartBattle(M3_LevelData LevelData)
    {
        M3_EventBus.Subscribe<M3_Event_GemSwapped>(OnGemSwapped);

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

        _AIController = new M3_AIController(_Grid);
    }

    public void SwitchController()
    {
        if (M3_GameController.Instance.CurrentBattleInputController == M3_ControllerType.Player)
        {
            M3_GameController.Instance.SetBattleInputController(M3_ControllerType.AI);
            M3_EventBus.SendEvent(new M3_Event_BattleControllerChanged(M3_ControllerType.AI));
        }
        else
        {
            M3_GameController.Instance.SetBattleInputController(M3_ControllerType.Player);
            M3_EventBus.SendEvent(new M3_Event_BattleControllerChanged(M3_ControllerType.Player));
        }
    }

    private void OnGemSwapped(M3_Event_GemSwapped Event)
    {
        SwitchController();
    }
}
