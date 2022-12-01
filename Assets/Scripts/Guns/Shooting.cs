using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Shooting : Item<WeaponData>
{
    protected float weaponCDTime;
    protected bool canFire = true;
    [SerializeField] protected Transform barrelTransform;
    protected AmmoSC ammoType;
    protected bool hasAmmo = true;
    protected int magSize;
    protected bool isReloading = false;
    protected Coroutine invoke;
    protected int inChamber = 0;
    protected BulletSpawn.BulletPool bulletPool;
    protected HotBarItem hotBar;
    protected BulletTag bTag = null;

    public HotBarItem HotBar { get => hotBar; }
    public bool IsReloading { get => isReloading;  }
    public bool HasAmmo { get => hasAmmo;}
    public bool CanFire { get => canFire;  }

    // public new WeaponData  ItemData { get => itemData;  }
    public void SetTag(BulletTag t)
    {
        bTag = t;
    }
    public static Shooting CreateShooting(GameObject gun, BulletSpawn p , Transform barrelTransform, WeaponData data, bool chamber)
    {
        Shooting r = gun.AddComponent<Shooting>();
        r.barrelTransform = barrelTransform;
        r.itemData = data;

        r.InitializeShooting(p, chamber); 
        r.hotBar = HotBarItem.CreateHotBar(r.TriggerDown, null, r.TriggerRelease, null, r.itemData, r.gameObject);
        return r;
    }
    protected void InitializeShooting( BulletSpawn bulletSpawn, bool c)
    {


        hasAmmo = c;
        if(c) { inChamber = itemData.magSize; }
         else {inChamber = 0; }
        
       ammoType = itemData.AmmoSource;
        magSize = itemData.magSize;
        weaponCDTime = itemData.weaponCDTime;
        bulletPool = bulletSpawn.RequestPool(ammoType.BulletType);

    }
    public void LoadBullets(Ammo reserves)
    {
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

    protected void WeaponCoolDown()
    {
        canFire = true;

    }
    protected virtual void TriggerRelease()
    {
        
    }
    protected IEnumerator Invoke(float time, UnityAction hi)
    {
        
        yield return new WaitForSeconds(time);
        hi();
    }
    protected virtual void Shoot()
    {
        if (CanFire && hasAmmo )
        {
            //RaycastHit rayInfo;
            inChamber--;
            canFire = false;
            // Invoke(WeaponCoolDown, weaponCDTime);
           invoke = StartCoroutine(Invoke(weaponCDTime, WeaponCoolDown));
            if (inChamber <= 0)
            {
                hasAmmo = false;
            }
            Bullet bullet = bulletPool.RequestBullet();//bulletPool.RequestBullet();
            Vector3 direction = barrelTransform.forward;
            // bullet.transform.rotation = barrelTransform.rotation;
            Vector3 position = barrelTransform.position;
            // bullet.transform.position = position;
            bullet.Shoot(position, direction, bTag);
           // searDown = ItemData.isAuto;
            // bullet.Rg.velocity = 
            // bullet.Rg.AddForce(direction * bullet.SC.ForceMagnitude, ForceMode.Impulse);
            //shooting bullet stuff
            Debug.Log("BANG");
        }
        
    }

    protected virtual void TriggerDown()
    {


       
            Shoot();
           
      

    }

  
}
