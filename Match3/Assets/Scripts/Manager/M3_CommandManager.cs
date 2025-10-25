using System;
using System.Collections.Generic;

public class M3_CommandManager : M3_Manager
{
    public override string ManagerName
    {
        get { return "Command Manager"; }
    }

    private Queue<M3_Command> CommandQueue = new Queue<M3_Command>();
    private M3_Command LatestExecutedCommand = null;

    public override void Initialize()
    {

    }

    public override void Destroy()
    {
        CommandQueue.Clear();
        LatestExecutedCommand = null;
    }

    public void PushCommand(M3_Command Command)
    {
        if (Command == null)
            return;

        CommandQueue.Enqueue(Command);
    }

    private void ExecuteLatestExecutedCommand()
    {
        LatestExecutedCommand = CommandQueue.Dequeue();
        M3_GameController.Instance.RunCoroutine(LatestExecutedCommand.Execute());
    }

    public void Update()
    {
        if (CommandQueue.Count == 0)
            return;

        if (LatestExecutedCommand == null)
        {
            ExecuteLatestExecutedCommand();
        }
        else
        {
            if (LatestExecutedCommand.IsAsync)
            {
                ExecuteLatestExecutedCommand();
            }
            else
            {
                if (!LatestExecutedCommand.IsExecuting)
                {
                    ExecuteLatestExecutedCommand();
                }
            }
        }
    }
}
