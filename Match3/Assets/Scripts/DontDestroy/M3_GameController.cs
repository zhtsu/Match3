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

public class M3_GameController : MonoBehaviour
{
    private static M3_GameController _Instance;
    public static M3_GameController Instance { get { return _Instance; } }

    [SerializeField]
    private M3_GameConfig _GameConfig;
    public M3_GameConfig GameConfig { get { return _GameConfig; } }

    public M3_GlobalData GlobalData;

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

        if (M3_GameController.Instance.GameConfig.UIRootPrefab)
        {
            Instantiate(M3_GameController.Instance.GameConfig.UIRootPrefab);
        }

        M3_EventManager.Subscribe<M3_Event_PrefabsLoadCompleted>(OnPrefabsLoadCompleted);

        M3_ManagerHub.Instance.Initialize();

        // M3_ManagerHub.Instance.UIManager.OpenUI(M3_UIType.MainMenu);
    }

    private void Update()
    {
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
