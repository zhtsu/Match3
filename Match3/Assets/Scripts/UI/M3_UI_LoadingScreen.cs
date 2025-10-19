using PrimeTween;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class M3_LoadingScreenUIParams : M3_UIParams
{
    public Action OnStartLoading;
}

public class M3_UI_LoadingScreen : M3_UI
{
    [SerializeField]
    TextMeshProUGUI _LoadingText;

    private void Start()
    {
        M3_EventBus.Subscribe<M3_Event_LoadingCompleted>(OnLoadingCompleted);

        Tween FadeOut = Tween.Alpha(_LoadingText, 1f, 0.1f, 1f);
        Tween FadeIn = Tween.Alpha(_LoadingText, 0.1f, 1f, 1f);
        Sequence.Create()
            .Chain(FadeOut)
            .Chain(FadeIn)
            .SetRemainingCycles(-1);

        M3_LoadingScreenUIParams Params = (M3_LoadingScreenUIParams)GetParams();
        if (Params != null)
        {
            Params.OnStartLoading?.Invoke();
        }
    }

    private void OnLoadingCompleted(M3_Event_LoadingCompleted Event)
    {
        M3_EventBus.Unsubscribe<M3_Event_LoadingCompleted>(OnLoadingCompleted);

        M3_CommonHelper.CloseUI(M3_UIType.LoadingScreen);
    }
}
