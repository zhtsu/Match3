using UnityEngine;

public class M3_ManagerHub
{
    public M3_TileManager TileManager { get; private set; }
    public M3_UnitManager UnitManager { get; private set; }
    public M3_XLuaManager XLuaManager { get; private set; }
    public M3_ModManager ModManager { get; private set; }
    public M3_LocaleManager LocaleManager { get; private set; }
    public M3_PathManager PathManager { get; private set; }
    public M3_PrefabManager PrefabManager { get; private set; }
    public M3_UIManager UIManager { get; private set; }

    private static M3_ManagerHub _Instance;

    static public M3_ManagerHub Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new M3_ManagerHub();
            }

            return _Instance;
        }
    }

    public void Initialize()
    {
        TileManager = new M3_TileManager();
        UnitManager = new M3_UnitManager();
        XLuaManager = new M3_XLuaManager();
        ModManager = new M3_ModManager();
        LocaleManager = new M3_LocaleManager();
        PathManager = new M3_PathManager();
        PrefabManager = new M3_PrefabManager();
        UIManager = new M3_UIManager();

        // First initialize
        InitManager<M3_UIManager>(UIManager);
        InitManager<M3_PrefabManager>(PrefabManager);
        InitManager<M3_PathManager>(PathManager);
        InitManager<M3_XLuaManager>(XLuaManager);

        // Second initialize
        InitManager<M3_LocaleManager>(LocaleManager);
        InitManager<M3_TileManager>(TileManager);
        InitManager<M3_UnitManager>(UnitManager);

        // Third initialize
        InitManager<M3_ModManager>(ModManager);
    }

    public void Destroy()
    {
        // First destroy
        DestroyManager<M3_LocaleManager>(LocaleManager);
        DestroyManager<M3_TileManager>(TileManager);
        DestroyManager<M3_UnitManager>(UnitManager);

        // Second destroy
        DestroyManager<M3_ModManager>(ModManager);
        DestroyManager<M3_PathManager>(PathManager);
        DestroyManager<M3_XLuaManager>(XLuaManager);

        // Third destroy
        DestroyManager<M3_PrefabManager>(PrefabManager);
        DestroyManager<M3_UIManager>(UIManager);
    }

    private void InitManager<T>(T Manager)
        where T : class, M3_IManager, new()
    {
        Manager.Initialize();
        Debug.Log(Manager.ManagerName + " initialized!");
    }

    private void DestroyManager<T>(T Manager)
        where T : class, M3_IManager
    {
        Manager.Destroy();
        Debug.Log(Manager.ManagerName + " destroyed!");
    }
}
