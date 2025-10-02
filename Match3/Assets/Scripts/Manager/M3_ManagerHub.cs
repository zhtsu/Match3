using UnityEngine;

public class M3_ManagerHub
{
    public M3_XLuaManager XLuaManager { get; private set; }
    public M3_DataManager DataManager { get; private set; }
    public M3_PrefabManager PrefabManager { get; private set; }
    public M3_UIManager UIManager { get; private set; }
    public M3_EventManager EventManager { get; private set; }
    public M3_CommandManager CommandManager { get; private set; }

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
        XLuaManager = new M3_XLuaManager();
        DataManager = new M3_DataManager();
        PrefabManager = new M3_PrefabManager();
        UIManager = new M3_UIManager();
        EventManager = new M3_EventManager();
        CommandManager = new M3_CommandManager();

        // First initialize
        InitManager<M3_EventManager>(EventManager);
        InitManager<M3_CommandManager>(CommandManager);

        // Second initialize
        InitManager<M3_UIManager>(UIManager);
        InitManager<M3_PrefabManager>(PrefabManager);
        InitManager<M3_XLuaManager>(XLuaManager);

        // Third initialize
        InitManager<M3_DataManager>(DataManager);
    }

    public void Destroy()
    {
        // First destroy
        DestroyManager<M3_DataManager>(DataManager);

        // Second destroy
        DestroyManager<M3_XLuaManager>(XLuaManager);

        // Third destroy
        DestroyManager<M3_PrefabManager>(PrefabManager);
        DestroyManager<M3_UIManager>(UIManager);
        DestroyManager<M3_CommandManager>(CommandManager);
        DestroyManager<M3_EventManager>(EventManager);
    }

    private void InitManager<T>(T Manager)
        where T : M3_Manager, new()
    {
        Manager.Initialize();
        Debug.Log(Manager.ManagerName + " initialized!");
    }

    private void DestroyManager<T>(T Manager)
        where T : M3_Manager, new()
    {
        Manager.Destroy();
        Debug.Log(Manager.ManagerName + " destroyed!");
    }
}
