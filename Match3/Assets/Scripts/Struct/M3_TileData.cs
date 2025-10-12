using System.Collections.Generic;

//[CreateAssetMenu(fileName = "NewTileData", menuName = "FB Data/TileData")]
public struct M3_TileData
{
    public string Id;
    public string Name;
    public string Description;
    public Dictionary<string, M3_AnimationData> AnimationTable;
    public string ScriptPath;
    // from mod data
    public string BelongingModId;
}
