using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSC : ScriptableObject
{
    [SerializeField] protected int itemID;

    public int ItemID { get => itemID;  }
}
