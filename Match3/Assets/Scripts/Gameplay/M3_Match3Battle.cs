using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M3_Match3Battle
{
    private M3_Grid _Grid;
    private M3_AIController _AIController;

    public IEnumerator StartBattle(M3_LevelData LevelData)
    {
        M3_EventBus.Subscribe<M3_Event_HPUIUpdated>(OnTurnEnded);

        GameObject GridPrefab = M3_CommonHelper.GetPrefab("Grid");
        if (GridPrefab != null)
        {
            GameObject GridObject = GameObject.Instantiate(GridPrefab);
            _Grid = GridObject.GetComponent<M3_Grid>();
        }

        _Grid.Initialize(LevelData.Row, LevelData.Column, LevelData.TileSize);
        _Grid.GenerateGrid();

        yield return _Grid.FillEmptyCellsCoroutine(0.6f);
        yield return _Grid.ProcessMatch3Coroutine();

        if (M3_GameController.Instance.CurrentBattleInputController == M3_ControllerType.None)
        {
            M3_GameController.Instance.SetBattleInputController(M3_ControllerType.Player);
            M3_EventBus.SendEvent(new M3_Event_BattleControllerChanged(M3_ControllerType.Player));
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

    private void OnTurnEnded(M3_Event_HPUIUpdated Event)
    {
        SwitchController();
    }
}
