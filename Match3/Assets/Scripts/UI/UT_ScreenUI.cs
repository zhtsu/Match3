using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UT_EScreenUIType
{
    None,
    LoadingScreen,
    MainMenu,
    Config,
}

public class UT_ScreenUI : UT_UI
{
    [SerializeField]
    private UT_EScreenUIType _Type;
    public UT_EScreenUIType Type { get { return _Type; } }

    private UT_UIParams _Params;

    public void SetParams(UT_UIParams Params)
    {
        _Params = Params;
    }

    protected UT_UIParams GetParams()
    {
        return _Params;
    }
}
