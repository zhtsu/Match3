public abstract class M3_Command
{
    public abstract string CommandName { get; }
    public abstract void Execute();
    public abstract string C2String();
}
