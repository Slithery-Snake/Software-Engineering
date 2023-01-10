using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoSpawn : MonoBehaviour
{
    public bool inf;
    public int id;
    public int count;
    private void Awake()
    {
        GameManager.SpawnData.ammo.Add(this);
    }
}
