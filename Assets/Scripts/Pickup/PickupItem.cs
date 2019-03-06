using UnityEngine;
using UnityEngine.UI;

public class PickupItem : ScriptableObject
{
    [SerializeField] GameObject Prefab;
    [SerializeField] Image icon;
    public Image Icon { get { return icon; } }

    GameObject item;

    public GameObject GetItem()
    {
        if (item == null)
            return GetNewItem();
        return item;
    }

    public GameObject GetNewItem()
    {
        //TODO use pooling instead of creating new object
        item = Instantiate(Prefab);
        return item;
    }
}
