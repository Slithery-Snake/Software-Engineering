using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSpawn : MonoBehaviour
{
    public bool chamber;
    public int id;
    private void Awake()
    {
        GameManager.SpawnData.guns.Add(this);
    }
}
