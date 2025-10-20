using System;
using UnityEngine.AddressableAssets;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;

public class M3_PrefabManager : M3_Manager
{
    public override string ManagerName
    {
        get { return "Prefab Manager"; }
    }

    private Dictionary<Hash128, AsyncOperationHandle<GameObject>> _PrefabHandleDict = new Dictionary<Hash128, AsyncOperationHandle<GameObject>>();

    public override void Initialize()
    {
        foreach (string Address in M3_GameController.Instance.GameConfig.PrefabAddressList)
        {
            if (string.IsNullOrEmpty(Address) == false)
            {
                AsyncOperationHandle<GameObject> Handle = Addressables.LoadAssetAsync<GameObject>(Address);
                Handle.Completed += OnPrefabLoaded;
                _PrefabHandleDict.Add(Hash128.Compute(Address), Handle);
            }
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

    public GameObject GetPrefab(string Address)
    {
        if (_PrefabHandleDict.TryGetValue(Hash128.Compute(Address), out AsyncOperationHandle<GameObject> OutHandle))
        {
            return OutHandle.Result;
        }

        return null;
    }

    private void OnPrefabLoaded(AsyncOperationHandle<GameObject> Handle)
    {
        bool IsAllCompleted = true;
        foreach (var Iter in _PrefabHandleDict.Values)
        {
            if (Iter.IsDone == false)
            {
                IsAllCompleted = false;
                break;
            }
        }

        if (IsAllCompleted)
        {
            M3_EventBus.SendEvent<M3_Event_PrefabsLoadCompleted>();
        }
    }
}
