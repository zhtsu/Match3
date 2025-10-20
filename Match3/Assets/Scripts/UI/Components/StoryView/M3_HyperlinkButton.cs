using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class M3_HyperlinkButton : M3_UIComponent
{
    [SerializeField]
    private Button _Button;
    [SerializeField]
    private TextMeshProUGUI _Text;

    private string _ChoosePath;

    public void SetText(string Str)
    {
        if (_Text != null)
        {
            _Text.text = Str;
        }
    }

    public void SetChoosePath(string ChoosePath)
    {
        _ChoosePath = ChoosePath;
    }

    public string GetChoosePath()
    {
        return _ChoosePath;
    }

    public void SetButtonClickAction(Action<string> Func)
    {
        if (_Button != null)
        {
            _Button.onClick.AddListener(() => { Func?.Invoke(_ChoosePath); });
        }
    }
}
