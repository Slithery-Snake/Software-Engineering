using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : Item<AmmoSC>, Iinteractable
{
    [SerializeField] int count = 0;
    public static void CreateAmmo(Ammo pref, int count, Vector3 cord, Quaternion rotation)
    {
        Ammo am = Instantiate(pref, cord, rotation);
        am.count = count;
    }
    public int Count { get => count; set => count = value; }

    public void Interacted(PInputManager source)
    {
        source.Inventory.AddAmmo(this);
        Destroy(gameObject);
    }

   
  
}
