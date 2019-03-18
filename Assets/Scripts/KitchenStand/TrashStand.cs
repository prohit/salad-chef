using UnityEngine;
using System.Collections;

public class TrashStand : KitchenStand
{
    public override bool Execute(Command command, Carrier playerCarrier = null)
    {
        bool result = base.Execute(command, playerCarrier);
        carrier.OnItemPickup(playerCarrier.GetCarryingItem());
        playerCarrier.OnItemDrop();
        result = true;
        StartCoroutine(DisposeItem());
        return result;
    }

    IEnumerator DisposeItem()
    {
        yield return new WaitForSeconds(0.1f);
        var item = carrier.GetCarryingItem();
        carrier.OnItemDrop();
        item.gameObject.AddComponent<Rigidbody>();
        Destroy(item.gameObject, 0.3f);
    }
}
