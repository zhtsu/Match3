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
