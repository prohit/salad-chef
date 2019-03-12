using UnityEngine;

public class VegetableStand : KitchenStand
{
    [SerializeField] VegetableItem vegetable;

    protected override void Init()
    {
        base.Init();
        transform.Find("Icon").GetComponent<SpriteRenderer>().sprite = vegetable.Icon;
        currentState = KitchenStandState.CONATIN_VEGETABLE;
    }

    public override bool Execute(Command command, Carrier playerCarrier = null)
    {
        bool result = base.Execute(command, playerCarrier);
        if(command == Command.PICKUP)
        {
            VegetableItem vegItem = Instantiate(vegetable); //TODO use object pooling
            playerCarrier.OnItemPickup(vegItem);
            result = true;
        }
        return result;
    }
}
