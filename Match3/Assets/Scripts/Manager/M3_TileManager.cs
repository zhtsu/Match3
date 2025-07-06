using UnityEngine;
using System.Collections.Generic;
using System.IO;
using XLua;

public class M3_TileManager : M3_IManager
{
    public string ManagerName
    {
        get { return "TileManager"; }
    }

    private List<M3_TileData> _TileDataList = new List<M3_TileData>();

    public void Initialize()
    {
        M3_EventManager.Subscribe<M3_Event_ReadTileFile>(LoadTileData);
    }

    public void Destroy()
    {
        M3_EventManager.Unsubscribe<M3_Event_ReadTileFile>(LoadTileData);
    }

    public M3_TileData[] GetTileDataList(string ModId)
    {
        return _TileDataList.FindAll(TileData => TileData.BelongingModId == ModId).ToArray();
    }

    private M3_TileData GetTileData(string ModId, string TileId)
    {
        return _TileDataList.Find(
            TileData => TileData.BelongingModId == ModId && TileData.Id == TileId);
    }

    private bool HasTileData(string ModId, string TileId)
    {
        return GetTileData(ModId, TileId) != null;
    }

    private void LoadTileData(M3_Event_ReadTileFile Event)
    {
        if (!File.Exists(Event.TileFilePath))
        {
            Debug.LogError(Event.TileFilePath + " no exist!");
            return;
        }

        try
        {
            string TileFileContent = File.ReadAllText(Event.TileFilePath, System.Text.Encoding.UTF8);

            if (M3_Data.Deserialize<M3_TileData>(TileFileContent, out M3_TileData TileData))
            {
                if (HasTileData(TileData.BelongingModId, TileData.Id))
                {
                    Debug.LogWarning("Repeated tile id");
                    return;
                }

                TileData.Initialize(Event.BelongingModId);

                _TileDataList.Add(TileData);
            }
        }
        catch (System.Exception Err)
        {
            Debug.LogError($"Fail to read {Event.TileFilePath}\n Error: {Err.Message}");
        }
    }
}
