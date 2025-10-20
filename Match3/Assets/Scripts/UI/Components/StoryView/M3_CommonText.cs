using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class M3_CommonText : M3_UIComponent
{
    [SerializeField]
    TextMeshProUGUI _Text;

    public void SetText(string Str)
    {
        if (_Text != null)
        {
            _Text.text = Str;
        }
    }
}
