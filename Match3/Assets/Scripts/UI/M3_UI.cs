using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum M3_UIType
{
    None,
    MainMenu,
    InGameHUD,
    PauseMenu,
    Config,
    LoadingScreen,
    Mod,
    StoryView,
    ModSelect,
}

public abstract class M3_UIParams
{
}

public class M3_UI : MonoBehaviour
{
    [SerializeField]
    private M3_UIType _Type;
    public M3_UIType Type { get { return _Type; } }

    private M3_UIParams _Params;
    public void SetParams(M3_UIParams Params)
    {
        _Params = Params;
    }
    protected M3_UIParams GetParams()
    {
        return _Params;
    }
}
