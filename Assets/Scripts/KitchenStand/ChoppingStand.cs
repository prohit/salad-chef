using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Assertions;
using UnityEngine;

public class ChoppingStand : KitchenStand //, ICarrier
{
    [SerializeField] SaladItem saladPrefab;
    //[SerializeField] Transform pickupTransform;
    [SerializeField] int carryLimit = 3;
    //Queue<PickupItem> carryingItems;
    //List<PickupItem> saladItems;
    [SerializeField] float timeSpentInChopping;
    Action choppingEvent;

    /*
    protected override void Init()
    {
        base.Init();
        //carryingItems = new Queue<PickupItem>();
        //saladItems = new List<PickupItem>();
        currentState = KitchenStandState.EMPTY;
    }
    */

    //TODO move chopping done event to seperate class
    public void AddChoppingDoneEvent(Action action)
    {
        choppingEvent += action;
    }

    public void RemoveChoppingDoneEvent(Action action)
    {
        if (choppingEvent != null)
        {
            choppingEvent -= action;
        }
    }

    public void TriggerChoppingDoneEvent()
    {
        choppingEvent?.Invoke();
    }

    public override bool Execute(Command command, Carrier playerCarrier = null)
    {
        bool result = base.Execute(command, playerCarrier);
        Debug.Log("Command: " + command + ", CurrentState: " + currentState);
        switch (currentState)
        {
            case KitchenStandState.EMPTY:
                if(command == Command.DROP)
                {
                    carrier.OnItemPickup(playerCarrier.GetCarryingItem());
                    playerCarrier.OnItemDrop();
                    currentState = KitchenStandState.READY_TO_CHOP;
                    result = true;
                }
                break;

            case KitchenStandState.READY_TO_CHOP:
                if(command == Command.CHOPPING)
                {
                    StartCoroutine(StartChopping((VegetableItem) carrier.GetCarryingItem()));
                    carrier.OnItemDrop();
                    currentState = KitchenStandState.CHOPPING;
                    result = true;
                }
                else if(command == Command.PICKUP)
                {
                    playerCarrier.OnItemPickup(carrier.GetCarryingItem());
                    if (carrier.IsCarrying() == false)
                    {
                        currentState = KitchenStandState.EMPTY;
                    }
                    result = true;
                }
                break;

            case KitchenStandState.CHOPPING_DONE:
                if(command == Command.PICKUP)
                {
                    //pick from salad item list not from carry item list
                    playerCarrier.OnItemPickup(carrier.GetCarryingItem());
                    carrier.OnItemDrop();
                    currentState = KitchenStandState.EMPTY;
                    result = true;
                }
                else if(command == Command.DROP)
                {
                    //AdjustVegetables();
                    //add raw veg at first position after chopping done
                    carrier.OnItemPickupAtFirst(playerCarrier.GetCarryingItem());
                    playerCarrier.OnItemDrop();
                    currentState = KitchenStandState.READY_TO_CHOP;
                    result = true;
                }
                break;

            case KitchenStandState.CHOPPING:
                //TODO buzzer sound or visual indication
                break;
        }
        return result;
    }

    IEnumerator StartChopping(VegetableItem item)
    {
        Debug.Log("Start Chopping, chopping time:  " + item.ChoppingTime);
        timeSpentInChopping = 0;
        item.SetChopping(true);
        while (timeSpentInChopping < item.ChoppingTime)
        {
            float deltaTime = Time.deltaTime;
            yield return new WaitForSeconds(deltaTime);
            timeSpentInChopping += deltaTime;
        }
        ChoppingEnd(item);
    }

    void ChoppingEnd(VegetableItem item)
    {
        currentState = KitchenStandState.CHOPPING_DONE;
        //chopped veg convertade to salad and add in salad list
        item.SetChopping(false);
        SaladItem salad;
        if (carrier.IsCarrying() && carrier.GetLastCarryingItem().Type == PickableType.SALAD)
        {
            salad = (SaladItem)carrier.GetLastCarryingItem();
            salad.AddItem(item);
        }
        else
        {
            salad = Instantiate(saladPrefab); //TODO use object pooling instead
            salad.AddItem(item);
            carrier.OnItemPickup(salad);
        }
        TriggerChoppingDoneEvent();
        Destroy(item, 0.1f);
    }

    //adjust current vegetables on stand
    void AdjustVegetables()
    {
        var item = carrier.GetCarryingItem();
        item.transform.position += new Vector3(0.25f, 0, 0);
    }
    /*
    public bool CanCarry()
    {
        return (carryLimit < carryingItems.Count);
    }

    public void OnItemPickup(PickupItem item)
    {
        var itemTransform = item.transform;
        itemTransform.SetParent(pickupTransform, false);
        carryingItems.Enqueue(item);
    }

    public void OnItemDrop()
    {
        Debug.Log("Chopping stand dropped item, count: " + carryingItems.Count);
        Assert.IsFalse(carryingItems.Count == 0, "Chopping stand have no item to drop!!");
        carryingItems.Dequeue();
    }

    public bool IsCarrying()
    {
        return (carryingItems.Count > 0);
    }

    public PickupItem GetCarryingItem()
    {
        Assert.AreNotEqual(carryingItems.Count, 0, "Chopping stand is empty!!");
        return carryingItems.Peek();
    }
    */
}
