using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiveGun : Item<WeaponData>, Iinteractable
{
    BulletSpawn bSpool;
    [SerializeField]Transform barrelTransform;
    [SerializeField] Shooting shooting;
    Interactable interact;
    public Shooting Shooting { get => shooting; }

    public static CollectiveGun CreateGun(CollectiveGun prefab, BulletSpawn p, Vector3 position, Quaternion rotation, bool fullChamber)
    {
        CollectiveGun r = Instantiate(prefab, position, rotation);
        r.bSpool = p;
        r.interact = Interactable.Create(r.gameObject, r);

        r.Init(fullChamber);
        return r;
    }
    public void SetTag(BulletTag tag)
    {
        shooting.SetTag(tag);
    }
    void Init(bool full)
    {
        Shooting.InitShoot(shooting, bSpool, barrelTransform, itemData, full, this, interact);
       
       
    }
   
    public void Interacted(SourceProvider source)
    {
        source.Inventory.AddGun(this);
        interact.enabled = false;
       
        
    }

  
}
