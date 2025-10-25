using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class M3_Gem : M3_Unit, M3_IGridCell
{
    public Vector2Int CellCoords { get; set; }
    public M3_GridCellContainer ParentContainer { get; set; }

    private Vector2 _PreMouseClickPos;
    private Vector3 _SavedScale;
    private int _SavedOrder;

    public void SetSavedScale(Vector3 InScale) { _SavedScale = InScale; }

    void Start()
    {
        _SavedOrder = transform.GetComponent<Renderer>().sortingOrder;
    }

    void Update()
    {

    }

    private void SetOrderToTop()
    {
        transform.GetComponent<Renderer>().sortingOrder = 100;
    }

    public void RecoverOrder()
    {
        transform.GetComponent<Renderer>().sortingOrder = _SavedOrder;
    }

    public void OnMouseDownEvent()
    {
        if (M3_GameController.Instance.IsAllowInput == false)
            return;

        if (M3_GameController.Instance.CurrentBattleInputController == M3_ControllerType.AI)
            return;

        M3_GameController.Instance.SetCurrentClickedGem(this);

        _PreMouseClickPos = Input.mousePosition;

        SetOrderToTop();

        Tween.Scale(transform, _SavedScale.x * 1.2f, 0.05f, Ease.OutQuad);

        CallScriptFunc("OnMouseDown");
    }

    public void OnMouseUpEvent()
    {
        if (M3_GameController.Instance.IsAllowInput == false)
            return;

        if (M3_GameController.Instance.CurrentBattleInputController == M3_ControllerType.AI)
            return;

        M3_GameController.Instance.SetAllowInput(false);

        Vector2 Delta = (Vector2)Input.mousePosition - _PreMouseClickPos;
        float Threshold = M3_GameController.Instance.GameConfig.MouseDragThreshold;

        if (Delta.magnitude < Threshold)
        {
            if (M3_GameController.Instance.CurrentClickedGem == this)
            {
                RecoverOrder();
                RecoverScale();
                M3_GameController.Instance.SetAllowInput(true);
            }
        }
        else
        {
            M3_MouseMoveDirection Direction = GetMouseMoveDirection(Delta);
            M3_ManagerHub.Instance.CommandManager.PushCommand(
                new M3_Command_DragGem(ParentContainer, Direction));
        }

        CallScriptFunc("OnMouseUp");
    }

    private M3_MouseMoveDirection GetMouseMoveDirection(Vector2 Delta)
    {
        if (Mathf.Abs(Delta.x) > Mathf.Abs(Delta.y))
        {
            if (Delta.x > 0)
                return M3_MouseMoveDirection.Right;
            else
                return M3_MouseMoveDirection.Left;
        }
        else
        {
            if (Delta.y > 0)
                return M3_MouseMoveDirection.Up;
            else
                return M3_MouseMoveDirection.Down;
        }
    }

    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }

    public IEnumerator RecoverScale()
    {
        if (transform.localScale != _SavedScale)
        {
            Tween Tw = Tween.Scale(transform, _SavedScale, 0.1f, Ease.OutBack);
            yield return Tw.ToYieldInstruction();
        }
    }
}
