using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class M3_SafeAreaEnforcer : MonoBehaviour
{
    [SerializeField]
    private Canvas _TargetCangas;

    private void Start()
    {
        if (_TargetCangas == null)
            return;

        Rect SafeArea = Screen.safeArea;

        RectTransform CanvasRect = _TargetCangas.GetComponent<RectTransform>();
        float ScreenScale = CanvasRect.rect.width / Screen.width;
        float MaxVal = Mathf.Max(SafeArea.x, Screen.width - (SafeArea.x + SafeArea.width)) * ScreenScale;

        RectTransform SelfRect = GetComponent<RectTransform>();
        SelfRect.offsetMin = new Vector2(MaxVal, SelfRect.offsetMin.y);
        SelfRect.offsetMax = new Vector2(-MaxVal, SelfRect.offsetMax.y);
    }
}
