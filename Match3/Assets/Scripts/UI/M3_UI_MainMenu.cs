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
    private Button _ModButton;

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

    private void SetPressEffect(Button InButton)
    {
        if (_HoverEffect != null && ColorUtility.TryParseHtmlString("#D6D6D6", out Color MyGray))
        {
            _HoverEffect.GetComponent<Image>().color = MyGray;
        }
    }

    private void SetReleaseEffect(Button InButton)
    {
        if (_HoverEffect != null)
        {
            _HoverEffect.GetComponent<Image>().color = Color.white;
        }
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

    public void OnStartButtonPress()
    {
        SetPressEffect(_StartButton);
    }

    public void OnStartButtonRelease()
    {
        SetReleaseEffect(_StartButton);
    }

    public void OnModButtonHover()
    {
        SetHoverEffect(_ModButton);
    }

    public void OnModButtonPress()
    {
        SetPressEffect(_ModButton);
    }

    public void OnModButtonRelease()
    {
        SetReleaseEffect(_ModButton);

        M3_UIManager UIManager = M3_ManagerHub.Instance.UIManager;
        UIManager.OpenUI(M3_UIType.Mod);
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
