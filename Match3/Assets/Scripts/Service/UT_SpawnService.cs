using System;
using System.Collections.Generic;
using UnityEngine;



public class UT_SpawnService : MonoBehaviour, UT_ISpawnService
{
    private UT_PrefabManager _PrefabManager;

    public void Initialize(UT_PrefabManager InPrefabManager)
    {
        _PrefabManager = InPrefabManager;
    }

    public GameObject CreateGameObject(string PrefabAddress)
    {
        GameObject Prefab = _PrefabManager.GetPrefab(PrefabAddress);
        if (Prefab == null)
        {
            Debug.LogWarning($"Invalid prefab: {PrefabAddress}");
            return null;
        }

        return Instantiate(Prefab);
    }
}
