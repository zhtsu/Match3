using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class M3_ModManager : M3_IManager
{
    public string ManagerName
    {
        get { return "ModManager"; }
    }

    private List<M3_ModData> _ModDataList = new List<M3_ModData>();

    public void Initialize()
    {
        foreach (string ModDir in Directory.GetDirectories(M3_PathManager.ModsPath))
        {
            string ModFilePath = M3_PathManager.GenerateModFilePath(Path.Combine(ModDir, "mod.fbmod"));
            if (!File.Exists(ModFilePath))
            {
                Debug.LogError(ModFilePath + " no exist!");
                continue;
            }

            try
            {
                string ModFileContent = File.ReadAllText(ModFilePath, System.Text.Encoding.UTF8);

                if (M3_Data.Deserialize<M3_ModData>(ModFileContent, out M3_ModData ModData))
                {
                    _ModDataList.Add(ModData);
                }

                // Load data by sending event
                foreach (string LocaleFile in ModData.LocaleList)
                {
                    M3_Event_ReadLocaleFile RLFE = new M3_Event_ReadLocaleFile(ModData.Id, M3_PathManager.GenerateModFilePath(LocaleFile));
                    M3_EventManager.SendEvent<M3_Event_ReadLocaleFile>(RLFE);
                }

                foreach (string TileFile in ModData.TileList)
                {
                    M3_Event_ReadTileFile RTFE = new M3_Event_ReadTileFile(ModData.Id, M3_PathManager.GenerateModFilePath(TileFile));
                    M3_EventManager.SendEvent<M3_Event_ReadTileFile>(RTFE);
                }

                foreach (string UnitFile in ModData.UnitList)
                {
                    M3_Event_ReadUnitFile RUFE = new M3_Event_ReadUnitFile(ModData.Id, M3_PathManager.GenerateModFilePath(UnitFile));
                    M3_EventManager.SendEvent<M3_Event_ReadUnitFile>(RUFE);
                }
            }
            catch (System.Exception Err)
            {
                Debug.LogError($"Fail to read {ModFilePath}\n Error: {Err.Message}");
            }
        }
    }

    public void Destroy()
    {

    }

    public List<M3_ModData> GetModDataList()
    {
        return _ModDataList;
    }

    M3_ModData GetModData(string ModId)
    {
        foreach (M3_ModData ModData in _ModDataList)
        {
            if (ModData.Id == ModId)
            {
                return ModData;
            }
        }

        return null;
    }
}
