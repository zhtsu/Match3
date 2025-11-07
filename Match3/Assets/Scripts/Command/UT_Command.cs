using System.Collections;

public abstract class UT_Command
{
    public string GetString() { return "Command"; }
    public abstract IEnumerator Execute();
}
