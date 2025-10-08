using UnityEngine;

public class M3_Command_DragGem : M3_Command
{
    public override string CommandName { get { return "Drag Gems Command"; } }
    public override bool IsExecuting { get { return _IsExecuting; } }
    public override bool IsAsync { get { return _IsAsync; } }

    private M3_Gem _DraggedGem;
    private M3_MouseMoveDirection _DragDirection;
    private bool _IsExecuting = false;
    private bool _IsAsync = true;

    public M3_Command_DragGem(M3_Gem DraggedGem, M3_MouseMoveDirection DraggedDirection, bool IsAsync = true)
    {
        _DraggedGem = DraggedGem;
        _DragDirection = DraggedDirection;
        _IsAsync = IsAsync;
    }

    public override void Execute()
    {
        _IsExecuting = true;
    }

    public override string C2String()
    {
        return CommandName;
    }
}
