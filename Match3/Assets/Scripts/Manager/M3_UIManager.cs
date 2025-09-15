using System;
using UnityEngine;

public class M3_UIManager : M3_IManager
{
    public string ManagerName
    {
        get { return "UIManager"; }
    }

    private M3_UIRoot _UIRoot;

    public void Initialize()
    {
        _UIRoot = GameObject.FindObjectOfType<M3_UIRoot>();
    }

    public void Destroy()
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
}
