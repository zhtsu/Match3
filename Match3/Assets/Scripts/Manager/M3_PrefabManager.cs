using System;
using UnityEngine.AddressableAssets;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;

public enum M3_PrefabType
{
    Gem = 0,
}

public class M3_PrefabManager : M3_Manager
{
    public override string ManagerName
    {
        get { return "Prefab Manager"; }
    }

    private Dictionary<M3_PrefabType, AsyncOperationHandle<GameObject>> _PrefabHandleDict = new Dictionary<M3_PrefabType, AsyncOperationHandle<GameObject>>();

    public override void Initialize()
    {
        for (int i = 0; i < M3_GameController.Instance.GameConfig.PrefabTypeList.Length; i++)
        {
            M3_PrefabType PrefabType = M3_GameController.Instance.GameConfig.PrefabTypeList[i];
            string Address = M3_GameController.Instance.GameConfig.PrefabAddressList[i];

            AsyncOperationHandle<GameObject> Handle = Addressables.LoadAssetAsync<GameObject>(Address);
            Handle.Completed += OnPrefabLoaded;
            _PrefabHandleDict.Add(PrefabType, Handle);
        }
    }

    public override void Destroy()
    {
        foreach (AsyncOperationHandle<GameObject> Handle in _PrefabHandleDict.Values)
        {
            Addressables.Release(Handle);
        }

        _PrefabHandleDict.Clear();
    }

    public GameObject GetPrefab(M3_PrefabType PrefabType)
    {
        if (_PrefabHandleDict.TryGetValue(PrefabType, out AsyncOperationHandle<GameObject> OutHandle))
        {
            return OutHandle.Result;
        }

        return null;
    }

    private void OnPrefabLoaded(AsyncOperationHandle<GameObject> Handle)
    {
        if (Handle.Status == AsyncOperationStatus.Succeeded)
        {
            if (_PrefabHandleDict.Count == M3_GameController.Instance.GameConfig.PrefabAddressList.Length)
            {
                M3_ManagerHub.Instance.EventManager.SendEvent<M3_Event_PrefabsLoadCompleted>();
            }
        }
    }
}
