using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class M3_UI_Match3Battle : M3_UI
{
    [SerializeField]
    private Image _TurnEndedPanel;
    [SerializeField]
    private TextMeshProUGUI _TurnNameText;
    [SerializeField]
    private TextMeshProUGUI _PlayerHPText;
    [SerializeField]
    private TextMeshProUGUI _AIHPText;
    [SerializeField]
    private TextMeshProUGUI _PlayerDamageText;
    [SerializeField]
    private TextMeshProUGUI _AIDamageText;
    [SerializeField]
    private Image _PlayerHPImage;
    [SerializeField]
    private Image _AIHPImage;

    private void Start()
    {
        M3_EventBus.Subscribe<M3_Event_BattleControllerChanged>(OnBattleControllerChanged);
        M3_EventBus.Subscribe<M3_Event_Match3Damage>(OnMatch3Damage);

        if (_TurnEndedPanel != null)
            _TurnEndedPanel.gameObject.SetActive(false);
        if (_PlayerDamageText != null)
            _PlayerDamageText.gameObject.SetActive(false);
        if (_AIDamageText != null)
            _AIDamageText.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        M3_EventBus.Unsubscribe<M3_Event_BattleControllerChanged>(OnBattleControllerChanged);
        M3_EventBus.Unsubscribe<M3_Event_Match3Damage>(OnMatch3Damage);
    }

    private void UpdateHPWithDamage(Image HPImage, TextMeshProUGUI HPText, int DamageValue)
    {
        if (HPImage == null)
            return;
        if (HPText == null)
            return;

        int OldHP = int.Parse(HPText.text);
        int NewHP = OldHP - DamageValue;
        int DeltaHP = OldHP - NewHP;

        float Delta = 0.0f;
        if (OldHP != 0)
            Delta = (float)DeltaHP / (float)OldHP;

        float OldHPImageWidth = HPImage.rectTransform.sizeDelta.x;
        float NewHPImageWidth = OldHPImageWidth - (OldHPImageWidth * Delta);

        HPImage.rectTransform.sizeDelta = new Vector2(NewHPImageWidth, HPImage.rectTransform.sizeDelta.y);
        HPText.text = NewHP.ToString();
    }

    private void OnMatch3Damage(M3_Event_Match3Damage Event)
    {
        if (Event.ControllerType == M3_ControllerType.Player)
        {
            if (_AIDamageText != null)
            {
                _AIDamageText.gameObject.SetActive(true);
                _AIDamageText.text = "-" + Event.DamageValue.ToString();

                Vector3 StartRot = new Vector3(0, 0, -90);
                Vector3 EndRot = Vector3.zero;

                Tween Tw1 = Tween.LocalRotation(_AIDamageText.transform, StartRot, EndRot, 0.25f);
                Tween Tw2 = Tween.Scale(_AIDamageText.transform, 0, 1, 0.25f);
                Tween Tw3 = Tween.Delay(1.0f);

                Sequence Seq = Sequence.Create()
                    .Group(Tw1).Group(Tw2).Chain(Tw3).ChainCallback(() =>
                    {
                        _AIDamageText.gameObject.SetActive(false);
                        M3_EventBus.SendEvent<M3_Event_HPUIUpdated>();
                    });

                UpdateHPWithDamage(_AIHPImage, _AIHPText, Event.DamageValue);
            }
        }
        else
        {
            if (_PlayerDamageText != null)
            {
                _PlayerDamageText.gameObject.SetActive(true);
                _PlayerDamageText.text = "-" + Event.DamageValue.ToString();

                Vector3 StartRot = new Vector3(0, 0, 90);
                Vector3 EndRot = Vector3.zero;

                Tween Tw1 = Tween.LocalRotation(_PlayerDamageText.transform, StartRot, EndRot, 0.25f);
                Tween Tw2 = Tween.Scale(_PlayerDamageText.transform, 0, 1, 0.25f);
                Tween Tw3 = Tween.Delay(1.0f);

                Sequence Seq = Sequence.Create()
                    .Group(Tw1).Group(Tw2).Chain(Tw3).ChainCallback(() =>
                    {
                        _PlayerDamageText.gameObject.SetActive(false);
                        M3_EventBus.SendEvent<M3_Event_HPUIUpdated>();
                    });

                UpdateHPWithDamage(_PlayerHPImage, _PlayerHPText, Event.DamageValue);
            }
        }
    }

    private void OnBattleControllerChanged(M3_Event_BattleControllerChanged Event)
    {
        if (_TurnEndedPanel)
            _TurnEndedPanel.gameObject.SetActive(true);
        if (_TurnNameText)
        {
            _TurnNameText.transform.localPosition = new Vector3(0, 800, 0);

            if (Event.NewControllerType == M3_ControllerType.Player)
                _TurnNameText.text = "玩家回合";
            if (Event.NewControllerType == M3_ControllerType.AI)
                _TurnNameText.text = "敌人回合";
        }

        Tween Tw0 = Tween.Delay(0.25f);
        Tween Tw1 = Tween.LocalPositionY(_TurnNameText.transform, 800, 0, 0.25f, Ease.OutQuad);
        Tween Tw2 = Tween.Delay(0.5f);
        Tween Tw3 = Tween.LocalPositionY(_TurnNameText.transform, 0, -800, 0.25f, Ease.InQuad);
        Sequence Seq = Sequence.Create()
            .Chain(Tw0).Chain(Tw1).Chain(Tw2).Chain(Tw3).ChainCallback(() =>
            {
                _TurnEndedPanel.gameObject.SetActive(false);
            });
    }
}
