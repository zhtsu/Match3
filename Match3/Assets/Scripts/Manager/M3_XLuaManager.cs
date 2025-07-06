using UnityEngine;
using System.Collections.Generic;
using XLua;

public class M3_XLuaManager : M3_IManager
{
    public string ManagerName
    {
        get { return "XLuaManager"; }
    }

    private XLua.LuaEnv _LuaEnv { get; set; }

    public void Initialize()
    {
        _LuaEnv = new XLua.LuaEnv();
    }

    public void Destroy()
    {
        _LuaEnv.Dispose();
    }

    public LuaTable ReturnLuaTable(string LuaCode)
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
}
