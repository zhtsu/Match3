using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M3_Event_ReadUnitFile : M3_Event
{
    public M3_Event_ReadUnitFile(string InModId, string InUnitFilePath)
    {
        BelongingModId = InModId;
        UnitFilePath = InUnitFilePath;
    }

    public string BelongingModId;
    public string UnitFilePath;
}
