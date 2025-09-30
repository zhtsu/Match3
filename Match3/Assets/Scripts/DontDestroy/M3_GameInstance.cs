using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum M3_GameState
{
    None = 0,
    MainMenu,
    InGame,
    Paused,
}

public class M3_GameInstance : MonoBehaviour
{
    private static M3_GameInstance _Instance;
    public static M3_GameInstance Instance { get { return _Instance; } }

    [SerializeField]
    private GameObject _UIRootPrefab;

    private bool _IsPrefabsLoadCompleted = false;
    public bool IsPrefabsLoadCompleted { get { return _IsPrefabsLoadCompleted; } }

    private M3_GameState _CurrentGameState = M3_GameState.None;
    public M3_GameState CurrentGameState { get { return _CurrentGameState; } }

    private void Awake()
    {
        if (_Instance != null && _Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _Instance = this;
    }

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
