using System.Collections;
using UnityEngine;

public struct M3_DragGemData
{
    public M3_DragGemData(M3_GridCellContainer InDraggedContainer, M3_MouseMoveDirection InDragDirection)
    {
        DraggedContainer = InDraggedContainer;
        DragDirection = InDragDirection;
    }

    public M3_GridCellContainer DraggedContainer;
    public M3_MouseMoveDirection DragDirection;
}

public class M3_Command_DragGem : M3_Command
{
    public override string CommandName { get { return "Drag Gems Command"; } }
    public override bool IsExecuting { get { return _IsExecuting; } }
    public override bool IsAsync { get { return _IsAsync; } }

    private M3_DragGemData _DragGemData;
    private M3_GridCellContainer _DraggedContainer;
    private M3_MouseMoveDirection _DragDirection;
    private bool _IsExecuting = false;
    private bool _IsAsync = true;

    public M3_Command_DragGem(M3_GridCellContainer DraggedContainer, M3_MouseMoveDirection DraggedDirection, bool IsAsync = true)
    {
        _DraggedContainer = DraggedContainer;
        _DragDirection = DraggedDirection;
        _IsAsync = IsAsync;
    }

    public override IEnumerator Execute()
    {
        _IsExecuting = true;

        M3_GridCellContainer ConA = _DraggedContainer;
        M3_GridCellContainer ConB = ConA.ParentGrid.GetContainer(ConA.GridCoords.x, ConA.GridCoords.y, _DragDirection);
        if (ConB == null)
        {
            M3_Gem Gem = ConA.GetGemCell() as M3_Gem;
            if (Gem != null)
            {
                yield return Gem.RecoverScale();
            }

            M3_GameController.Instance.SetAllowInput(true);

            yield break;
        }

        M3_GameController.Instance.SetAllowInput(false);
        yield return ConA.ParentGrid.SwapGem(ConA.GridCoords.x, ConA.GridCoords.y, ConB.GridCoords.x, ConB.GridCoords.y);
        M3_GameController.Instance.SetAllowInput(true);

        _IsExecuting = true;
    }

    public override string C2String()
    {
        return CommandName;
    }
}
