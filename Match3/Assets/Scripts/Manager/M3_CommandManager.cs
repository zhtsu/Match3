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

    public void Update()
    {
        if (CommandQueue.Count > 0)
        {
            if (LatestExecutedCommand == null)
            {
                LatestExecutedCommand = CommandQueue.Dequeue();
                LatestExecutedCommand.Execute();
            }
            else
            {
                if (LatestExecutedCommand.IsAsync)
                {
                    LatestExecutedCommand = CommandQueue.Dequeue();
                    LatestExecutedCommand.Execute();
                }
                else
                {
                    if (!LatestExecutedCommand.IsExecuting)
                    {
                        LatestExecutedCommand = CommandQueue.Dequeue();
                        LatestExecutedCommand.Execute();
                    }
                }
            }
        }
    }
}
