using System;
using System.Collections.Generic;
using UnityEngine;

public enum M3_UILayerType
{
    Bottom,
    Main,
    Top,
    Popup,
    Max
}

public class M3_UIRoot : MonoBehaviour
{
    [SerializeField]
    private GameObject _BottomLayer;
    [SerializeField]
    private GameObject _MainLayer;
    [SerializeField]
    private GameObject _TopLayer;
    [SerializeField]
    private GameObject _PopupLayer;
    [SerializeField]
    private GameObject _LoadingScreenPrefab;

    private List<GameObject> _ActiveUIList = new List<GameObject>();

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    private void OnDestroy()
    {
    }

    public void OpenUI(M3_UIType TargetUIType, M3_UIParams Params, M3_UILayerType Layer)
    {
        if (M3_GameController.Instance.GlobalData.IsGameReady == false &&
            TargetUIType != M3_UIType.LoadingScreen)
        {
            Debug.LogWarning("Cannot open UI before the game is ready, except for LoadingScreen.");
            return;
        }

        GameObject UIPrefab = null;
        if (TargetUIType == M3_UIType.LoadingScreen)
            UIPrefab = _LoadingScreenPrefab;
        else
            UIPrefab = M3_CommonHelper.GetUIPrefab(TargetUIType);

        if (UIPrefab == null)
        {
            Debug.LogWarning($"Cannot open UI {TargetUIType.ToString()}: Invalid prefab");
            return;
        }

        GameObject TargetUI = Instantiate(UIPrefab);

        M3_UI UIComp = TargetUI.GetComponent<M3_UI>();
        UIComp.SetParams(Params);

        RectTransform Rect = TargetUI.GetComponent<RectTransform>();
        M3_CommonHelper.SetFullStretch(Rect);

        switch (Layer)
        {
            case M3_UILayerType.Bottom:
                TargetUI.transform.SetParent(_BottomLayer.transform, false);
                _ActiveUIList.Add(TargetUI);
                break;
            case M3_UILayerType.Main:
                TargetUI.transform.SetParent(_MainLayer.transform, false);
                _ActiveUIList.Add(TargetUI);
                break;
            case M3_UILayerType.Top:
                TargetUI.transform.SetParent(_TopLayer.transform, false);
                _ActiveUIList.Add(TargetUI);
                break;
            case M3_UILayerType.Popup:
                TargetUI.transform.SetParent(_PopupLayer.transform, false);
                _ActiveUIList.Add(TargetUI);
                break;
            default:
                throw new ArgumentOutOfRangeException("Layer", Layer, null);
        }
    }

    public void CloseUI(M3_UIType TargetUIType)
    {
        foreach (GameObject Obj in _ActiveUIList)
        {
            M3_UI UIComponent = Obj.GetComponent<M3_UI>();
            if (UIComponent != null && UIComponent.Type == TargetUIType)
            {
                _ActiveUIList.Remove(Obj);
                Destroy(Obj);
                break;
            }
        }
    }

    public void ShowUI(M3_UIType TargetUIType)
    {
        foreach (GameObject Obj in _ActiveUIList)
        {
            M3_UI UIComponent = Obj.GetComponent<M3_UI>();
            if (UIComponent != null && UIComponent.Type == TargetUIType)
            {
                Obj.SetActive(true);
                break;
            }
        }
    }

    public void HideUI(M3_UIType TargetUIType)
    {
        foreach (GameObject Obj in _ActiveUIList)
        {
            M3_UI UIComponent = Obj.GetComponent<M3_UI>();
            if (UIComponent != null && UIComponent.Type == TargetUIType)
            {
                Obj.SetActive(false);
                break;
            }
        }
    }

    public void CloseAllUI()
    {
        foreach (GameObject Obj in _ActiveUIList)
        {
            Destroy(Obj);
        }

        _ActiveUIList.Clear();
    }

    public bool IsUIActive(M3_UIType CheckedUIType)
    {
        foreach (GameObject Obj in _ActiveUIList)
        {
            M3_UI UIComponent = Obj.GetComponent<M3_UI>();
            if (UIComponent != null && UIComponent.Type == CheckedUIType)
            {
                return true;
            }
        }

        return false;
    }
}
