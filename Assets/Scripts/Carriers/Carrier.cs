using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System.Linq;

public enum PickableType
{
    VEGETABLE,
    SALAD,
    PLATE
}

public class Carrier : MonoBehaviour
{
    [SerializeField] Transform carryTransform;
    protected LinkedList<PickableItem> pickableItems;
    public List<PickableType> ableToPickupItems;

    // Start is called before the first frame update
    void Start()
    {
        pickableItems = new LinkedList<PickableItem>();
    }

    public bool CanCarryItem(PickableItem item)
    {
        return ableToPickupItems.Contains(item.Type);
    }

    //TODO make TransferItem() = OnItemPickup + OnItemDrop
    public void OnItemPickup(PickableItem item)
    {
        item.transform.SetParent(carryTransform, false);
        pickableItems.AddLast(item);
        item.Carrier = this;
    }

    public void OnItemPickupAtFirst(PickableItem item)
    {
        item.transform.SetParent(carryTransform, false);
        pickableItems.AddFirst(item);
        item.Carrier = this;
    }

    public void OnItemDrop()
    {
        Assert.IsFalse(pickableItems.Count == 0, name + " have no item to drop!!");
        pickableItems.RemoveFirst();
    }

    public bool IsCarrying()
    {
        return (pickableItems.Count > 0);
    }

    public PickableItem GetCarryingItem()
    {
        Assert.AreNotEqual(pickableItems.Count, 0, name + " have no items to carry!!");
        return pickableItems.First();
    }

    public PickableItem GetLastCarryingItem()
    {
        Assert.AreNotEqual(pickableItems.Count, 0, name + " have no items to carry!!");
        return pickableItems.Last();
    }

    public int ItemsCount()
    {
        return pickableItems.Count;
    }
}
