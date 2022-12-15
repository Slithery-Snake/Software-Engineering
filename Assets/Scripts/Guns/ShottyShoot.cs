using System.Collections;
using System;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.Events;

public class ShottyShoot : Shooting
{
    [SerializeField] ShottySC shottySC;
    bool roundChambered = true;
    bool needEjection = false;
    public override bool GetCanFire()
    {
        return roundChambered && !needEjection && canFire; ;
    }
  
    protected override void Shoot()
    {
        if (canFire && hasAmmo && needEjection == false && roundChambered)
        {
            //RaycastHit rayInfo;
            inChamber--;
            canFire = false;
            needEjection = true;
            roundChambered = false;
            if (inChamber <= 0)
            {
                hasAmmo = false;
            }
            Vector3 direction = barrelTransform.forward;
            Vector3 position = barrelTransform.position;
            for (int i = 0; i < shottySC.Pellets; i++)
            {
                Bullet bullet = bulletPool.RequestBullet();//bulletPool.RequestBullet();
                
                float x =  UnityEngine.Random.Range(-shottySC.Radius, shottySC.Radius) * 0.1f;
                float y =  UnityEngine.Random.Range(-shottySC.Radius, shottySC.Radius) * 0.1f;

                direction += new Vector3(x, y, 0);
                bullet.Shoot(position, direction, bTag);
            }
            InvokeShotEvent(position);

            invoke = StartCoroutine(Invoke(weaponCDTime, WeaponCoolDown));

            // searDown = ItemData.isAuto;
            // bullet.Rg.velocity = 
            // bullet.Rg.AddForce(direction * bullet.SC.ForceMagnitude, ForceMode.Impulse);
            //shooting bullet stuff
            Debug.Log("BANG");
        }

    }

    protected override bool LoadBullets(Ammo reserves)
    {

        if (reserves.Count == 0 || inChamber >= magSize)
        {
            return false;
        }
       
        if (inChamber > 0) { hasAmmo = true; }
        return true;
    }
    public override async Task ReloadTask( CancellationToken t, Ammo am)
    {
        try
        {
            isReloading = true;

             if( needEjection == false && LoadBullets(am))
            {

                await Task.Delay(itemData.reloadTime * 100);
                inChamber++;
                am.Count--;
                if(inChamber == magSize)
                {
                    InvokeIdealReload();
                }
                if (t.IsCancellationRequested) { t.ThrowIfCancellationRequested(); }
            }
            if(needEjection || roundChambered == false)
            {
                await Task.Delay(shottySC.PumpTime * 100);
                if (t.IsCancellationRequested) { t.ThrowIfCancellationRequested(); }
                
                needEjection = false;
                if(inChamber > 0)
                {
                    roundChambered = true;
                }
            }
            InvokeReloadDone();


            isReloading = false;

        }
        catch (OperationCanceledException)
        {
            isReloading = false;
        }

    }
}
