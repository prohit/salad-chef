
public class ChoppingStand : KitchenStand
{
    protected override void Init()
    {
        base.Init();
        currentState = KitchenStandState.READY_TO_CHOP;
    }

    protected override void Execute(Command command)
    {
        switch(currentState)
        {
            case KitchenStandState.CONATIN_VEGITABLE:
                if(command == Command.CHOPPING)
                {
                    currentState = KitchenStandState.CHOPPING;
                }
                break;

            case KitchenStandState.CHOPPING_DONE:
                if(command == Command.PICKUP)
                {
                    currentState = KitchenStandState.READY_TO_CHOP;
                }
                else if(command == Command.DROP)
                {
                    currentState = KitchenStandState.CONATIN_VEGITABLE;
                }
                break;

            case KitchenStandState.CHOPPING:
                //TODO buzzer sound or visual indication
                break;
        }
    }
}
