public enum M3_ControllerType
{
    None = 0,
    Player,
    AI,
}

public abstract class M3_Controller
{
    public abstract string ControllerName { get; }
}
