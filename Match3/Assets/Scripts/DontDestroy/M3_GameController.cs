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
    [SerializeField]
    private M3_GameConfig _GameConfig;

    private static M3_GameController _Instance;
    private M3_GlobalData _GlobalData;

    public static M3_GameController Instance { get { return _Instance; } }
    public M3_GameConfig GameConfig { get { return _GameConfig; } }
    public M3_GlobalData GlobalData { get { return _GlobalData; } }

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

        M3_ManagerHub.Instance.Initialize();

        M3_ManagerHub.Instance.EventManager.Subscribe<M3_Event_PrefabsLoadCompleted>(OnPrefabsLoadCompleted);

        // M3_ManagerHub.Instance.UIManager.OpenUI(M3_UIType.MainMenu);
    }

    private void Update()
    {
        if (M3_ManagerHub.Instance.CommandManager != null)
        {
            M3_ManagerHub.Instance.CommandManager.Update();
        }
    }

    private void OnDestroy()
    {
        M3_ManagerHub.Instance.Destroy();
    }

    void SetMouseDragDirection(M3_MouseMoveDirection Direction)
    {
        _GlobalData.MouseDragDirection = Direction;
    }

    private void OnPrefabsLoadCompleted(M3_Event_PrefabsLoadCompleted Event)
    {
        _GlobalData.IsPrefabsLoadCompleted = true;
        M3_ManagerHub.Instance.EventManager.Unsubscribe<M3_Event_PrefabsLoadCompleted>(OnPrefabsLoadCompleted);
    }
}
