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
    private Dictionary<string, Texture2D> TextureDict = new Dictionary<string, Texture2D>();

    public override void Initialize()
    {
        LoadModData();
        LoadTextureData();
    }

    public override void Destroy()
    {
        _LocaleStringDict.Clear();
    }

    //
    // Unit Data
    //

    public bool GetUnitData(string ModId, string TileId, out M3_UnitData OutUnitData)
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

    public bool HasUnitData(string ModId, string TileId)
    {
        return GetUnitData(ModId, TileId, out M3_UnitData TempUnitData);
    }

    private void LoadUnitData(string InModId, string InUnitFilePath)
    {
        if (!File.Exists(InUnitFilePath))
        {
            Debug.LogError(InUnitFilePath + " no exist!");
            return;
        }

        try
        {
            string UnitFileContent = File.ReadAllText(InUnitFilePath, System.Text.Encoding.UTF8);

            if (M3_DataHelper.Deserialize<M3_UnitData>(UnitFileContent, out M3_UnitData UnitData))
            {
                if (HasUnitData(UnitData.BelongingModId, UnitData.Id))
                {
                    Debug.LogWarning("Repeated unit id");
                    return;
                }

                UnitData.BelongingModId = InModId;

                _UnitDataList.Add(UnitData);
            }
        }
        catch (System.Exception Err)
        {
            Debug.LogError($"Fail to read {InUnitFilePath}\n Error: {Err.Message}");
        }
    }

    //
    // Tile Data
    //

    public M3_TileData[] GetTileDataList(string ModId)
    {
        return _TileDataList.FindAll(TileData => TileData.BelongingModId == ModId).ToArray();
    }

    public bool GetTileData(string ModId, string TileId, out M3_TileData OutTileData)
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

    public bool HasTileData(string ModId, string TileId)
    {
        return GetTileData(ModId, TileId, out M3_TileData TempTileData);
    }

    private void LoadTileData(string InModId, string InTileFilePath)
    {
        if (!File.Exists(InTileFilePath))
        {
            Debug.LogError(InTileFilePath + " no exist!");
            return;
        }

        try
        {
            string TileFileContent = File.ReadAllText(InTileFilePath, System.Text.Encoding.UTF8);

            if (M3_DataHelper.Deserialize<M3_TileData>(TileFileContent, out M3_TileData TileData))
            {
                if (HasTileData(TileData.BelongingModId, TileData.Id))
                {
                    Debug.LogWarning("Repeated tile id");
                    return;
                }

                TileData.BelongingModId = InModId;

                _TileDataList.Add(TileData);
            }
        }
        catch (System.Exception Err)
        {
            Debug.LogError($"Fail to read {InTileFilePath}\n Error: {Err.Message}");
        }
    }

    //
    // Mod Data
    //

    private void LoadModData()
    {
        foreach (string ModSubDir in Directory.GetDirectories(M3_PathHelper.GetModsPath()))
        {
            string ModFilePath = M3_PathHelper.GetModSubfilePath(Path.Combine(ModSubDir, "mod.json"));
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

                foreach (string LocaleFile in ModData.LocaleList)
                {
                    LoadLocaleData(ModData.Id, M3_PathHelper.GetModSubfilePath(LocaleFile));
                }

                foreach (string TileFile in ModData.TileList)
                {
                    LoadTileData(ModData.Id, M3_PathHelper.GetModSubfilePath(TileFile));
                }

                foreach (string UnitFile in ModData.UnitList)
                {
                    LoadUnitData(ModData.Id, M3_PathHelper.GetModSubfilePath(UnitFile));
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

    public bool GetModData(string ModId, out M3_ModData OutModData)
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

    private void LoadLocaleData(string InNamespace, string InLocaleFilePath)
    {
        if (_LocaleStringDict.ContainsKey(InNamespace) == false)
        {
            _LocaleStringDict[InNamespace] = new Dictionary<string, Dictionary<string, string>>();
        }

        if (!File.Exists(InLocaleFilePath))
        {
            Debug.LogError(InLocaleFilePath + " no exist!");
            return;
        }

        try
        {
            string LocaleFileContent = File.ReadAllText(InLocaleFilePath, System.Text.Encoding.UTF8);
            JsonData LocaleJsonData = JsonMapper.ToObject(LocaleFileContent);

            foreach (string LanguageCode in LocaleJsonData.Keys)
            {
                if (_LocaleStringDict[InNamespace].ContainsKey(LanguageCode) == false)
                {
                    _LocaleStringDict[InNamespace][LanguageCode] = new Dictionary<string, string>();
                }

                JsonData StringJsonData = LocaleJsonData[LanguageCode];

                foreach (string StringId in StringJsonData.Keys)
                {
                    string LocaleString = (string)StringJsonData[StringId];
                    _LocaleStringDict[InNamespace][LanguageCode].Add(StringId, LocaleString);
                }
            }
        }
        catch (System.Exception Err)
        {
            Debug.LogError($"Fail to read {InLocaleFilePath}\n Error: {Err.Message}");
        }
    }

    //
    // Texture Data
    //

    private void AsyncLoadTexture(string TexturePath)
    {
        if (TextureDict.ContainsKey(TexturePath))
        {
            return;
        }

        if (!File.Exists(TexturePath))
        {
            Debug.LogError(TexturePath + " no exist!");
            return;
        }
        try
        {
            byte[] TextureData = File.ReadAllBytes(TexturePath);
            Texture2D NewTexture = new Texture2D(2, 2);
            if (NewTexture.LoadImage(TextureData))
            {
                TextureDict[TexturePath] = NewTexture;
            }
            else
            {
                Debug.LogError("Fail to load texture: " + TexturePath);
            }
        }
        catch (System.Exception Err)
        {
            Debug.LogError($"Fail to read {TexturePath}\n Error: {Err.Message}");
        }
    }

    public bool GetTexture(string TexturePath, out Texture2D OutTexture)
    {
        if (TextureDict.TryGetValue(TexturePath, out Texture2D TempTexture))
        {
            OutTexture = TempTexture;
            return true;
        }

        OutTexture = null;
        return false;
    }

    private void LoadTextureData()
    {
        foreach (M3_UnitData UnitData in _UnitDataList)
        {
            foreach (M3_AnimationData AnimData in UnitData.AnimationTable.Values)
            {
                foreach (string TexturePath in AnimData.Keyframes)
                {
                    AsyncLoadTexture(M3_PathHelper.GetModSubfilePath(TexturePath));
                }
            }
        }

        foreach (M3_TileData TileData in _TileDataList)
        {
            foreach (M3_AnimationData AnimData in TileData.AnimationTable.Values)
            {
                foreach (string TexturePath in AnimData.Keyframes)
                {
                    AsyncLoadTexture(M3_PathHelper.GetModSubfilePath(TexturePath));
                }
            }
        }
    }
}
