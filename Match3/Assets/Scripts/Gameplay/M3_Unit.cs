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
        _AnimTable.Clear();

        foreach (string AnimName in UnitData.AnimationTable.Keys)
        {
            M3_AnimationData AnimData = UnitData.AnimationTable[AnimName];

            List<Sprite> SpriteList = new List<Sprite>();
            foreach (string TexturePath in AnimData.Keyframes)
            {
                SpriteList.Add(M3_CommonHelper.GetSprite(TexturePath));
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

        _ActionScript = M3_CommonHelper.GetScript(UnitData.ScriptPath, this);
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
