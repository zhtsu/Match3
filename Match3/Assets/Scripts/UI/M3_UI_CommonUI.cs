using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

public class M3_UI_CommonUI : M3_UI
{
    [SerializeField]
    private Button _CloseButton;
    [SerializeField]
    private Canvas _MainCanvas;
    [SerializeField]
    private Image _WhiteMask;
    [SerializeField]
    private GameObject _ContentUI;

    private void Start()
    {
        PlayOpenAnim();
    }

    private void PlayOpenAnim()
    {
        _MainCanvas.transform.localScale = new Vector3(0, 0.02f, 0.1f);

        Tween ScaleX = Tween.ScaleX(_MainCanvas.transform, 0f, 1f, 0.2f);
        Tween ScaleY = Tween.ScaleY(_MainCanvas.transform, 0.02f, 1f, 0.2f);

        Sequence NewSequence = Sequence.Create(
            sequenceEase: Ease.OutCubic)
            .Chain(ScaleX)
            .ChainCallback(() =>
            {
                _WhiteMask.enabled = false;
            })
            .Chain(ScaleY);
    }

    private void PlayCloseAnim()
    {
        Tween ScaleY = Tween.ScaleY(_MainCanvas.transform, 1f, 0.02f, 0.1f);

        Sequence NewSequence = Sequence.Create(
            sequenceEase: Ease.InCubic)
            .Chain(ScaleY)
            .ChainCallback(() =>
            {
                _WhiteMask.enabled = true;
            })
            .ChainCallback(() =>
            {
                M3_UIManager UIManager = M3_ManagerHub.Instance.UIManager;
                UIManager.CloseUI(base.Type);
            });
    }

    public void OnCloseButtonRelease()
    {
        PlayCloseAnim();
    }
}
