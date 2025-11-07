using UnityEngine;
using System.Collections.Generic;
using XLua;

public class UT_ScriptManager : UT_Manager
{
    public override string ManagerName => "Script Manager";

    private XLua.LuaEnv _LuaEnv { get; set; }

    public override void Initialize()
    {
        _LuaEnv = new XLua.LuaEnv();
    }

    public override void Destroy()
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
            Debug.LogError($"Fail to get lua table! Error: {Err.Message}");
        }

        return null;
    }
}
