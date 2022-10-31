using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    float weaponCDTime;
    bool canFire = true;
    float damage;
    float range;
    [SerializeField] Transform barrelTransform;
    bool hasAmmo = true;
    int magSize;
    bool isReloading = false;
    WeaponData whichWeapon;
    bool triggerDown = false;
    bool searDown = false;
    int inChamber = 0;
    int reloadTime;
    Coroutine reloadingRoutine;
    BulletSpawn.BulletPool bulletPool;
    int ammoTypeID;
<<<<<<< HEAD
    HotBarItem hotBar;
    BulletTag bTag = null;

    public HotBarItem HotBar { get => hotBar; }

    // public new WeaponData  ItemData { get => itemData;  }
    public void SetTag(BulletTag t)
    {
        bTag = t;
    }
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
=======
    public void InitializeShooting(WeaponData whichWeapon, BulletSpawn.BulletPool bulletPool)
>>>>>>> a8d63fda94037cbf14e04d44860db8ab2a4e5271
    {



        this.whichWeapon = whichWeapon;
        damage = whichWeapon.damage;
        range = whichWeapon.range;
        magSize = whichWeapon.magSize;
        reloadTime = whichWeapon.reloadTime;
        weaponCDTime = whichWeapon.weaponCDTime;
        this.bulletPool = bulletPool;
    }
    public void WeaponCoolDown()
    {
        canFire = true;

    }
    public void TriggerRelease()
    {
        triggerDown = false;
        searDown = false;
    }
    public void Shoot()
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
<<<<<<< HEAD
            Bullet bullet = bulletPool.RequestBullet();//bulletPool.RequestBullet();
            Vector3 direction = barrelTransform.forward;
            // bullet.transform.rotation = barrelTransform.rotation;
            Vector3 position = barrelTransform.position;
           // bullet.transform.position = position;
            bullet.Shoot(position, direction, bTag);
            // bullet.Rg.velocity = 
           // bullet.Rg.AddForce(direction * bullet.SC.ForceMagnitude, ForceMode.Impulse);
=======
            Bullet bullet =   bulletPool.RequestBullet();
            bullet.Activate();
            Vector3 direction = barrelTransform.forward;
            transform.LookAt(barrelTransform.position);
            transform.rotation = barrelTransform.rotation;
            Vector3 position = barrelTransform.position + new Vector3(0, 1, 0);
            transform.position = position;
           // bullet.Rg.velocity = 
            bullet.Rg.AddForce(direction * bullet.SC.forceMagnitude, ForceMode.Impulse);
>>>>>>> a8d63fda94037cbf14e04d44860db8ab2a4e5271
            //shooting bullet stuff

        }
    }

    public void TriggerDown()
    {


       
            triggerDown = true;
           // Shoot();
            searDown = !whichWeapon.isAuto;
      

    }
}
