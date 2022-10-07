using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : Item<WeaponData>
{
    float weaponCDTime;
    bool canFire = true;
    [SerializeField] Transform barrelTransform;
    AmmoSC ammoType;
    bool hasAmmo = true;
    int magSize;
    bool isReloading = false;
    bool triggerDown = false;
    bool searDown = false;
    int inChamber = 0;
    int reloadTime;
    Coroutine reloadingRoutine;
    BulletSpawn.BulletPool bulletPool;
    BulletSC bulletType;
    int ammoTypeID;

   // public new WeaponData  ItemData { get => itemData;  }

    public static Shooting CreateGun(Shooting gun, BulletSpawn p)
    {
        Shooting r = Instantiate(gun);
        r.InitializeShooting(p);
        HotBarItem.CreateHotBar(r.TriggerDown, null, r.TriggerRelease, null, r.itemData, r.gameObject);
        return r;
    }
     void InitializeShooting( BulletSpawn bulletSpawn)
    {



       ammoType = itemData.AmmoSource;
        magSize = itemData.magSize;
        reloadTime = itemData.reloadTime;
        weaponCDTime = itemData.weaponCDTime;
        bulletPool = bulletSpawn.RequestPool(ammoType.BulletType);

    }
    void WeaponCoolDown()
    {
        canFire = true;

    }
     void TriggerRelease()
    {
        triggerDown = false;
        searDown = false;
    }
     void Shoot()
    {
        if (!searDown && canFire && hasAmmo)
        {
            //RaycastHit rayInfo;
            inChamber = inChamber - 1;
            canFire = false;
            Invoke("WeaponCoolDown", weaponCDTime);
            if (inChamber <= 0)
            {
                hasAmmo = false;
            }
            Bullet bullet = Instantiate(bulletPool.RequestBullet());//bulletPool.RequestBullet();
            bullet.Activate();
            Vector3 direction = barrelTransform.forward;
            bullet.transform.rotation = barrelTransform.rotation;
            Vector3 position = barrelTransform.position + new Vector3(0, 1, 0);
            bullet.transform.position = position;
           // bullet.Rg.velocity = 
            bullet.Rg.AddForce(direction * bullet.SC.forceMagnitude, ForceMode.Impulse);
            //shooting bullet stuff

        }
    }

     void TriggerDown()
    {


       
            triggerDown = true;
            Shoot();
            searDown = !itemData.isAuto;
      

    }

  
}
