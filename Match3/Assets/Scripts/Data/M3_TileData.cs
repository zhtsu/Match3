using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using XLua;

//[CreateAssetMenu(fileName = "NewTileData", menuName = "FB Data/TileData")]
public class M3_TileData : M3_Data
{
    public struct FAnimation
    {
        public string Type;

        // When Type == "Sprite"
        public string[] Keyframes;

        // When Type == "Sheet"
        // TODO

        // When Type == "Spine"
        // TODO
    };

    public string Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }

    public Dictionary<string, FAnimation> AnimationTable { get; private set; } = new Dictionary<string, FAnimation>();

    public string BelongingModId { get; private set; }

    public void Initialize(string ModId)
    {
        BelongingModId = ModId;
        AnimationTable.Clear();
    }
}
