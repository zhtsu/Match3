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
    private M3_ModAPI _MODAPI = new M3_ModAPI();

    public static M3_GameController Instance { get { return _Instance; } }
    public M3_GameConfig GameConfig { get { return _GameConfig; } }
    public M3_GlobalData GlobalData { get { return _GlobalData; } }
    public M3_ModAPI MODAPI { get { return _MODAPI; } }

    private void Awake()
    {
        if (_Instance != null && _Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _GlobalData.IsGameReady = false;
        _GlobalData.IsPrefabsLoadCompleted = false;
        _GlobalData.IsTexturesLoadCompleted = false;
        _GlobalData.IsScriptsLoadCompleted = false;
        _GlobalData.IsStoriesLoadCompleted = false;

        _Instance = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(this);

        if (M3_GameController.Instance.GameConfig.UIRootPrefab)
        {
            Instantiate(M3_GameController.Instance.GameConfig.UIRootPrefab);
        }

        M3_UIRoot UIRoot = Object.FindFirstObjectByType<M3_UIRoot>();
        if (UIRoot != null)
        {
            UIRoot.OpenUI(M3_UIType.LoadingScreen, M3_UILayerType.Top);
        }

        M3_EventBus.Subscribe<M3_Event_GameReady>(OnGameReady);
        M3_EventBus.Subscribe<M3_Event_ManagerHubReady>(OnManagerHubReady);
        M3_EventBus.Subscribe<M3_Event_TexturesLoadCompleted>(OnTexturesLoadCompleted);
        M3_EventBus.Subscribe<M3_Event_PrefabsLoadCompleted>(OnPrefabsLoadCompleted);
        M3_EventBus.Subscribe<M3_Event_ScriptsLoadCompleted>(OnScriptsLoadCompleted);
        M3_EventBus.Subscribe<M3_Event_StoriesLoadCompleted>(OnStoriesLoadCompleted);

        M3_ManagerHub.Instance.Initialize();
    }

    private bool CheckGameReady()
    {
        return _GlobalData.IsPrefabsLoadCompleted &&
               _GlobalData.IsTexturesLoadCompleted &&
               _GlobalData.IsScriptsLoadCompleted &&
               _GlobalData.IsStoriesLoadCompleted &&
               _GlobalData.IsManagerHubReady;
    }

    private void Update()
    {
        if (!_GlobalData.IsGameReady)
        {
            if (CheckGameReady())
            {
                _GlobalData.IsGameReady = true;
                M3_EventBus.SendEvent(new M3_Event_GameReady());
            }
        }

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

    private void OnManagerHubReady(M3_Event_ManagerHubReady Event)
    {
        Debug.Log("Manager Hub Ready");
        M3_EventBus.Unsubscribe<M3_Event_ManagerHubReady>(OnManagerHubReady);

        _GlobalData.IsManagerHubReady = true;
    }

    private void OnGameReady(M3_Event_GameReady Event)
    {
        Debug.Log("Game Ready");
        M3_EventBus.Unsubscribe<M3_Event_GameReady>(OnGameReady);

        M3_UIManager UIManager = M3_ManagerHub.Instance.UIManager;
        UIManager.CloseUI(M3_UIType.LoadingScreen);
        UIManager.OpenUI(M3_UIType.MainMenu);

        Test();
    }

    private void OnTexturesLoadCompleted(M3_Event_TexturesLoadCompleted Event)
    {
        Debug.Log("Textures Load Completed");
        M3_EventBus.Unsubscribe<M3_Event_TexturesLoadCompleted>(OnTexturesLoadCompleted);

        _GlobalData.IsTexturesLoadCompleted = true;
    }

    private void OnPrefabsLoadCompleted(M3_Event_PrefabsLoadCompleted Event)
    {
        Debug.Log("Prefabs Load Completed");
        M3_EventBus.Unsubscribe<M3_Event_PrefabsLoadCompleted>(OnPrefabsLoadCompleted);

        _GlobalData.IsPrefabsLoadCompleted = true;
    }

    private void OnScriptsLoadCompleted(M3_Event_ScriptsLoadCompleted Event)
    {
        Debug.Log("Scripts Load Completed");
        M3_EventBus.Unsubscribe<M3_Event_ScriptsLoadCompleted>(OnScriptsLoadCompleted);

        _GlobalData.IsScriptsLoadCompleted = true;
    }

    private void OnStoriesLoadCompleted(M3_Event_StoriesLoadCompleted Event)
    {
        Debug.Log("Stories Load Completed");
        M3_EventBus.Unsubscribe<M3_Event_StoriesLoadCompleted>(OnStoriesLoadCompleted);

        _GlobalData.IsStoriesLoadCompleted = true;
    }

    void Test()
    {
        M3_Grid Grid = FindObjectOfType<M3_Grid>();
        Grid.Initialize(3, 3, 1f);
        Grid.GenerateGrid();

        M3_Gem elephant = M3_CommonHelper.SpawnGem("animals", "elephant_gem");
        Grid.AddCell(elephant, 0, 0, M3_FillMode.AspectFit);

        M3_Gem giraffe = M3_CommonHelper.SpawnGem("animals", "giraffe_gem");
        Grid.AddCell(giraffe, 0, 1, M3_FillMode.AspectFit);

        M3_Gem hippo = M3_CommonHelper.SpawnGem("animals", "hippo_gem");
        Grid.AddCell(hippo, 0, 2, M3_FillMode.AspectFit);

        M3_Gem monkey = M3_CommonHelper.SpawnGem("animals", "monkey_gem");
        Grid.AddCell(monkey, 1, 0, M3_FillMode.AspectFit);

        M3_Gem panda = M3_CommonHelper.SpawnGem("animals", "panda_gem");
        Grid.AddCell(panda, 1, 1, M3_FillMode.AspectFit);

        M3_Gem parrot = M3_CommonHelper.SpawnGem("animals", "parrot_gem");
        Grid.AddCell(parrot, 1, 2, M3_FillMode.AspectFit);

        M3_Gem penguin = M3_CommonHelper.SpawnGem("animals", "penguin_gem");
        Grid.AddCell(penguin, 2, 0, M3_FillMode.AspectFit);

        M3_Gem pig = M3_CommonHelper.SpawnGem("animals", "pig_gem");
        Grid.AddCell(pig, 2, 1, M3_FillMode.AspectFit);

        M3_Gem rabbit = M3_CommonHelper.SpawnGem("animals", "rabbit_gem");
        Grid.AddCell(rabbit, 2, 2, M3_FillMode.AspectFit);
    }
}
