using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M3_Event_ReadTileFile : M3_Event
{
    public M3_Event_ReadTileFile(string InModId, string InTileFilePath)
    {
        BelongingModId = InModId;
        TileFilePath = InTileFilePath;
    }

    public string BelongingModId;
    public string TileFilePath;
}
