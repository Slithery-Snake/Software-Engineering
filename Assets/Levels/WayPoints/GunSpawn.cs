using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GunSpawn : MonoBehaviour
{
  
    public ItemManager.GunStruct s;
        private void Awake()
    {
        s.v = transform.position;
        
        GameManager.SpawnData.guns.Add(s);
    }
}
