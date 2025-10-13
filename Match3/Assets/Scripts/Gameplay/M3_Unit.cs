using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class M3_Unit : MonoBehaviour
{
    private Dictionary<string, List<Sprite>> _AnimTable = new Dictionary<string, List<Sprite>>();
    private string _PlayingAnimName = null;
    private string _DefaultAnimName = "Idle";
    private LuaTable _ActionScript = null;

    public void SetUnitData(M3_UnitData UnitData)
    {
        M3_TextureManager TexManager = M3_ManagerHub.Instance.TextureManager;
        M3_ScriptManager ScriptManager = M3_ManagerHub.Instance.ScriptManager;

        _AnimTable.Clear();

        foreach (string AnimName in UnitData.AnimationTable.Keys)
        {
            M3_AnimationData AnimData = UnitData.AnimationTable[AnimName];

            List<Sprite> SpriteList = new List<Sprite>();
            foreach (string TexturePath in AnimData.Keyframes)
            {
                string TextureId = M3_PathHelper.GetModSubfilePath(TexturePath);
                if (TexManager.GetTexture(TextureId, out Texture2D Texture))
                {
                    Sprite NewSprite = Sprite.Create(Texture, new Rect(0, 0, Texture.width, Texture.height), new Vector2(0.5f, 0.5f));
                    SpriteList.Add(NewSprite);
                }
            }
            _AnimTable.Add(AnimName, SpriteList);
        }

        SpriteRenderer Renderer = GetComponent<SpriteRenderer>();
        if (Renderer != null)
        {
            if (_AnimTable.TryGetValue(_DefaultAnimName, out List<Sprite> SpriteList))
            {
                if (SpriteList.Count > 0)
                {
                    Renderer.sprite = SpriteList[0];
                }

                _PlayingAnimName = _DefaultAnimName;
            }
        }

        BoxCollider2D Collider = GetComponent<BoxCollider2D>();
        if (Collider != null && Renderer != null && Renderer.sprite != null)
        {
            Collider.size = Renderer.sprite.bounds.size;
        }

        string ScriptId = M3_PathHelper.GetModSubfilePath(UnitData.ScriptPath);
        if (ScriptManager.GetScript(ScriptId, out LuaTable OutLuaTable))
        {
            _ActionScript = OutLuaTable;
            _ActionScript.Set("Self", this);
            _ActionScript.Set("MODAPI", M3_GameController.Instance.MODAPI);
        }
    }

    public void CallScriptFunc(string FuncName, params object[] Args)
    {
        if (_ActionScript != null)
        {
            LuaFunction Func = _ActionScript.Get<LuaFunction>(FuncName);
            if (Func != null)
            {
                Func.Call(Args);
            }
        }
    }
}
