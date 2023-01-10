using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Ammo : Item<AmmoSC>, Iinteractable
{
    [SerializeField] int count = 0;
    bool infinity;
    UnityAction<int> setCount;
    int startingCount;
    Interactable interact;
    public static Ammo  CreateAmmo(Ammo pref, int count, Vector3 cord, Quaternion rotation, bool infinity)
    {
        Ammo am = Instantiate(pref, cord, rotation);
        am.count = count;
        am.startingCount = count;
        am.interact = Interactable.Create(am.gameObject, am);
        am.setCount = infinity ? am.CountInfinite : am.CountLimited;
        return am;
    }
    public int Count { get => count; }
    public UnityAction<int> SetCount { get => setCount; }

     void CountLimited(int i)
    {
        count = i;
    }
    void CountInfinite(int i)
    {
        count = startingCount;
    }
    public void Interacted(SourceProvider source)
    {
        source.Inventory.AddAmmo(this);

        Destroy(gameObject);
    }

  
}
