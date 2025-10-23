using Ink.Parsed;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XLua;

public enum M3_ColorType
{
    None,
    White,
    Black,
    Cyan,
    HyperLink,
    TileWhite,
    TileBlack,
}

public class M3_CommonHelper
{
    public static Color GetCommonColor(M3_ColorType ColorType)
    {
        switch (ColorType)
        {
            case M3_ColorType.White:
                {
                    UnityEngine.ColorUtility.TryParseHtmlString("#EBE0D0", out Color White);
                    return White;
                }
            case M3_ColorType.Black:
                {
                    ColorUtility.TryParseHtmlString("#262020", out Color Black);
                    return Black;
                }
            case M3_ColorType.Cyan:
                {
                    ColorUtility.TryParseHtmlString("#F5F5E5", out Color Cyan);
                    return Cyan;
                }
            case M3_ColorType.HyperLink:
                {
                    ColorUtility.TryParseHtmlString("#6ACCCB", out Color HyperLink);
                    return HyperLink;
                }
            case M3_ColorType.TileWhite:
                {
                    ColorUtility.TryParseHtmlString("#3A3434", out Color Col);
                    return Col;
                }
            case M3_ColorType.TileBlack:
                {
                    ColorUtility.TryParseHtmlString("#332C2C", out Color Col);
                    return Col;
                }
        }

        return Color.white;
    }

    public static void SetButtonTextColor(Button InButton, M3_ColorType ColorType)
    {
        if (InButton == null)
            return;

        TextMeshProUGUI[] ObjList = InButton.GetComponentsInChildren<TextMeshProUGUI>();
        if (ObjList.Length == 0)
            return;

        TextMeshProUGUI TextObj = ObjList[0];
        TextObj.color = GetCommonColor(ColorType);
    }

    public static void SetImageColor(Image InImage, M3_ColorType ColorType)
    {
        if (InImage == null)
            return;

        InImage.GetComponent<Image>().color = GetCommonColor(ColorType);
    }

    public static M3_Tile SpawnTile()
    {
        M3_DataManager DataManager = M3_ManagerHub.Instance.DataManager;

        GameObject TilePrefab = M3_ManagerHub.Instance.PrefabManager.GetPrefab("Tile");
        if (TilePrefab != null)
        {
            M3_Tile Tile = GameObject.Instantiate(TilePrefab).GetComponent<M3_Tile>();

            return Tile;
        }

        return null;
    }

    public static M3_Gem SpawnGem(string ModId, string GemId)
    {
        M3_DataManager DataManager = M3_ManagerHub.Instance.DataManager;

        GameObject GemPrefab = M3_ManagerHub.Instance.PrefabManager.GetPrefab("Gem");
        if (GemPrefab != null)
        {
            M3_Gem Gem = GameObject.Instantiate(GemPrefab).GetComponent<M3_Gem>();

            if (DataManager.GetUnitData(ModId, GemId, out M3_UnitData UnitData))
            {
                Gem.SetUnitData(UnitData);
            }

            return Gem;
        }

        return null;
    }

    public static void SetFullStretch(RectTransform TargetRectTransform)
    {
        TargetRectTransform.anchorMin = Vector2.zero;
        TargetRectTransform.anchorMax = Vector2.one;

        TargetRectTransform.offsetMin = Vector2.zero;
        TargetRectTransform.offsetMax = Vector2.zero;

        TargetRectTransform.pivot = new Vector2(0.5f, 0.5f);
    }

    public static GameObject GetPrefab(string Address)
    {
        M3_DataManager DataManager = M3_ManagerHub.Instance.DataManager;
        return M3_ManagerHub.Instance.PrefabManager.GetPrefab(Address);
    }

    public static string GetGameString(string Namespace, string LanguageCode, string StringId)
    {
        M3_DataManager DataManager = M3_ManagerHub.Instance.DataManager;
        return DataManager.GetLocalString(Namespace, LanguageCode, StringId);
    }

