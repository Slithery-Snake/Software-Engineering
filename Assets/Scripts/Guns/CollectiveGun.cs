using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiveGun : MonoBehaviour, Iinteractable
{
    Shooting shooting;
    [SerializeField] WeaponData weaponData;
    BulletSpawn bSpool;
    [SerializeField]Transform barrelTransform;

    public WeaponData WeaponData { get => weaponData;  }
    public Shooting Shooting { get => shooting; }

    public static CollectiveGun CreateGun(CollectiveGun prefab, BulletSpawn p, Vector3 position, Quaternion rotation)
    {
        CollectiveGun r = Instantiate(prefab, position, rotation);
        r.bSpool = p;
        r.Init();
        return r;
    }
    public void SetTag(BulletTag tag)
    {
        shooting.SetTag(tag);
    }
    void Init()
    {
       shooting =  Shooting.CreateShooting(gameObject, bSpool, barrelTransform, weaponData);
       
    }
    public void Interacted(PInputManager source)
    {
        source.Inventory.AddGun(this);
        
    }

  
}
