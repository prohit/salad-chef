﻿using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public float ChoppingTime;
    public virtual void SetChopping(bool enable) { }
    /*
    [SerializeField] GameObject Prefab;
    [SerializeField] Image icon;
    public Image Icon { get { return icon; } }

    GameObject item;

    public float ChoppingTime;

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
    */
}
