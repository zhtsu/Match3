using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UT_EUILayer
{
    None,
    Bottom,
    Main,
    Top,
    Popup,
}

public abstract class UT_UIParams
{

}

public abstract class UT_UI : MonoBehaviour
{
    protected bool Visible;

    virtual public void Initialize<T>()
        where T : UT_UIParams
    {
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    virtual public void OnShow()
    {
        Visible = true;
    }

    virtual public void OnHide()
    {
        Visible = false;
    }

    virtual public void OnOpen()
    {
        Visible = true;
    }

    virtual public void OnClose()
    {
        Visible = false;
    }
}
