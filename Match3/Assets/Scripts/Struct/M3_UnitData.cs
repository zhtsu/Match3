using System.Collections.Generic;

//[CreateAssetMenu(fileName = "NewUnitData", menuName = "FB Data/UnitData")]
public struct M3_UnitData
{
    public string Id;
    public string Name;
    public string Description;
    public int Width;
    public int Height;
    public Dictionary<string, M3_AnimationData> AnimationTable;
    public string BelongingModId;
}
