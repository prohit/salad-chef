using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerCarrier : MonoBehaviour, ICarrier
{
    [SerializeField] Transform carryTransform;
    [SerializeField] int carryLimit = 2;
    Queue<PickupItem> carryingItems;
    // Start is called before the first frame update
    void Start()
    {
        carryingItems = new Queue<PickupItem>();
    }

    public bool CanCarry()
    {
        return (carryLimit < carryingItems.Count);
    }

    public void OnItemDrop()
    {
        Assert.IsFalse(carryingItems.Count == 0, "Player have no item to drop!!");
        carryingItems.Dequeue();
        Debug.Log("Player dropped an item");
    }

    public void OnItemPickup(PickupItem item)
    {
        var itemTransform = item.transform;
        itemTransform.SetParent(carryTransform, false);
        carryingItems.Enqueue(item);
        Debug.Log("Item pickup by player: " + item.name);
        Debug.Log("Items in player hand: " + carryingItems.Count);
    }

    public bool IsCarrying()
    {
        return (carryingItems.Count > 0);
    }

    public PickupItem GetCarryingItem()
    {
        Debug.Log("Items in player hand: " + carryingItems.Count);
        Assert.AreNotEqual(carryingItems.Count, 0, "Player hand is empty!!");
        return carryingItems.Peek();
    }
}
