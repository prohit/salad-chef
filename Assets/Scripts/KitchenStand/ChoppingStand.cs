using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;

public class ChoppingStand : KitchenStand, ICarrier
{
    [SerializeField] Transform pickupTransform;
    [SerializeField] int carryLimit = 3;
    Queue<PickupItem> carryingItems;

    protected override void Init()
    {
        base.Init();
        carryingItems = new Queue<PickupItem>();
        currentState = KitchenStandState.READY_TO_CHOP;
    }

    public override void Execute(Command command, ICarrier carrier = null)
    {
        base.Execute(command, carrier);
        switch(currentState)
        {
            case KitchenStandState.CONATIN_VEGETABLE:
                if(command == Command.CHOPPING)
                {
                    currentState = KitchenStandState.CHOPPING;
                }
                else if(command == Command.PICKUP)
                {
                    carrier.OnItemPickup(GetCarryingItem());
                    OnItemDrop();
                    if (IsCarrying() == false)
                    {
                        currentState = KitchenStandState.READY_TO_CHOP;
                    }
                }
                break;

            case KitchenStandState.CHOPPING_DONE:
                if(command == Command.PICKUP)
                {
                    carrier.OnItemPickup(GetCarryingItem());
                    OnItemDrop();
                    if (IsCarrying() == false)
                    {
                        currentState = KitchenStandState.READY_TO_CHOP;
                    }
                }
                else if(command == Command.DROP)
                {
                    OnItemPickup(carrier.GetCarryingItem());
                    carrier.OnItemDrop();
                    currentState = KitchenStandState.CONATIN_VEGETABLE;
                }
                break;

            case KitchenStandState.CHOPPING:
                //TODO buzzer sound or visual indication
                break;
        }
    }

    public bool CanCarry()
    {
        return (carryLimit < carryingItems.Count);
    }

    public void OnItemPickup(PickupItem item)
    {
        var itemTransform = item.GetItem().transform;
        itemTransform.SetParent(pickupTransform);
        carryingItems.Enqueue(item);
    }

    public void OnItemDrop()
    {
        Assert.IsTrue(carryingItems.Count == 0, "Chopping stand have no item to drop!!");
        carryingItems.Dequeue();
    }

    public bool IsCarrying()
    {
        return (carryingItems.Count > 0);
    }

    public PickupItem GetCarryingItem()
    {
        Assert.AreEqual(carryingItems.Count, 0, "Chopping stand is empty!!");
        return carryingItems.Peek();
    }
}
