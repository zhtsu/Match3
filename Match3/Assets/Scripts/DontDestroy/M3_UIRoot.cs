using System;
using UnityEngine;

public class M3_UIRoot : MonoBehaviour
{
    [SerializeField]
    private GameObject BottomLayer;
    [SerializeField]
    private GameObject MainLayer;
    [SerializeField]
    private GameObject TopLayer;
    [SerializeField]
    private GameObject PopupLayer;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    private void OnDestroy()
    {
    }
}
