using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class M3_ModCard : M3_UIComponent
{
    [SerializeField]
    private Image _CheckedEffectImage;
    [SerializeField]
    private Image _CoverImage;
    [SerializeField]
    private TextMeshProUGUI _ModNameText;
    [SerializeField]
    private Button _Button;

    private M3_ModData _ModData;
    public M3_ModData ModData { get { return _ModData; } }

    public void SetModData(M3_ModData ModData)
    {
        _ModData = ModData;

        if (_ModNameText != null)
        {
            string ModNameStr = M3_CommonHelper.GetGameString(ModData.Id, "zh", ModData.Name);
            _ModNameText.text = ModNameStr;
        }

        if (_CoverImage != null)
        {
            _CoverImage.sprite = M3_CommonHelper.GetSprite(ModData.CoverImage);
        }
    }

    public void SetButtonClickAction(UnityEngine.Events.UnityAction InAction)
    {
        if (_Button == null)
            return;

        _Button.onClick.RemoveAllListeners();
        _Button.onClick.AddListener(InAction);
    }

    public void SetChecked(bool Checked)
    {
        if (_CheckedEffectImage == null)
            return;

        if (Checked)
        {
            _CheckedEffectImage.gameObject.SetActive(true);
        }
        else
        {
            _CheckedEffectImage.gameObject.SetActive(false);
        }
    }
}
