using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
    HotBarItem hotBar;

    public HotBarItem HotBar { get => hotBar; }

    // public new WeaponData  ItemData { get => itemData;  }

    public static Shooting CreateShooting(GameObject gun, BulletSpawn p , Transform barrelTransform, WeaponData data)
    {
        Shooting r = gun.AddComponent<Shooting>();
        r.barrelTransform = barrelTransform;
        r.itemData = data;

        r.InitializeShooting(p); 
        r.hotBar = HotBarItem.CreateHotBar(r.TriggerDown, null, r.TriggerRelease, null, r.itemData, r.gameObject);
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
    public void LoadBullets(Ammo reserves)
    {
        Debug.Log(reserves.Count);
        int bulletsToFull = magSize - inChamber;
        if (reserves.Count <= bulletsToFull)
        {
            magSize += reserves.Count;
            reserves.Count = 0;
            return;
        }

        inChamber = magSize;
        reserves.Count -= bulletsToFull;
          if(inChamber > 0) { hasAmmo = true; }
        Debug.Log(inChamber);
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
    IEnumerator Invoke(float time, UnityAction hi)
    {
        
        yield return new WaitForSeconds(time);
        hi();
    }
     void Shoot()
    {
        if (!searDown && canFire && hasAmmo)
        {
            //RaycastHit rayInfo;
            inChamber--;
            canFire = false;
            // Invoke(WeaponCoolDown, weaponCDTime);
            StartCoroutine(Invoke(weaponCDTime, WeaponCoolDown));
            if (inChamber <= 0)
            {
                hasAmmo = false;
            }
            Bullet bullet = bulletPool.RequestBullet();//bulletPool.RequestBullet();
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
