using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemSpawn : MonoBehaviour
{

    public ItemManager.ItemStruct s;
    private void Awake()
    {
        s.v = transform.position;
        GameManager.SpawnData.item.Add(s);
    }
}


