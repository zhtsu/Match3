using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M3_GameInstance : MonoBehaviour
{
    [SerializeField]
    private GameObject _UIRootPrefab;

    private bool _IsPrefabsLoadCompleted = false;
    public bool IsPrefabsLoadCompleted { get { return _IsPrefabsLoadCompleted; } }

    private void Start()
    {
        DontDestroyOnLoad(this);

        if (_UIRootPrefab)
        {
            Instantiate(_UIRootPrefab);
        }

        M3_EventManager.Subscribe<M3_Event_PrefabsLoadCompleted>(OnPrefabsLoadCompleted);

        M3_ManagerHub.Instance.Initialize();

        // M3_ManagerHub.Instance.UIManager.OpenUI(M3_UIType.MainMenu);
    }

    private void OnDestroy()
    {
        M3_ManagerHub.Instance.Destroy();
    }

    private void OnPrefabsLoadCompleted(M3_Event_PrefabsLoadCompleted Event)
    {
        _IsPrefabsLoadCompleted = true;
        M3_EventManager.Unsubscribe<M3_Event_PrefabsLoadCompleted>(OnPrefabsLoadCompleted);
    }
}
