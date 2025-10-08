using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M3_Unit : MonoBehaviour
{
    private Dictionary<string, List<Sprite>> _AnimTable = new Dictionary<string, List<Sprite>>();
    private string _PlayingAnimName = null;
    private string _DefaultAnimName = "Idle";

    public void SetUnitData(M3_UnitData UnitData)
    {
        M3_DataManager DataManager = M3_ManagerHub.Instance.DataManager;

        _AnimTable.Clear();

        foreach (string AnimName in UnitData.AnimationTable.Keys)
        {
            M3_AnimationData AnimData = UnitData.AnimationTable[AnimName];

            List<Sprite> SpriteList = new List<Sprite>();
            foreach (string TexturePath in AnimData.Keyframes)
            {
                string TextureId = M3_PathHelper.GetModSubfilePath(TexturePath);
                if (DataManager.GetTexture(TextureId, out Texture2D Texture))
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
            }
        }
    }
}
