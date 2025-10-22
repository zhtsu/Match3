using LitJson;
using System.IO;
using UnityEngine;

public class M3_ModAPI
{
    public void StartMatch3()
    {
        M3_CommonHelper.CloseAllUI();

        M3_Match3Battle Battle = new M3_Match3Battle();
        M3_LevelData LevelData = new M3_LevelData();
        LevelData.Row = 8;
        LevelData.Column = 8;
        LevelData.TileSize = 1.2f;
        M3_GameController.Instance.SetCurrentM3Battle(Battle);

        Battle.StartBattle(LevelData);
    }
}
