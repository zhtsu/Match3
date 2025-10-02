using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M3_Gem : MonoBehaviour, M3_IGridCell
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

    public void OnMouseDown()
    {
        _PreMouseClickPos = Input.mousePosition;
    }

    public void OnMouseUp()
    {
        Vector2 Delta = (Vector2)Input.mousePosition - _PreMouseClickPos;
        float Threshold = M3_GameController.Instance.GameConfig.MouseDragThreshold;

        if (Delta.magnitude < Threshold)
        {
            Debug.Log("Mouse Click");
        }
        else
        {
            M3_MouseMoveDirection Direction = GetMouseMoveDirection(Delta);
            M3_ManagerHub.Instance.CommandManager.PushCommand(
                new M3_Command_DragGem(this, Direction));
        }
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
