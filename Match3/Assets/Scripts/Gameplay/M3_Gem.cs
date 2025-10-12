using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M3_Gem : M3_Unit, M3_IGridCell
{
    public Vector2Int CellCoords { get; set; }
    public M3_Grid ParentGrid { get; set; }

    private Vector2 _PreMouseClickPos;

    void Start()
    {
        
    }

    void Update()
    {

    }

    public void OnMouseDownEvent()
    {
        _PreMouseClickPos = Input.mousePosition;

        CallScriptFunc("OnMouseDown");
    }

    public void OnMouseUpEvent()
    {
        Vector2 Delta = (Vector2)Input.mousePosition - _PreMouseClickPos;
        float Threshold = M3_GameController.Instance.GameConfig.MouseDragThreshold;

        if (Delta.magnitude < Threshold)
        {

        }
        else
        {
            M3_MouseMoveDirection Direction = GetMouseMoveDirection(Delta);
            M3_ManagerHub.Instance.CommandManager.PushCommand(
                new M3_Command_DragGem(this, Direction));
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
}
