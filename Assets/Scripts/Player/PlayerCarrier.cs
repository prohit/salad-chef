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
        Assert.IsTrue(carryingItems.Count == 0, "Player have no item to drop!!");
        carryingItems.Dequeue();
    }

    public void OnItemPickup(PickupItem item)
    {
        var itemTransform = item.GetItem().transform;
        itemTransform.SetParent(carryTransform);
    }

    public bool IsCarrying()
    {
        return (carryingItems.Count > 0);
    }

    public PickupItem GetCarryingItem()
    {
        Assert.AreEqual(carryingItems.Count, 0, "Chopping hand is empty!!");
        return carryingItems.Peek();
    }
}
