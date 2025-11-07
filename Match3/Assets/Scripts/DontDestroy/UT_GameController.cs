using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UT_EGameState
{
    None,
    Loading,
    InMainMenu,
    Playing,
    Paused,
}

public class UT_GameController : MonoBehaviour
{
    [SerializeField] private UT_SO_GameConfig _GameConfig;
    [SerializeField] private UT_SO_AudioConfig _AudioConfig;
    [SerializeField] private UT_SO_PrefabConfig _PrefabConfig;
    [SerializeField] private UT_SO_UIConfig _UIConfig;

    private UT_ServiceContainer _ServiceContainer;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public static bool IsGameReady()
    {
        return false;
    }

    private void Start()
    {
        if (_GameConfig.ServiceContainerPrefab == null)
        {
            Debug.LogWarning("Invalid ServiceContainer Prefab!");
            return;
        }

        InstantiateAsync(_GameConfig.MainCameraPrefab);

        _ServiceContainer = Instantiate(_GameConfig.ServiceContainerPrefab).GetComponent<UT_ServiceContainer>();
        UT_FServiceContainerInitParams Params = new();
        Params.GameConfig = _GameConfig;
        Params.PrefabConfig = _PrefabConfig;
        Params.UIConfig = _UIConfig;
        _ServiceContainer.Initialize(Params);
    }

    private void OnDestroy()
    {
        if (_ServiceContainer != null)
        {
            _ServiceContainer.Destroy();
        }
    }
}
