using System;
using UnityEngine;

public class M3_UIManager : M3_Manager
{
    public override string ManagerName
    {
        get { return "UI Manager"; }
    }

    private M3_UIRoot _UIRoot;

    public override void Initialize()
    {
        _UIRoot = GameObject.FindObjectOfType<M3_UIRoot>();
    }

    public override void Destroy()
    {

    }

    public void OpenUI(M3_UIType TargetUIType, M3_UILayerType Layer = M3_UILayerType.Main)
    {
        if (_UIRoot != null)
        {
            _UIRoot.OpenUI(TargetUIType, Layer);
        }
        else
        {
            throw new Exception("UIRoot is not found in the scene.");
        }
    }

    public void CloseUI(M3_UIType TargetUIType)
    {
        if (_UIRoot != null)
        {
            _UIRoot.CloseUI(TargetUIType);
        }
        else
        {
            throw new Exception("UIRoot is not found in the scene.");
        }
    }

    public bool IsUIActive(M3_UIType TargetUIType)
    {
        if (_UIRoot != null)
        {
            return _UIRoot.IsUIActive(TargetUIType);
        }
        else
        {
            return false;
        }
    }
}
