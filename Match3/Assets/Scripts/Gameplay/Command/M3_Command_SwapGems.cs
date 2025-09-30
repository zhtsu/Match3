public class M3_Command_SwapGems : M3_Command
{
    public override string CommandName { get { return "Swap Gems Command"; } }
    
    public override void Execute()
    {

    }

    public override string C2String()
    {
        return CommandName;
    }
}
