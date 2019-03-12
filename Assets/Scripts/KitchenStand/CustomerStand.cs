
using System.Collections.Generic;

public struct SaladOrder
{
    string orderName;

    public SaladOrder(List<VegType> items)
    {
        items.Sort();
        orderName = "";
        foreach(var item in items)
        {
            orderName += item + "_";
        }
    }

    public bool IsCorrectOrder(SaladOrder order)
    {
        return (orderName == order.orderName);
    }
}

public class CustomerStand : KitchenStand
{
    SaladOrder currentOrder;

    public bool IsEmpty()
    {
        return (currentState == KitchenStandState.EMPTY);
    }

    public void CustomerArraive(List<VegType> order)
    {
        currentOrder = new SaladOrder(order);
        currentState = KitchenStandState.CUSTOMER_WAITING;
    }

    public override bool Execute(Command command, Carrier playerCarrier)
    {
        bool result = base.Execute(command, playerCarrier);
        switch(currentState)
        {
            case KitchenStandState.EMPTY: //TODO for test only
            case KitchenStandState.CUSTOMER_WAITING:
                if(command == Command.DROP)
                {
                    currentState = KitchenStandState.CUSTOMER_EATING;
                    carrier.OnItemPickup(playerCarrier.GetCarryingItem());
                    playerCarrier.OnItemDrop();
                    result = true;
                }
                break;
        }
        return result;
    }

    void OrderRecieved(SaladItem salad)
    {

    }
}
