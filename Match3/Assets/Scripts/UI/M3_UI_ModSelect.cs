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

    private void Start()
    {
        if (_BackButton != null)
        {
            _BackButton.onClick.AddListener(OnBackButtonClicked);
        }

        RefreshModList();
    }

    private void OnBackButtonClicked()
    {
        M3_CommonHelper.CloseUI(M3_UIType.ModSelect);
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

        GameObject ModCardPrefab = M3_ManagerHub.Instance.PrefabManager.GetPrefab(M3_PrefabType.ModCard);
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
