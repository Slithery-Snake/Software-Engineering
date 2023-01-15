using System.Collections;
using System;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.Events;
[System.Serializable]
public class ShottyShoot : Shooting
{
    [SerializeField] ShottySC shottySC;
    protected bool needEjection = false;

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
                
              
                bullet.Shoot(position, Randomize(direction), bTag);
            }
            InvokeShotEvent(position);
       //     InvokeShotEvent(position);

            invoke = Invoke(weaponCDTime, WeaponCoolDown);

            // searDown = ItemData.isAuto;
            // bullet.Rg.velocity = 
            // bullet.Rg.AddForce(direction * bullet.SC.ForceMagnitude, ForceMode.Impulse);
            //shooting bullet stuff
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

    public override async Task ReloadTask(CancellationToken t, Ammo am)
    {
        try
        {
            isReloading = true;

            if (needEjection == false && LoadBullets(am))
            {
                Debug.Log("RELOAD");
                await Task.Delay(itemData.reloadTime * 100);
                inChamber++;
                am.SetCount(am.Count - 1);
                InvokeMagSwap();

                if (t.IsCancellationRequested) { t.ThrowIfCancellationRequested(); }
            }
            if (needEjection || roundChambered == false)
            {
                InvokeCharge();
                await Task.Delay(itemData.PumpTime * 100);
                if (t.IsCancellationRequested) { t.ThrowIfCancellationRequested(); }

                needEjection = false;
                if (inChamber > 0)
                {
                    roundChambered = true;
                }

            }

            if (inChamber > 0) { hasAmmo = true; }

            if ( roundChambered && inChamber == magSize)
            {
                InvokeIdealReload();
            }

            isReloading = false;
            InvokeReloadDone();

        }
        catch (OperationCanceledException)
        {
            Debug.Log("canceleed");

            isReloading = false;
        }

    }
}
