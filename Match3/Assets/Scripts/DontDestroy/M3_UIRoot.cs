using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
    private List<M3_UIType> _UITypeList;
    [SerializeField]
    private List<GameObject> _UIPrefabList;

    private Dictionary<M3_UIType, GameObject> _UIPrefabDict = new Dictionary<M3_UIType, GameObject>();

    private void Awake()
    {
        if (_UITypeList.Count == _UIPrefabList.Count)
        {
            _UIPrefabDict.Clear();
            for (int i = 0; i < _UITypeList.Count; i++)
            {
                if (!_UIPrefabDict.ContainsKey(_UITypeList[i]))
                {
                    _UIPrefabDict.Add(_UITypeList[i], _UIPrefabList[i]);
                }
            }
        }
        else
        {
            throw new Exception("UITypeList and UIPrefabList must have the same count.");
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    private void OnDestroy()
    {
    }

    public void OpenUI(M3_UIType TargetUIType, M3_UILayerType Layer)
    {
        if (_UIPrefabDict.TryGetValue(TargetUIType, out GameObject UIPrefab))
        {
            if (UIPrefab == null)
                return;

            GameObject TargetUI = Instantiate(UIPrefab);

            switch (Layer)
            {
                case M3_UILayerType.Bottom:
                    TargetUI.transform.SetParent(_BottomLayer.transform, false);
                    break;
                case M3_UILayerType.Main:
                    TargetUI.transform.SetParent(_MainLayer.transform, false);
                    break;
                case M3_UILayerType.Top:
                    TargetUI.transform.SetParent(_TopLayer.transform, false);
                    break;
                case M3_UILayerType.Popup:
                    TargetUI.transform.SetParent(_PopupLayer.transform, false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Layer", Layer, null);
            }
        }
    }
}
