using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class M3_UI_MainMenu : M3_UI
{
    [SerializeField]
    private Button _StartButton;

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
        M3_CommonHelper.SetButtonTextColor(_CurrentHoveredButton, M3_ColorType.White);
        _CurrentHoveredButton = InButton;
        M3_CommonHelper.SetButtonTextColor(_CurrentHoveredButton, M3_ColorType.Black);

        VerticalLayoutGroup VLG = _ButtonBox.GetComponentInParent<VerticalLayoutGroup>();
        Vector3 VLGPos = VLG.transform.position;

        RectTransform BRT = InButton.GetComponent<RectTransform>();
        _HoverEffect.rectTransform.position = BRT.position;
        _HoverEffect.rectTransform.sizeDelta = BRT.sizeDelta;
    }

    private void SetPressEffect(Button InButton)
    {
        M3_CommonHelper.SetImageColor(_HoverEffect, M3_ColorType.Cyan);
    }

    private void SetReleaseEffect(Button InButton)
    {
        M3_CommonHelper.SetImageColor(_HoverEffect, M3_ColorType.White);
    }

    public void OnStartButtonHover()
    {
        SetHoverEffect(_StartButton);
    }

    public void OnStartButtonPress()
    {
        SetPressEffect(_StartButton);
    }

    public void OnStartButtonRelease()
    {
        SetReleaseEffect(_StartButton);

        M3_CommonHelper.CloseUI(M3_UIType.MainMenu);
        M3_StoryViewUIParams UIParams = new M3_StoryViewUIParams();
        M3_ManagerHub.Instance.DataManager.GetModData("default", out M3_ModData ModData);
        UIParams.ModData = ModData;
        M3_CommonHelper.OpenUI(M3_UIType.StoryView, UIParams);

        //M3_UIManager UIManager = M3_ManagerHub.Instance.UIManager;
        //UIManager.OpenUI(M3_UIType.ModSelect);
    }

    public void OnConfigButtonHover()
    {
        SetHoverEffect(_ConfigButton);
    }

    public void OnConfigButtonPress()
    {
        SetPressEffect(_ConfigButton);
    }

    public void OnConfigButtonRelease()
    {
        SetReleaseEffect(_ConfigButton);

        M3_UIManager UIManager = M3_ManagerHub.Instance.UIManager;
        UIManager.OpenUI(M3_UIType.Config);
    }

    public void OnExitButtonHover()
    {
        SetHoverEffect(_ExitButton);
    }

    public void OnExitButtonPress()
    {
        SetPressEffect(_ExitButton);
    }

    public void OnExitButtonRelease()
    {
        SetReleaseEffect(_ExitButton);

        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }
}
