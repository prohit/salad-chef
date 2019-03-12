using UnityEngine;

public class PlateItem : PickableItem
{
    [SerializeField] Transform pickupTransform;
    public SaladItem Salad { get; private set; }

    public void AddSalad(SaladItem salad)
    {
        if (Salad == null)
        {
            Salad = salad;
            salad.transform.SetParent(pickupTransform, false);
        }
        else
        {
            Salad.AddSalad(salad);
        }
    }

    public bool ContainCorrectOrder()
    {
        return true;
    }
}
