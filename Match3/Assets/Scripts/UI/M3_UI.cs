using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum M3_UIType
{
    MainMenu,
    InGameHUD,
    PauseMenu,
    Settings,
    LoadingScreen,
    ModManager,
    Max
}

public class M3_UI : MonoBehaviour
{
    [SerializeField]
    private M3_UIType _Type;

    public M3_UIType Type { get { return _Type; } }

    private void Start()
    {

    }
}
