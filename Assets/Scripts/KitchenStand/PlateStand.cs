
public class PlateStand : KitchenStand
{
    protected override void Init()
    {
        base.Init();
        currentState = KitchenStandState.CONTAIN_PLATE;
    }

    public override void Execute(Command command, ICarrier carrier = null)
    {
        base.Execute(command, carrier);
        switch(currentState)
        {
            case KitchenStandState.CONTAIN_PLATE:
                if (command == Command.DROP)
                {
                    currentState = KitchenStandState.CONATIN_VEGETABLE; //ready to pick up
                }
                break;

            case KitchenStandState.CONATIN_VEGETABLE:
                if(command == Command.PICKUP)
                {
                    currentState = KitchenStandState.CONTAIN_PLATE;
                }
                break;
        }
    }
}