    public static void OpenUI(M3_UIType TargetUIType, M3_UIParams Params = null, M3_UILayerType Layer = M3_UILayerType.Main)
    {
        M3_UIManager UIManager = M3_ManagerHub.Instance.UIManager;
        UIManager.OpenUI(TargetUIType, Params, Layer);
    }

    public static void CloseUI(M3_UIType TargetUIType)
    {
        M3_UIManager UIManager = M3_ManagerHub.Instance.UIManager;
        UIManager.CloseUI(TargetUIType);
    }

    public static void ShowUI(M3_UIType TargetUIType)
    {
        M3_UIManager UIManager = M3_ManagerHub.Instance.UIManager;
        UIManager.ShowUI(TargetUIType);
    }

    public static void HideUI(M3_UIType TargetUIType)
    {
        M3_UIManager UIManager = M3_ManagerHub.Instance.UIManager;
        UIManager.HideUI(TargetUIType);
    }

    public static void CloseAllUI()
    {
        M3_UIManager UIManager = M3_ManagerHub.Instance.UIManager;
        UIManager.CloseAllUI();
    }

    public static string UITypeToPrefabAddress(M3_UIType UIType)
    {
        switch (UIType)
        {
            case M3_UIType.MainMenu:
                return "MainMenu";
            case M3_UIType.Mod:
                return "Mod";
            case M3_UIType.Config:
                return "Config";
            case M3_UIType.StoryView:
                return "StoryView";
            case M3_UIType.ModSelect:
                return "ModSelect";
        }

        return null;
    }

    public static GameObject GetUIPrefab(M3_UIType UIType)
    {
        return GetPrefab(UITypeToPrefabAddress(UIType));
    }

    public static Ink.Runtime.Story GetStory(string StoryPath)
    {
        M3_StoryManager StoryManager = M3_ManagerHub.Instance.StoryManager;

        Hash128 StoryId = M3_PathHelper.GetHash(StoryPath);
        if (StoryManager.GetStory(StoryId, out Ink.Runtime.Story OutStory))
        {
            return OutStory;
        }

        return null;
    }

    public static XLua.LuaTable GetScript<T>(string ScriptPath, T SetToSelf)
    {
        M3_ScriptManager ScriptManager = M3_ManagerHub.Instance.ScriptManager;

        Hash128 ScriptId = M3_PathHelper.GetHash(M3_PathHelper.GetModSubfilePath(ScriptPath));
        if (ScriptManager.GetScript(ScriptId, out LuaTable OutLuaTable))
        {
            OutLuaTable.Set("Self", SetToSelf);
            OutLuaTable.Set("ModAPI", M3_GameController.Instance.ModAPI);

            return OutLuaTable;
        }

        return null;
    }

    public static Sprite GetSprite(string TexturePath)
    {
        M3_TextureManager TexManager = M3_ManagerHub.Instance.TextureManager;

        Hash128 TextureId = M3_PathHelper.GetHash(TexturePath);
        if (TexManager.GetTexture(TextureId, out Texture2D Texture))
        {
            return Sprite.Create(Texture, new Rect(0, 0, Texture.width, Texture.height), new Vector2(0.5f, 0.5f));
        }

        return null;
    }

    public static void BindModApiToStory(Ink.Runtime.Story InStory)
    {
        Action StartMatch3 = M3_GameController.Instance.ModAPI.StartMatch3;

        InStory.BindExternalFunction("StartMatch3", StartMatch3);
    }

    public static M3_UnitData GetRandomGemData()
    {
        M3_DataManager DataManager = M3_ManagerHub.Instance.DataManager;
        
        List<M3_UnitData> UnitDataList = DataManager.GetUnitDataList();
        if (UnitDataList.Count == 0)
            return new M3_UnitData();

        int RandomIndex = UnityEngine.Random.Range(0, 5);
        return UnitDataList[RandomIndex];
    }
}
