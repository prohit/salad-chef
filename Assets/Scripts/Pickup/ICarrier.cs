
public interface ICarrier
{
    bool CanCarry();

    void OnItemPickup(PickupItem item);

    void OnItemDrop();

    bool IsCarrying();

    PickupItem GetCarryingItem();
}
