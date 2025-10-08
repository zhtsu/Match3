using LitJson;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class M3_SpawnHelper
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
}
