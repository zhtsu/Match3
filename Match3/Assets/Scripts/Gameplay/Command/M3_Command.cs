using System.Collections;

public abstract class M3_Command
{
    public abstract string CommandName { get; }
    public abstract bool IsExecuting { get; }
    public abstract bool IsAsync { get; }
    public abstract IEnumerator Execute();
    public abstract string C2String();
}
