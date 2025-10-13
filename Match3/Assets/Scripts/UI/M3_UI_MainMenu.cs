using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class M3_UI_MainMenu : M3_UI
{
    [SerializeField]
    private Button _StartButton;

    [SerializeField]
    private Button _MODButton;

    [SerializeField]
    private Button _ConfigButton;

    [SerializeField]
    private Button _ExitButton;

    [SerializeField]
    private VerticalLayoutGroup _ButtonBox;

    [SerializeField]
    private Image _HoverEffect;

    private Button _CurrentHoveredButton = null;

    private void Start()
    {
        _CurrentHoveredButton = _StartButton;
    }

    private void SetHoverEffect(Button InButton)
    {
        SetButtonTextColor(_CurrentHoveredButton, 1);
        _CurrentHoveredButton = InButton;
        SetButtonTextColor(_CurrentHoveredButton, 0);

        VerticalLayoutGroup VLG = _ButtonBox.GetComponentInParent<VerticalLayoutGroup>();
        Vector3 VLGPos = VLG.transform.position;

        RectTransform BRT = InButton.GetComponent<RectTransform>();
        _HoverEffect.rectTransform.position = BRT.position;
        _HoverEffect.rectTransform.sizeDelta = BRT.sizeDelta;
    }

    private void SetButtonTextColor(Button InButton, int InColor)
    {
        if (InButton == null)
            return;

        TextMeshProUGUI[] ObjList = InButton.GetComponentsInChildren<TextMeshProUGUI>();
        if (ObjList.Length == 0)
            return;

        TextMeshProUGUI TextObj = ObjList[0];
        if (InColor == 0)
            TextObj.color = Color.black;
        else if (InColor == 1)
            TextObj.color = Color.white;
    }

    public void OnStartButtonHover()
    {
        SetHoverEffect(_StartButton);
    }

    public void OnMODButtonHover()
    {
        SetHoverEffect(_MODButton);
    }

    public void OnConfigButtonHover()
    {
        SetHoverEffect(_ConfigButton);
    }

    public void OnExitButtonHover()
    {
        SetHoverEffect(_ExitButton);
    }
}
