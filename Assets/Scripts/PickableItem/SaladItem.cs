using System.Collections.Generic;
using UnityEngine;

public class SaladItem : PickableItem
{
    [SerializeField] float gapBetweenItems = 0.5f;
    public List<VegType> VegetableItemTypes;// { get; private set; }


    private void Awake()
    {
        VegetableItemTypes = new List<VegType>();
    }

    public void AddItem(VegetableItem item)
    {
        VegetableItemTypes.Add(item.VegType);
        item.transform.SetParent(transform, false);
        item.transform.localPosition = Vector3.zero;

        if(VegetableItemTypes.Count == 1)
        {
            item.transform.localPosition = Vector3.zero;
        }
        else 
        {
            AdjustItems();
        }
    }

    public void AddSalad(SaladItem item)
    {
        VegetableItemTypes.AddRange(item.VegetableItemTypes);
        foreach (Transform child in item.transform)
        {
            child.transform.SetParent(transform, false);
        }
        AdjustItems();
        Destroy(item.gameObject);
    }

    void AdjustItems()
    {
        int count = transform.childCount;
        float startposX = gapBetweenItems * (1 - count) / 2.0f;
        int index = 0;
        foreach (Transform child in transform)
        {
            child.localPosition = new Vector3(startposX + (index++ * gapBetweenItems), 0, 0);
        }
    }
}
