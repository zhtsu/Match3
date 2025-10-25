using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum M3_GameState
{
    None = 0,
    Loading,
    MainMenu,
    InGame,
    Paused,
}

public class M3_GameController : MonoBehaviour
{
    [SerializeField]
    private M3_GameConfig _GameConfig;
    [SerializeField]
    private M3_UserConfig _UserConfig;
    [SerializeField]
    private AudioSource _MusicAudioSource;
    [SerializeField]
    private AudioSource _FXAudioSource;
    [SerializeField]
    private AudioMixer _MainMixer;

    private static M3_GameController _Instance;
    private M3_GlobalData _GlobalData;
    private M3_ModAPI _ModAPI = new M3_ModAPI();

    public static M3_GameController Instance { get { return _Instance; } }
    public M3_GameConfig GameConfig { get { return _GameConfig; } }
    public M3_GlobalData GlobalData { get { return _GlobalData; } }
    public M3_ModAPI ModAPI { get { return _ModAPI; } }
    public M3_Gem CurrentClickedGem { get { return _GlobalData.CurrentClickedGem; } }
    public bool IsAllowInput { get { return _GlobalData.IsAllowInput; } }
    public M3_ControllerType CurrentBattleInputController { get { return _GlobalData.CurrentBattleInputController; } }

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
        _GlobalData.IsAllowInput = true;

        _GlobalData.CurrentM3Battle = null;
        _GlobalData.CurrentGameState = M3_GameState.Loading;
        _GlobalData.CurrentBattleInputController = M3_ControllerType.Player;

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
            M3_LoadingScreenUIParams UIParams = new M3_LoadingScreenUIParams();
            UIParams.OnStartLoading += StartLoading;
            UIRoot.OpenUI(M3_UIType.LoadingScreen, UIParams, M3_UILayerType.Top);
        }
    }

    private void StartLoading()
    {
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

    public void RunCoroutine(IEnumerator Coroutine)
    {
        StartCoroutine(Coroutine);
    }

    private void Update()
    {
        if (!_GlobalData.IsGameReady)
        {
            if (CheckGameReady())
            {
                _GlobalData.IsGameReady = true;
                _GlobalData.CurrentGameState = M3_GameState.MainMenu;
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

    public void SetCurrentM3Battle(M3_Match3Battle M3Battle)
    {
        _GlobalData.CurrentM3Battle = M3Battle;
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

        M3_CommonHelper.OpenUI(M3_UIType.MainMenu);

        M3_EventBus.SendEvent<M3_Event_LoadingCompleted>();
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

    public void SetCurrentClickedGem(M3_Gem ClickedGem)
    {
        _GlobalData.CurrentClickedGem = ClickedGem;
    }

    public void SetAllowInput(bool IsAllow)
    {
        _GlobalData.IsAllowInput = IsAllow;
    }

    public void SetBattleInputController(M3_ControllerType ControllerType)
    {
        _GlobalData.CurrentBattleInputController = ControllerType;
    }

    public float GetFXVolume() { return _UserConfig.FXVolume; }
    public float GetMusicVolume() { return _UserConfig.MusicVolume; }

    public void SetFXVolume(float Volume)
    {
        _UserConfig.FXVolume = Volume;
    }

    public void SetMusicVolume(float Volume)
    {
        _UserConfig.MusicVolume = Volume;
    }

    public void PlayMusic(AudioClip MusicClip)
    {
        if (_MusicAudioSource != null)
        {
            _MusicAudioSource.clip = MusicClip;
            _MusicAudioSource.Play();
        }
    }

    public void PlayFX(AudioClip FXClip)
    {
        if (_FXAudioSource != null)
        {
            _FXAudioSource.clip = FXClip;
            _FXAudioSource.Play();
        }
    }
}
