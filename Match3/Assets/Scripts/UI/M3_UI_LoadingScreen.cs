using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class M3_UI_LoadingScreen : M3_UI
{
    [SerializeField]
    TextMeshProUGUI _LoadingText;

    private void Start()
    {
        Tween FadeOut = Tween.Alpha(_LoadingText, 1f, 0.1f, 1f);
        Tween FadeIn = Tween.Alpha(_LoadingText, 0.1f, 1f, 1f);
        Sequence.Create()
            .Chain(FadeOut)
            .Chain(FadeIn)
            .SetRemainingCycles(-1);
    }
}
