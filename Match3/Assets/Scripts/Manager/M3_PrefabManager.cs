using System;
using UnityEngine.AddressableAssets;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;

public enum FB_PrefabType
{
    SquareTile = 0,
}

public class M3_PrefabManager : M3_Manager
{
    public override string ManagerName
    {
        get { return "PrefabManager"; }
    }

    private string[] _PrefabAddressList = {
        // Tile
        "Assets/Prefabs/SquareTile.prefab"
    };

    private static List<AsyncOperationHandle<GameObject>> _PrefabHandleList = new List<AsyncOperationHandle<GameObject>>();

    public override void Initialize()
    {
        foreach (string Address in _PrefabAddressList)
        {
            Addressables.LoadAssetAsync<GameObject>(Address).Completed += OnPrefabLoaded;
        }
    }

    public override void Destroy()
    {
        foreach (AsyncOperationHandle<GameObject> Handle in _PrefabHandleList)
        {
            Addressables.Release(Handle);
        }
    }

    public static GameObject GetPrefab(FB_PrefabType PrefabType)
    {
        if (_PrefabHandleList.Count > (int)PrefabType)
        {
            AsyncOperationHandle<GameObject> Handle = _PrefabHandleList[(int)PrefabType];
            return Handle.Result;
        }

        return null;
    }

    private void OnPrefabLoaded(AsyncOperationHandle<GameObject> Handle)
    {
        if (Handle.Status == AsyncOperationStatus.Succeeded)
        {
            _PrefabHandleList.Add(Handle);
            Debug.Log(Handle.DebugName);

            if (_PrefabHandleList.Count == _PrefabAddressList.Length)
            {
                M3_EventManager.SendEvent<M3_Event_PrefabsLoadCompleted>();
            }
        }
    }
}
