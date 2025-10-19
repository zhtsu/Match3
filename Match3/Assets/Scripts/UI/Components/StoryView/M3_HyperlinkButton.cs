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

    public void OnMouseHoverEvent()
    {
        if (_Text != null)
        {
            _Text.fontStyle = FontStyles.Underline;
        }
    }

    public void OnMouseLeaveEvent()
    {
        if (_Text != null)
        {
            _Text.fontStyle = FontStyles.Normal;
        }
    }
}
