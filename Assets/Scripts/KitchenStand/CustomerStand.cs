using UnityEngine;
using System.Collections;

public class CustomerStand : KitchenStand
{
    Customer currentCustomer;

    public bool IsEmpty()
    {
        return (currentState == KitchenStandState.EMPTY);
    }

    public override bool Execute(Command command, Carrier playerCarrier)
    {
        bool result = base.Execute(command, playerCarrier);
        switch(currentState)
        {
            case KitchenStandState.EMPTY: //TODO for test only
                //do nothing
                break;

            case KitchenStandState.CUSTOMER_WAITING:
                if(command == Command.DROP)
                {
                    OrderRecieved(playerCarrier);
                    result = true;
                }
                break;
        }
        return result;
    }

    void OrderRecieved(Carrier playerCarrier)
    {
        PlateItem plate = (PlateItem)playerCarrier.GetCarryingItem();
        carrier.OnItemPickup(plate);
        playerCarrier.OnItemDrop();
        currentCustomer.ProcessOrder(plate.Salad, playerCarrier.GetComponent<Player>());

        //TODO sync state
        /*
        if (correctOrder)
        {
            currentState = KitchenStandState.CUSTOMER_EATING;
            Debug.Log(name + " -- correct order - state : " + currentState);
        }
        //TODO set state to wrong order
        */       
        StartCoroutine(RemovePlate());
    }

    IEnumerator RemovePlate()
    {
        yield return new WaitForSeconds(0.3f);
        var item = carrier.GetCarryingItem();
        carrier.OnItemDrop();
        Destroy(item.gameObject);
    }

    public void CustomerArrived(Customer customer)
    {
        currentState = KitchenStandState.CUSTOMER_WAITING;
        currentCustomer = customer;
        Debug.Log(name + " -- customer arrived - state : " + currentState);
    }

    public void CustomerLeft()
    {
        Debug.Log(name + " -- customer left - state : " + currentState);
        currentState = KitchenStandState.EMPTY;
    }
}
