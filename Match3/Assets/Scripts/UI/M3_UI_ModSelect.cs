using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class M3_UI_ModSelect : M3_UI
{
    [SerializeField]
    private HorizontalLayoutGroup _Content;
    [SerializeField]
    private Button _BackButton;
    [SerializeField]
    private Button _StartButton;

    private List<M3_ModCard> _ModCardList = new List<M3_ModCard>();
    private M3_ModData _SelectedModData;

    private Tween _Tween;
    private float _Duration = 0.2f;
    private Ease _EaseType = Ease.OutQuad;

    private void Start()
    {
        PlayOpenAnim();

        if (_BackButton != null)
            _BackButton.onClick.AddListener(OnBackButtonClicked);
        if (_StartButton != null)
            _StartButton.onClick.AddListener(OnStartButtonClicked);

        RefreshModList();
    }

    private void PlayOpenAnim()
    {
        if (_Tween.isAlive) return;

        CanvasGroup CG = GetComponent<CanvasGroup>();
        CG.alpha = 0f;
        CG.interactable = false;
        CG.blocksRaycasts = false;

        _Tween = Tween.Alpha(CG, 0f, 1f, _Duration, _EaseType)
            .OnComplete(() =>
            {
                CG.interactable = true;
                CG.blocksRaycasts = true;
            });
    }

    private void PlayCloseAnim()
    {
        if (_Tween.isAlive) return;

        CanvasGroup CG = GetComponent<CanvasGroup>();
        CG.interactable = false;
        CG.blocksRaycasts = false;

        _Tween = Tween.Alpha(CG, 1f, 0f, _Duration, _EaseType)
            .OnComplete(() =>
            {
                M3_CommonHelper.CloseUI(M3_UIType.ModSelect);
            });
    }

    private void OnBackButtonClicked()
    {
        PlayCloseAnim();
    }

    private void OnStartButtonClicked()
    {
        M3_CommonHelper.CloseUI(M3_UIType.MainMenu);
        M3_StoryViewUIParams UIParams = new M3_StoryViewUIParams();
        UIParams.ModData = _SelectedModData;
        M3_CommonHelper.OpenUI(M3_UIType.StoryView, UIParams);
        PlayCloseAnim();
    }

    private void RefreshModList()
    {
        if (_Content == null)
            return;

        _ModCardList.Clear();

        M3_DataManager DataManager = M3_ManagerHub.Instance.DataManager;
        List<M3_ModData> ModDataList = DataManager.GetModDataList();
        foreach (M3_ModData ModData in ModDataList)
        {
            M3_ModCard ModCard = SpawnModCard(ModData);
            _ModCardList.Add(ModCard);
            ModCard.transform.SetParent(_Content.transform, false);
        }

        if (_ModCardList.Count > 0)
        {
            OnModCardClicked(_ModCardList[0]);
        }
    }

    private void OnModCardClicked(M3_ModCard ModCard)
    {
        foreach (M3_ModCard Card in _ModCardList)
        {
            if (Card == ModCard)
            {
                Card.SetChecked(true);
                _SelectedModData = ModCard.ModData;
            }
            else
            {
                Card.SetChecked(false);
            }
        }
    }

    public M3_ModCard SpawnModCard(M3_ModData ModData)
    {
        M3_DataManager DataManager = M3_ManagerHub.Instance.DataManager;

        GameObject ModCardPrefab = M3_ManagerHub.Instance.PrefabManager.GetPrefab("ModCard");
        if (ModCardPrefab != null)
        {
            M3_ModCard ModCard = GameObject.Instantiate(ModCardPrefab).GetComponent<M3_ModCard>();
            ModCard.SetChecked(false);
            ModCard.SetButtonClickAction(() =>
            {
                OnModCardClicked(ModCard);
            });

            ModCard.SetModData(ModData);

            return ModCard;
        }

        return null;
    }
}
