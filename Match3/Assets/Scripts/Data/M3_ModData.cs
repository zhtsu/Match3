using LitJson;
using UnityEngine;

public class M3_ModData : M3_Data
{
    public string Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string Author { get; private set; }
    public string Email { get; private set; }
    public string[] TileList { get; private set; }
    public string[] UnitList { get; private set; }
    public string[] LocaleList { get; private set; }
}
