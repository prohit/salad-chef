using UnityEngine;

public class PlateStand : KitchenStand
{
    [SerializeField] PlateItem platePrefab;

    /*
    protected override void Init()
    {
        base.Init();
        currentState = KitchenStandState.EMPTY;
    }
    */

    public override bool Execute(Command command, Carrier playerCarrier = null)
    {
        bool result = base.Execute(command, playerCarrier);
        switch(currentState)
        {
            case KitchenStandState.EMPTY:
                if (command == Command.DROP)
                {
                    PlateItem plate = Instantiate(platePrefab); //TODO use object pooling
                    plate.AddSalad((SaladItem)playerCarrier.GetCarryingItem());
                    currentState = KitchenStandState.CONTAIN_SALAD; //ready to pick up
                    carrier.OnItemPickup(plate);
                    playerCarrier.OnItemDrop();
                    result = true;
                }
                break;

            case KitchenStandState.CONTAIN_SALAD:
                if(command == Command.PICKUP)
                {
                    currentState = KitchenStandState.EMPTY;
                    playerCarrier.OnItemPickup(carrier.GetCarryingItem());
                    carrier.OnItemDrop();
                    result = true;
                }
                else if(command == Command.DROP)
                {
                    PlateItem plate = (PlateItem)carrier.GetCarryingItem();
                    plate.AddSalad((SaladItem)playerCarrier.GetCarryingItem());
                    playerCarrier.OnItemDrop();
                    result = true;
                }
                break;
        }
        return result;
    }
}
