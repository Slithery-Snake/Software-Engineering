using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiveGun : MonoBehaviour, Iinteractable
{
    Shooting shooting;
    [SerializeField] WeaponData weaponData;
    BulletSpawn bSpool;
    [SerializeField]Transform barrelTransform;
    [SerializeField] Shooting shootingType;
    public WeaponData WeaponData { get => weaponData;  }
    public Shooting Shooting { get => shooting; }

    public static CollectiveGun CreateGun(CollectiveGun prefab, BulletSpawn p, Vector3 position, Quaternion rotation, bool fullChamber)
    {
        CollectiveGun r = Instantiate(prefab, position, rotation);
        r.bSpool = p;
        r.Init(fullChamber);
        return r;
    }
    public void SetTag(BulletTag tag)
    {
        shooting.SetTag(tag);
    }
    void Init(bool full)
    {
       shooting =  Shooting.CreateShooting(gameObject, bSpool, barrelTransform, weaponData, full);
       
       
    }
    public void Interacted(PInputManager source)
    {
        source.Inventory.AddGun(this);
        
    }

  
}
