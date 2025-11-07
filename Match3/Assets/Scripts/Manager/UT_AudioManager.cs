
public class UT_AudioManager : UT_Manager
{
    public override string ManagerName => "Audio Manager";

    private UT_AudioRoot _AudioRoot;

    public UT_AudioManager(UT_AudioRoot InAudioRoot)
    {
        _AudioRoot = InAudioRoot;
    }

    public override void Destroy()
    {

    }

    public override void Initialize()
    {

    }
}
