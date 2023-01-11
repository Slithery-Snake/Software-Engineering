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
    protected Task invoke;
    protected int inChamber = 0;
    protected BulletSpawn.BulletPool bulletPool;
    protected HotBarItem hotBar;
    protected BulletTag bTag = null;
    public event UnityAction IdealReloadState;
    protected WeaponData itemData;
    protected bool roundChambered = true;
    protected void InvokeIdealReload()
    {
        IdealReloadState?.Invoke();
    }

    protected void InvokeReloadDone()
    {
        reloaded.Call();
    }

    public HotBarItem HotBar { get => hotBar; }
    public bool IsReloading { get => isReloading;  }
    public bool HasAmmo { get => hasAmmo;}
    public  WeaponData ItemData { get => itemData;  }
    protected MonoCall someBulletShot = new MonoCall();
    protected MonoCall empty = new MonoCall();
    protected MonoCall reloaded = new MonoCall();
    public  IMonoCall SomeBulletShot { get => someBulletShot; }
    public  IMonoCall Empty { get => empty;  }
    public IMonoCall Reloaded { get => reloaded; }

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
    public static Shooting InitShoot(Shooting r,  BulletSpawn p , Transform barrelTransform, WeaponData data, bool chamber, Creatable c, Interactable inter)
    {
        r.barrelTransform = barrelTransform;
        r.itemData = data;

        r.InitializeShooting(p, chamber); 
        r.hotBar = HotBarItem.CreateHotBar(r.TriggerDown, null, r.TriggerRelease, null, r.itemData, r.gameObject, c, inter);
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
        SoundCentral.Instance.Invoke(transform.position, itemData.ChargeSound);

    }
    protected virtual void InvokeMagSwap()
    {
        SoundCentral.Instance.Invoke(transform.position, itemData.MagSound);
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
            reserves.SetCount( 0);
            return;
        }
        if(reserves.Infinity)
        {
            inChamber = magSize;
        }
        inChamber = magSize;
        reserves.SetCount(reserves.Count- bulletsToFull);
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
        if (reserves.Count <= bulletsToFull || reserves.Infinity)
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
    protected async Task Invoke(float time, UnityAction hi)
    {
        
        await Task.Delay((int)(time*1000));
        hi();
    }
    protected Vector3 Randomize( Vector3 direction)
    {
        float x = UnityEngine.Random.Range(-itemData.Radius, itemData.Radius);
        float y = UnityEngine.Random.Range(-itemData.Radius, itemData.Radius);

        direction += new Vector3(x, y, 0);
        return direction;
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
                empty.Call();
            }
            Bullet bullet = bulletPool.RequestBullet();//bulletPool.RequestBullet();
            Vector3 direction = barrelTransform.forward;
            
            direction = Randomize(direction);
            // bullet.transform.rotation = barrelTransform.rotation;
            Vector3 position = barrelTransform.position;
            // bullet.transform.position = position;
            bullet.Shoot(position, direction, bTag);
            InvokeShotEvent(position);
            invoke = Invoke(weaponCDTime, WeaponCoolDown);
            // searDown = ItemData.isAuto;
            // bullet.Rg.velocity = 
            // bullet.Rg.AddForce(direction * bullet.SC.ForceMagnitude, ForceMode.Impulse);
            //shooting bullet stuff
        }
        
    }

    protected void InvokeShotEvent(Vector3 v)
    {
        SoundCentral.Instance.Invoke(v, itemData.ShootSound);
        someBulletShot.Call();

    }
    protected virtual void TriggerDown()
    {


       
            Shoot();
           
      

    }

  
}
