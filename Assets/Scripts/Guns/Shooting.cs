using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Events;
using System.Threading.Tasks;
using System.Threading;
public class Shooting : MonoBehaviour
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
    public event UnityAction IdealReloadState;
    public event UnityAction ReloadDone;
    protected WeaponData itemData;
    protected bool roundChambered = true;
    protected void InvokeIdealReload()
    {
        IdealReloadState?.Invoke();
    }

    protected void InvokeReloadDone()
    {
        ReloadDone?.Invoke();
    }

    public HotBarItem HotBar { get => hotBar; }
    public bool IsReloading { get => isReloading;  }
    public bool HasAmmo { get => hasAmmo;}
    public  WeaponData ItemData { get => itemData;  }

    public virtual bool NeedReload()
    {
        return !roundChambered || !hasAmmo;
    }

    public virtual bool GetCanFire()
    {
        return canFire;
    }

    // public new WeaponData  ItemData { get => itemData;  }
    public void SetTag(BulletTag t)
    {
        bTag = t;
    }
    public static Shooting InitShoot(Shooting r,  BulletSpawn p , Transform barrelTransform, WeaponData data, bool chamber, Creatable c)
    {
        r.barrelTransform = barrelTransform;
        r.itemData = data;

        r.InitializeShooting(p, chamber); 
        r.hotBar = HotBarItem.CreateHotBar(r.TriggerDown, null, r.TriggerRelease, null, r.itemData, r.gameObject, c);
        return r;
    }
    public bool IsEmpty()
    {
        return !hasAmmo;
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
    protected virtual void InvokeCharge()
    {
        SoundCentral.Instance.Invoke(transform.position, itemData.ChargeSound, gameObject);

    }
    protected virtual void InvokeMagSwap()
    {
        SoundCentral.Instance.Invoke(transform.position, itemData.MagSound, gameObject);
    }
   public virtual async Task ReloadTask( CancellationToken t, Ammo am)
    {
        try
        {
            isReloading = true;
            if (roundChambered == false && hasAmmo)
            {
                InvokeCharge();
                await Task.Delay(itemData.PumpTime * 100);
                if (t.IsCancellationRequested) { t.ThrowIfCancellationRequested(); }

                roundChambered = true;
                Debug.Log("chambered");
            } else
            if (LoadBullets(am) ) 
            {
                InvokeMagSwap();
                await Task.Delay(itemData.reloadTime * 100);
                if (t.IsCancellationRequested) { t.ThrowIfCancellationRequested(); }
                LoadActualBullets(am);
            } 
           
            isReloading = false;
            if (roundChambered)
            {
                InvokeIdealReload();
            }
            InvokeReloadDone();

         

        }
        catch (OperationCanceledException)
        {
            isReloading = false;
        }

    }
    void LoadActualBullets(Ammo reserves)
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
        if (inChamber > 0) { hasAmmo = true; }
        return;
    }
   protected  virtual bool LoadBullets(Ammo reserves)
    {
        int bulletsToFull = magSize - inChamber;
        if(reserves.Count <= 0 )
        {
            return false;
        }
        if (reserves.Count <= bulletsToFull)
        {
            return true;
        }

     
          if(inChamber > 0) { hasAmmo = true; }
        return true;
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
        if (GetCanFire() && hasAmmo && roundChambered )
        {
            //RaycastHit rayInfo;
            inChamber--;
            canFire = false;
            // Invoke(WeaponCoolDown, weaponCDTime);
            if (inChamber <= 0)
            {
                hasAmmo = false;
                roundChambered = false;
            }
            Bullet bullet = bulletPool.RequestBullet();//bulletPool.RequestBullet();
            Vector3 direction = barrelTransform.forward;
            // bullet.transform.rotation = barrelTransform.rotation;
            Vector3 position = barrelTransform.position;
            // bullet.transform.position = position;
            bullet.Shoot(position, direction, bTag);
            InvokeShotEvent(position);
            invoke = StartCoroutine(Invoke(weaponCDTime, WeaponCoolDown));
            // searDown = ItemData.isAuto;
            // bullet.Rg.velocity = 
            // bullet.Rg.AddForce(direction * bullet.SC.ForceMagnitude, ForceMode.Impulse);
            //shooting bullet stuff
            Debug.Log("BANG");
        }
        
    }
    protected void InvokeShotEvent(Vector3 v)
    {
        SoundCentral.Instance.Invoke(v, itemData.ShootSound, gameObject);

    }
    protected virtual void TriggerDown()
    {


       
            Shoot();
           
      

    }

  
}
