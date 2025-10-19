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
    private Ink.Runtime.Story _Story;

    private void Start()
    {
        M3_StoryViewUIParams Params = (M3_StoryViewUIParams)GetParams();
        _Story = M3_CommonHelper.GetStory(Params.ModData.MainInkFile);

        RefreshStoryPanel();
    }

    private void RefreshStoryPanel()
    {
        if (_Story == null)
            return;

        if (_Story.canContinue)
        {
            string StoryText = _Story.Continue();
            Debug.Log(StoryText);
        }
        else
        {
            List<Ink.Runtime.Choice> Choices = _Story.currentChoices;
            if (Choices.Count > 0)
            {

            }
        }
    }
}
