using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class M3_DataManager : M3_Manager
{
    public override string ManagerName
    {
        get { return "Data Manager"; }
    }

    private List<M3_ModData> _ModDataList = new List<M3_ModData>();
    private List<M3_TileData> _TileDataList = new List<M3_TileData>();
    private List<M3_UnitData> _UnitDataList = new List<M3_UnitData>();
    private Dictionary<string, Dictionary<string, Dictionary<string, string>>> _LocaleStringDict
        = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();

    public override void Initialize()
    {
        M3_EventManager.Subscribe<M3_Event_ReadTileFile>(LoadTileData);
        M3_EventManager.Subscribe<M3_Event_ReadUnitFile>(LoadUnitData);
        M3_EventManager.Subscribe<M3_Event_ReadLocaleFile>(LoadLocaleData);

        LoadModData();
    }

    public override void Destroy()
    {
        M3_EventManager.Unsubscribe<M3_Event_ReadTileFile>(LoadTileData);
        M3_EventManager.Unsubscribe<M3_Event_ReadUnitFile>(LoadUnitData);
        M3_EventManager.Unsubscribe<M3_Event_ReadLocaleFile>(LoadLocaleData);

        _LocaleStringDict.Clear();
    }

    //
    // Unit Data
    //

    private bool GetUnitData(string ModId, string TileId, out M3_UnitData OutUnitData)
    {
        foreach (M3_UnitData UnitData in _UnitDataList)
        {
            if (UnitData.BelongingModId == ModId && UnitData.Id == TileId)
            {
                OutUnitData = UnitData;
                return true;
            }
        }

        OutUnitData = default;
        return false;
    }

    private bool HasUnitData(string ModId, string TileId)
    {
        return GetUnitData(ModId, TileId, out M3_UnitData TempUnitData);
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

            if (M3_DataHelper.Deserialize<M3_UnitData>(UnitFileContent, out M3_UnitData UnitData))
            {
                if (HasUnitData(UnitData.BelongingModId, UnitData.Id))
                {
                    Debug.LogWarning("Repeated unit id");
                    return;
                }

                UnitData.BelongingModId = Event.BelongingModId;

                _UnitDataList.Add(UnitData);
            }
        }
        catch (System.Exception Err)
        {
            Debug.LogError($"Fail to read {Event.UnitFilePath}\n Error: {Err.Message}");
        }
    }

    //
    // Tile Data
    //

    public M3_TileData[] GetTileDataList(string ModId)
    {
        return _TileDataList.FindAll(TileData => TileData.BelongingModId == ModId).ToArray();
    }

    private bool GetTileData(string ModId, string TileId, out M3_TileData OutTileData)
    {
        foreach (M3_TileData TileData in _TileDataList)
        {
            if (TileData.BelongingModId == ModId && TileData.Id == TileId)
            {
                OutTileData = TileData;
                return true;
            }
        }

        OutTileData = default;
        return false;
    }

    private bool HasTileData(string ModId, string TileId)
    {
        return GetTileData(ModId, TileId, out M3_TileData TempTileData);
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

            if (M3_DataHelper.Deserialize<M3_TileData>(TileFileContent, out M3_TileData TileData))
            {
                if (HasTileData(TileData.BelongingModId, TileData.Id))
                {
                    Debug.LogWarning("Repeated tile id");
                    return;
                }

                TileData.BelongingModId = Event.BelongingModId;

                _TileDataList.Add(TileData);
            }
        }
        catch (System.Exception Err)
        {
            Debug.LogError($"Fail to read {Event.TileFilePath}\n Error: {Err.Message}");
        }
    }

    //
    // Mod Data
    //

    private void LoadModData()
    {
        foreach (string ModSubDir in Directory.GetDirectories(M3_PathHelper.GetModsPath()))
        {
            string ModFilePath = M3_PathHelper.GetModSubfilePath(Path.Combine(ModSubDir, "mod.fbmod"));
            if (!File.Exists(ModFilePath))
            {
                Debug.LogError(ModFilePath + " no exist!");
                continue;
            }

            try
            {
                string ModFileContent = File.ReadAllText(ModFilePath, System.Text.Encoding.UTF8);

                if (M3_DataHelper.Deserialize<M3_ModData>(ModFileContent, out M3_ModData ModData))
                {
                    _ModDataList.Add(ModData);
                }

                // Load data by sending event
                foreach (string LocaleFile in ModData.LocaleList)
                {
                    M3_Event_ReadLocaleFile RLFE = new M3_Event_ReadLocaleFile(ModData.Id, M3_PathHelper.GetModSubfilePath(LocaleFile));
                    M3_EventManager.SendEvent<M3_Event_ReadLocaleFile>(RLFE);
                }

                foreach (string TileFile in ModData.TileList)
                {
                    M3_Event_ReadTileFile RTFE = new M3_Event_ReadTileFile(ModData.Id, M3_PathHelper.GetModSubfilePath(TileFile));
                    M3_EventManager.SendEvent<M3_Event_ReadTileFile>(RTFE);
                }

                foreach (string UnitFile in ModData.UnitList)
                {
                    M3_Event_ReadUnitFile RUFE = new M3_Event_ReadUnitFile(ModData.Id, M3_PathHelper.GetModSubfilePath(UnitFile));
                    M3_EventManager.SendEvent<M3_Event_ReadUnitFile>(RUFE);
                }
            }
            catch (System.Exception Err)
            {
                Debug.LogError($"Fail to read {ModFilePath}\n Error: {Err.Message}");
            }
        }
    }

    public List<M3_ModData> GetModDataList()
    {
        return _ModDataList;
    }

    bool GetModData(string ModId, out M3_ModData OutModData)
    {
        foreach (M3_ModData ModData in _ModDataList)
        {
            if (ModData.Id == ModId)
            {
                OutModData = ModData;
                return true;
            }
        }

        OutModData = default;
        return false;
    }

    //
    // Locale Data
    //

    public string GetLocalString(string Namespace, string LanguageCode, string StringId)
    {
        if (_LocaleStringDict.TryGetValue(Namespace, out Dictionary<string, Dictionary<string, string>> LanguageDict))
        {
            if (LanguageDict.TryGetValue(LanguageCode, out Dictionary<string, string> StringDict))
            {
                if (StringDict.TryGetValue(StringId, out string LocaleString))
                {
                    return LocaleString;
                }
                else
                {
                    return $"Error string id: {StringId}";
                }
            }
            else
            {
                return $"Error language code: {LanguageCode}";
            }
        }
        else
        {
            return $"Error namespace: {Namespace}";
        }
    }

    private void LoadLocaleData(M3_Event_ReadLocaleFile Event)
    {
        if (_LocaleStringDict.ContainsKey(Event.Namespace) == false)
        {
            _LocaleStringDict[Event.Namespace] = new Dictionary<string, Dictionary<string, string>>();
        }

        if (!File.Exists(Event.LocaleFilePath))
        {
            Debug.LogError(Event.LocaleFilePath + " no exist!");
            return;
        }

        try
        {
            string LocaleFileContent = File.ReadAllText(Event.LocaleFilePath, System.Text.Encoding.UTF8);
            JsonData LocaleJsonData = JsonMapper.ToObject(LocaleFileContent);

            foreach (string LanguageCode in LocaleJsonData.Keys)
            {
                if (_LocaleStringDict[Event.Namespace].ContainsKey(LanguageCode) == false)
                {
                    _LocaleStringDict[Event.Namespace][LanguageCode] = new Dictionary<string, string>();
                }

                JsonData StringJsonData = LocaleJsonData[LanguageCode];

                foreach (string StringId in StringJsonData.Keys)
                {
                    string LocaleString = (string)StringJsonData[StringId];
                    _LocaleStringDict[Event.Namespace][LanguageCode].Add(StringId, LocaleString);
                }
            }
        }
        catch (System.Exception Err)
        {
            Debug.LogError($"Fail to read {Event.LocaleFilePath}\n Error: {Err.Message}");
        }
    }
}
