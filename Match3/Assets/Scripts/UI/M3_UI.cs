using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum M3_UIType
{
    None,
    MainMenu,
    InGameHUD,
    PauseMenu,
    Config,
    LoadingScreen,
    Mod,
    StoryView,
    ModSelect,
}

public class M3_UI : MonoBehaviour
{
    [SerializeField]
    private M3_UIType _Type;

    public M3_UIType Type { get { return _Type; } }
}
