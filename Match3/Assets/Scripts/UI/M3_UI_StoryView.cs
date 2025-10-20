using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class M3_StoryViewUIParams : M3_UIParams
{
    public M3_ModData ModData;
}

public class M3_UI_StoryView : M3_UI
{
    [SerializeField]
    private VerticalLayoutGroup _StoryPanel;

    private Ink.Runtime.Story _Story;

    private void Start()
    {
        M3_StoryViewUIParams Params = (M3_StoryViewUIParams)GetParams();
        _Story = M3_CommonHelper.GetStory(Params.ModData.MainInkFile);

        RefreshStoryPanel();
    }

    private void RefreshStoryPanel()
    {
        if (_StoryPanel == null)
            return;

        while (_StoryPanel.transform.childCount > 0)
        {
            Transform Child = _StoryPanel.transform.GetChild(0);
            Child.SetParent(null);
            Destroy(Child.gameObject);
        }

        if (_Story == null)
            return;

        string StoryStr = "";
        while (_Story.canContinue)
        {
            StoryStr += _Story.Continue();
        }

        M3_CommonText StoryText = SpawnCommonText(StoryStr);
        if (StoryText != null)
        {
            StoryText.transform.SetParent(_StoryPanel.transform, false);
        }

        List<Ink.Runtime.Choice> Choices = _Story.currentChoices;
        foreach (Ink.Runtime.Choice Choice in Choices)
        {
            M3_HyperlinkButton ChoiceButton = SpawnHyperlinkButton(Choice);
            if (ChoiceButton != null)
            {
                ChoiceButton.transform.SetParent(_StoryPanel.transform, false);
            }
        }
    }

    public M3_CommonText SpawnCommonText(string Str)
    {
        GameObject CommonTextPrefab = M3_ManagerHub.Instance.PrefabManager.GetPrefab("CommonText");
        if (CommonTextPrefab != null)
        {
            M3_CommonText CommonText = GameObject.Instantiate(CommonTextPrefab).GetComponent<M3_CommonText>();
            if (CommonText != null)
            {
                CommonText.SetText(Str);
            }

            return CommonText;
        }

        return null;
    }

    public M3_HyperlinkButton SpawnHyperlinkButton(Ink.Runtime.Choice Choice)
    {
        GameObject HBPrefab = M3_ManagerHub.Instance.PrefabManager.GetPrefab("HyperlinkButton");
        if (HBPrefab != null)
        {
            M3_HyperlinkButton Button = GameObject.Instantiate(HBPrefab).GetComponent<M3_HyperlinkButton>();
            Button.SetText(Choice.text);
            Button.SetChoosePath(Choice.pathStringOnChoice);
            Button.SetButtonClickAction(OnChoiceSelected);

            return Button;
        }

        return null;
    }

    private void OnChoiceSelected(string ChoosePathStr)
    {
        _Story.ChoosePathString(ChoosePathStr);

        RefreshStoryPanel();
    }
}
