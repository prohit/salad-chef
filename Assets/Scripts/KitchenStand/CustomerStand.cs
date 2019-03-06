
public class CustomerStand : KitchenStand
{
    public override void Execute(Command command, ICarrier carrier)
    {
        base.Execute(command, carrier);
        switch(currentState)
        {
            case KitchenStandState.CUSTOMER_WAITING:
                if(command == Command.DROP)
                {
                    currentState = KitchenStandState.CUSTOMER_EATING;
                }
                break;
        }
    }
}
