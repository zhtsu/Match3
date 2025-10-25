using PrimeTween;

public class M3_AIController : M3_Controller
{
    public override string ControllerName { get { return "AI Controller"; } }

    private M3_Grid _Grid;

    public M3_AIController(M3_Grid Grid)
    {
        _Grid = Grid;

        M3_EventBus.Subscribe<M3_Event_BattleControllerChanged>(OnBattleControllerChanged);
    }

    private void OnBattleControllerChanged(M3_Event_BattleControllerChanged Event)
    {
        if (Event.NewControllerType != M3_ControllerType.AI)
            return;

        Tween.Delay(2.0f, () =>
        {
            _Grid.SwapGemByAI();
        });
    }
}
