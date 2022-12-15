using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiveGun : MonoBehaviour, Iinteractable
{
    [SerializeField] WeaponData weaponData;
    BulletSpawn bSpool;
    [SerializeField]Transform barrelTransform;
    [SerializeField] Shooting shooting;
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
        Shooting.InitShoot(shooting, bSpool, barrelTransform, weaponData, full);
       
       
    }
    public void Interacted(PInputManager source)
    {
        source.Inventory.AddGun(this);
        
    }

  
}
