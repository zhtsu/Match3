using System;
using UnityEngine;

public interface UT_IUIService
{
    public void OpenScreenUI(UT_EScreenUIType ScreenUIType, UT_EUILayer Layer, UT_UIParams Params);
    public void CloseScreenUI(UT_EScreenUIType ScreenUIType);
    public void OpenScreenUI(UT_ScreenUI ScreenUI, UT_EUILayer Layer, UT_UIParams Params);
    public void CloseScreenUI(UT_ScreenUI ScreenUI);
    public void ShowScreenUI(UT_EScreenUIType ScreenUIType);
    public void HideScreenUI(UT_EScreenUIType ScreenUIType);
    public void CloseAllScreenUI();
    public bool IsScreenUIActive(UT_EScreenUIType ScreenUIType);
}
