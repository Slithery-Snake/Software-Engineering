using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item<T> : MonoBehaviour where T : ItemSC
{
    protected int itemID;
    [SerializeField] protected T itemData;
    public int ItemID { get => itemID; }
    public T ItemData { get => itemData; }
    protected void Start()
    {
        itemID = itemData.ItemID;
    }
}
