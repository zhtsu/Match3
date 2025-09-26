using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class M3_UnitManager : M3_Manager
{
    public override string ManagerName
    {
        get { return "UnitManager"; }
    }

    private List<M3_UnitData> _UnitDataList = new List<M3_UnitData>();

    public override void Initialize()
    {
        M3_EventManager.Subscribe<M3_Event_ReadUnitFile>(LoadUnitData);
    }

    public override void Destroy()
    {
        M3_EventManager.Unsubscribe<M3_Event_ReadUnitFile>(LoadUnitData);
    }

    private M3_UnitData GetUnitData(string ModId, string TileId)
    {
        return _UnitDataList.Find(
            UnitData => UnitData.BelongingModId == ModId && UnitData.Id == TileId);
    }

    private bool HasUnitData(string ModId, string TileId)
    {
        return GetUnitData(ModId, TileId) != null;
    }

    private void LoadUnitData(M3_Event_ReadUnitFile Event)
    {
        if (!File.Exists(Event.UnitFilePath))
        {
            Debug.LogError(Event.UnitFilePath + " no exist!");
            return;
        }

        try
        {
            string UnitFileContent = File.ReadAllText(Event.UnitFilePath, System.Text.Encoding.UTF8);

            if (M3_Data.Deserialize<M3_UnitData>(UnitFileContent, out M3_UnitData UnitData))
            {
                if (HasUnitData(UnitData.BelongingModId, UnitData.Id))
                {
                    Debug.LogWarning("Repeated unit id");
                    return;
                }

                UnitData.Initialize(Event.BelongingModId);

                _UnitDataList.Add(UnitData);
            }
        }
        catch (System.Exception Err)
        {
            Debug.LogError($"Fail to read {Event.UnitFilePath}\n Error: {Err.Message}");
        }
    }
}
