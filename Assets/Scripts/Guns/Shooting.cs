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
    public void InitializeShooting(WeaponData whichWeapon, BulletSpawn.BulletPool bulletPool)
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
            Bullet bullet =   bulletPool.RequestBullet();
            bullet.Activate();
            Vector3 direction = barrelTransform.forward;
            transform.LookAt(barrelTransform.position);
            transform.rotation = barrelTransform.rotation;
            Vector3 position = barrelTransform.position + new Vector3(0, 1, 0);
            transform.position = position;
           // bullet.Rg.velocity = 
            bullet.Rg.AddForce(direction * bullet.SC.forceMagnitude, ForceMode.Impulse);
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
