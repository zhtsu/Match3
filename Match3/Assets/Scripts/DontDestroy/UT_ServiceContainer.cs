using UnityEngine;

public struct UT_FServiceContainerInitParams
{
    public UT_SO_GameConfig GameConfig;
    public UT_SO_PrefabConfig PrefabConfig;
    public UT_SO_UIConfig UIConfig;
}

public class UT_ServiceContainer : MonoBehaviour
{
    private UT_EventManager _EventManager = null;
    private UT_CommandManager _CommandManager = null;
    private UT_UIManager _UIManager = null;
    private UT_AudioManager _AudioManager = null;
    private UT_PrefabManager _PrefabManager = null;
    private UT_ScriptManager _ScriptManager = null;

    private UT_EventService _EventService = null;
    private UT_CommandService _CommandService = null;
    private UT_CoroutineService _CoroutineService = null;
    private UT_SpawnService _SpawnService = null;
    private UT_UIService _UIService = null;

    private void LateUpdate()
    {
        if (_EventManager != null)
        {
            _EventManager.Update();
        }
    }

    public void Initialize(UT_FServiceContainerInitParams Params)
    {
        if (!CheckObject<UT_SO_GameConfig>(Params.GameConfig)) return;
        if (!CheckObject<Camera>(Params.GameConfig.MainCameraPrefab)) return;
        if (!CheckObject<UT_UIRoot>(Params.GameConfig.UIRootPrefab)) return;
        if (!CheckObject<UT_AudioRoot>(Params.GameConfig.AudioRootPrefab)) return;

        // Services
        _EventService = GetComponent<UT_EventService>();
        _CoroutineService = GetComponent<UT_CoroutineService>();
        _CommandService = GetComponent<UT_CommandService>();
        _SpawnService = GetComponent<UT_SpawnService>();
        _UIService = GetComponent<UT_UIService>();

        // Managers
        UT_UIRoot UIRoot = Instantiate(Params.GameConfig.UIRootPrefab);
        UT_AudioRoot AudioRoot = Instantiate(Params.GameConfig.AudioRootPrefab);

        _UIManager = new UT_UIManager(UIRoot, _SpawnService);
        _AudioManager = new UT_AudioManager(AudioRoot);
        _EventManager = new UT_EventManager();
        _CommandManager = new UT_CommandManager(_CoroutineService);
        _PrefabManager = new UT_PrefabManager(_EventService, Params.PrefabConfig);

        // Show Loading Screen
        UT_UI_LoadingScreen LoadingScreen = Instantiate(Params.UIConfig.LoadingScreenPrefab);
        _UIManager.OpenScreenUI(LoadingScreen, UT_EUILayer.Top, null);

        // Initialize Services
        _SpawnService.Initialize(_PrefabManager);
        _EventService.Initialize(_EventManager);
        _UIService.Initialize(_UIManager);

        // Initialize Managers
        InitManager<UT_EventManager>(_EventManager);
        InitManager<UT_CommandManager>(_CommandManager);
        InitManager<UT_PrefabManager>(_PrefabManager);
        InitManager<UT_UIManager>(_UIManager);
        InitManager<UT_AudioManager>(_AudioManager);
    }

    public void Destroy()
    {
        DestroyManager<UT_UIManager>(_UIManager);
        DestroyManager<UT_EventManager>(_EventManager);
        DestroyManager<UT_PrefabManager>(_PrefabManager);
        DestroyManager<UT_CommandManager>(_CommandManager);
        DestroyManager<UT_AudioManager>(_AudioManager);
    }

    private void InitManager<T>(T Manager)
        where T : UT_Manager
    {
        if (Manager != null)
        {
            Manager.Initialize();
            Debug.Log(Manager.ManagerName + " initialized!");
        }
        else
        {
            Debug.LogWarning($"Invalid {Manager.ManagerName}!");
        }
    }

    private void DestroyManager<T>(T Manager)
        where T : UT_Manager
    {
        if (Manager != null)
        {
            Manager.Destroy();
            Debug.Log(Manager.ManagerName + " destroyed!");
        }
        else
        {
            Debug.LogWarning($"Invalid {Manager.ManagerName}!");
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private bool CheckObject<T>(object Object)
    {
        if (Object == null || Object is not T)
        {
            Debug.LogWarning($"Failed to create {typeof(T)}!");
            return false;
        }

        return true;
    }
}
