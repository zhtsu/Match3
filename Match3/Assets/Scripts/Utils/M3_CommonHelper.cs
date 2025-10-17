using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;
using XLua;

public class M3_CommonHelper
{
    public static M3_Gem SpawnGem(string ModId, string GemId)
    {
        M3_DataManager DataManager = M3_ManagerHub.Instance.DataManager;

        GameObject GemPrefab = M3_ManagerHub.Instance.PrefabManager.GetPrefab(M3_PrefabType.Gem);
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

    public static GameObject GetPrefab(M3_PrefabType PrefabType)
    {
        M3_DataManager DataManager = M3_ManagerHub.Instance.DataManager;

        GameObject GemPrefab = M3_ManagerHub.Instance.PrefabManager.GetPrefab(PrefabType);
        if (GemPrefab != null)
        {
            return GemPrefab;
        }

        return null;
    }

    public static M3_PrefabType UITypeToPrefabType(M3_UIType UIType)
    {
        switch (UIType)
        {
            case M3_UIType.MainMenu:
                return M3_PrefabType.MainMenu;
            case M3_UIType.Mod:
                return M3_PrefabType.Mod;
            case M3_UIType.Config:
                return M3_PrefabType.Config;
        }

        return M3_PrefabType.Max;
    }

    public static GameObject GetUIPrefab(M3_UIType UIType)
    {
        return GetPrefab(UITypeToPrefabType(UIType));
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

        Hash128 ScriptId = M3_PathHelper.GetHash(ScriptPath);
        if (ScriptManager.GetScript(ScriptId, out LuaTable OutLuaTable))
        {
            OutLuaTable.Set("Self", SetToSelf);
            OutLuaTable.Set("ModAPI", M3_GameController.Instance.MODAPI);
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
}
