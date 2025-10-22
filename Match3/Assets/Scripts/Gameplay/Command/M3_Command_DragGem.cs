using UnityEngine;

public class M3_Command_DragGem : M3_Command
{
    public override string CommandName { get { return "Drag Gems Command"; } }
    public override bool IsExecuting { get { return _IsExecuting; } }
    public override bool IsAsync { get { return _IsAsync; } }

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

    public override void Execute()
    {
        _IsExecuting = true;

        M3_GridCellContainer ConA = _DraggedContainer;
        if (ConA == null)
            return;

        M3_GridCellContainer ConB = _DraggedContainer.ParentGrid.GetContainer(ConA.GridCoords.x, ConA.GridCoords.y, _DragDirection);
        if (ConB == null)
        {
            M3_Gem GemA = ConA.GetGemCell() as M3_Gem;
            if (GemA != null)
            {
                GemA.RecoverOrder();
                GemA.RecoverScale();
            }

            M3_GameController.Instance.SetAllowInput(true);

            return;
        }

        M3_Gem GemB = ConB.GetGemCell() as M3_Gem;
        if (GemB == null)
        {
            M3_Gem GemA = ConA.GetGemCell() as M3_Gem;
            if (GemA != null)
            {
                GemA.RecoverOrder();
                GemA.RecoverScale();
            }

            M3_GameController.Instance.SetAllowInput(true);

            return;
        }

        M3_GameController.Instance.SetAllowInput(false);

        _DraggedContainer.ParentGrid.SwapGemCell(ConA.GridCoords.x, ConA.GridCoords.y, ConB.GridCoords.x, ConB.GridCoords.y);
    }

    public override string C2String()
    {
        return CommandName;
    }
}
