using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{

    public int ID;
    private void Awake()
    {
        GameManager.SpawnData.item.Add(this);
    }
}
