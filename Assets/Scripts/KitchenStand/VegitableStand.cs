
public class VegitableStand : KitchenStand
{
    protected override void Init()
    {
        base.Init();
        currentState = KitchenStandState.CONATIN_VEGETABLE;
    }
}
