using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using XLua;

public class M3_ScriptManager : M3_Manager
{
    public override string ManagerName
    {
        get { return "Script Manager"; }
    }

    private XLua.LuaEnv _LuaEnv { get; set; }
    private Dictionary<string, LuaTable> _LuaTableCache = new Dictionary<string, LuaTable>();

    public override void Initialize()
    {
        _LuaEnv = new XLua.LuaEnv();

        M3_EventBus.Subscribe<M3_Event_ScriptsReadCompleted>(OnScriptsReadCompleted);
    }

    public override void Destroy()
    {
        _LuaEnv.Dispose();

        M3_EventBus.Unsubscribe<M3_Event_ScriptsReadCompleted>(OnScriptsReadCompleted);
    }

    private void OnScriptsReadCompleted(M3_Event_ScriptsReadCompleted Event)
    {
        LoadLuaScriptsAsync(Event.ScriptList).ContinueWith((Task<bool> LoadTask) =>
        {
            M3_EventBus.SendEvent<M3_Event_ScriptsLoadCompleted>();
        });
    }

    private async Task<bool> LoadLuaScriptsAsync(List<string> ScriptPaths)
    {
        List<Task<bool>> LoadTasks = new List<Task<bool>>();
        foreach (string ScriptPath in ScriptPaths)
        {
            LoadTasks.Add(LoadLuaTableAsync(ScriptPath));
        }

        bool[] Results = await Task.WhenAll(LoadTasks);
        foreach (bool Result in Results)
        {
            if (!Result)
            {
                return false;
            }
        }

        Debug.Log("[ScriptManager] All Lua scripts loaded successfully.");
        return true;
    }

    private async Task<string> ReadLuaScriptAsync(string ScriptPath)
    {
        try
        {
            return await Task.Run(() => File.ReadAllText(ScriptPath));
        }
        catch (FileNotFoundException)
        {
            Debug.LogError($"[ScriptManager] Lua script not found: {ScriptPath}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"[ScriptManager] Failed to read lua script: {ScriptPath}: {ex.Message}");
        }

        return null;
    }

    private async Task<bool> LoadLuaTableAsync(string ScriptPath)
    {
        if (_LuaTableCache.ContainsKey(ScriptPath))
        {
            Debug.LogWarning($"[ScriptManager] Duplicated script ID: {ScriptPath}");
            return false;
        }

        if (!File.Exists(ScriptPath))
        {
            Debug.LogWarning($"[ScriptManager] {ScriptPath} no exist!");
            return false;
        }

        string LuaCode = await ReadLuaScriptAsync(ScriptPath);
        if (string.IsNullOrEmpty(LuaCode))
        {
            return false;
        }

        LuaTable Table = ReturnLuaTable(LuaCode);
        if (Table != null)
        {
            if (_LuaTableCache.ContainsKey(ScriptPath))
            {
                Debug.Log($"[ScriptManager] Lua script updated: {ScriptPath}");
                _LuaTableCache[ScriptPath] = Table;
            }
            else
            {
                _LuaTableCache.Add(ScriptPath, Table);
            }

            Debug.Log($"[ScriptManager] Lua script loaded: {ScriptPath}");
            return true;
        }

        return false;
    }

    private LuaTable ReturnLuaTable(string LuaCode)
    {
        try
        {
            return _LuaEnv.DoString(LuaCode)[0] as LuaTable;
        }
        catch (System.Exception Err)
        {
            Debug.LogError($"Fail to get lua table\n" +
                $"Error: {Err.Message}");
        }

        return null;
    }

    public T GetLuaValue<K, T>(LuaTable Table, K Key)
    {
        try
        {
            return Table.Get<K, T>(Key);
        }
        catch (System.Exception Err)
        {
            Debug.LogError($"Fail to get lua value\n" +
                $"Type: {typeof(T)} Key: {Key} Error: {Err.Message}");
        }

        return default(T);
    }

    public T[] GetLuaArray<T>(LuaTable Table, string Key)
    {
        try
        {
            object LuaObj = Table.Get<object>(Key);
            if (!(LuaObj is LuaTable LuaList))
                return new T[0];

            List<T> ValueList = new List<T>();
            foreach (int K in LuaList.GetKeys<int>())
            {
                T Val = GetLuaValue<int, T>(LuaList, K);
                ValueList.Add(Val);
            }

            return ValueList.ToArray();
        }
        catch (System.Exception Err)
        {
            Debug.LogError($"Fail to get lua array\n" +
                $"Type: {typeof(T)} Key: {Key} Error: {Err.Message}");
        }

        return new T[0];
    }

    public bool GetScript(string ScriptId, out LuaTable OutLuaTable)
    {
        if (string.IsNullOrEmpty(ScriptId))
        {
            OutLuaTable = null;
            return false;
        }

        if (_LuaTableCache.TryGetValue(ScriptId, out LuaTable TempLuaTable))
        {
            OutLuaTable = TempLuaTable;
            return true;
        }

        OutLuaTable = null;
        return false;
    }
}
