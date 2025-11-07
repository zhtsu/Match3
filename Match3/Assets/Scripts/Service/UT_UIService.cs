using System;
using System.Collections.Generic;
using UnityEngine;



public class UT_UIService : MonoBehaviour, UT_IUIService
{
    private UT_UIManager _UIManager;

    public void Initialize(UT_UIManager InUIManager)
    {
        _UIManager = InUIManager;
    }

    public void CloseAllScreenUI()
    {
        _UIManager?.CloseAllScreenUI();
    }

    public void CloseScreenUI(UT_EScreenUIType ScreenUIType)
    {
        _UIManager?.CloseScreenUI(ScreenUIType);
    }

    public void HideScreenUI(UT_EScreenUIType ScreenUIType)
    {
        _UIManager?.HideScreenUI(ScreenUIType);
    }

    public bool IsScreenUIActive(UT_EScreenUIType ScreenUIType)
    {
        return _UIManager.IsScreenUIActive(ScreenUIType);
    }

    public void OpenScreenUI(UT_EScreenUIType ScreenUIType, UT_EUILayer Layer = UT_EUILayer.Main, UT_UIParams Params = null)
    {
        _UIManager?.OpenScreenUI(ScreenUIType, Layer, Params);
    }

    public void ShowScreenUI(UT_EScreenUIType ScreenUIType)
    {
        _UIManager?.ShowScreenUI(ScreenUIType);
    }

    public void OpenScreenUI(UT_ScreenUI ScreenUI, UT_EUILayer Layer = UT_EUILayer.Main, UT_UIParams Params = null)
    {
        _UIManager?.OpenScreenUI(ScreenUI, Layer, Params);
    }

    public void CloseScreenUI(UT_ScreenUI ScreenUI)
    {
        _UIManager?.CloseScreenUI(ScreenUI);
    }
}
