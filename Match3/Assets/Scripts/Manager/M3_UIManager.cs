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
}
