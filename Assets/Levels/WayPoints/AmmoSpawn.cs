using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoSpawn : MonoBehaviour
{
    public ItemManager.AmmoStruct s;
    private void Awake()
    {
        s.v = transform.position;
        GameManager.SpawnData.ammo.Add(s);
    }
}
