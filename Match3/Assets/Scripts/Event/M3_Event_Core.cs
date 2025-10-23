using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M3_Event_PrefabsLoadCompleted : M3_Event { }
public class M3_Event_TexturesLoadCompleted : M3_Event { }
public class M3_Event_StoriesLoadCompleted : M3_Event { }
public class M3_Event_ScriptsLoadCompleted : M3_Event { }
public class M3_Event_ManagerHubReady : M3_Event { }
public class M3_Event_GameReady : M3_Event { }
public class M3_Event_LoadingCompleted : M3_Event { }

public class M3_Event_ScriptsReadCompleted : M3_Event
{
    public List<string> ScriptList;

    public M3_Event_ScriptsReadCompleted(List<string> InScriptList)
    {
        ScriptList = InScriptList;
    }
}

public class M3_Event_TexturesReadCompleted : M3_Event
{
    public List<string> TextureList;

    public M3_Event_TexturesReadCompleted(List<string> InTextureList)
    {
        TextureList = InTextureList;
    }
}

public class M3_Event_StoriesReadCompleted : M3_Event
{
    public List<string> MainInkFileList;

    public M3_Event_StoriesReadCompleted(List<string> InMainInkFileList)
    {
        MainInkFileList = InMainInkFileList;
    }
}

public class M3_Event_BattleControllerChanged : M3_Event
{
    public M3_ControllerType NewControllerType;
    public M3_Event_BattleControllerChanged(M3_ControllerType InNewControllerType)
    {
        NewControllerType = InNewControllerType;
    }
}

public class M3_Event_GemSwapped : M3_Event
{
}
