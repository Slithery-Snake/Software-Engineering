using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Threading.Tasks;
using System.Threading;
public class Inventory : StateManagerComponent
{
    struct EventEquip 
    {
        UnityAction toCall;
        EventEquip  (UnityAction toCall)
        {
            this.toCall = toCall;
        }

        public UnityAction ToCall { get => toCall; }
    }


    int hotBarSize = 4;
     HotBarItem[] items;
    Transform itemGameObject;
    Dictionary<AmmoSC, Ammo> ammo;
    HotBarItem currentEquipped;
    int currentSlot;
    Transform hotBarTransform;
    MonoCall<IntGun> gunAdded;
    //Stack<int> gunSlots s` new Stack<int>();
    Dictionary<int, CollectiveGun> guns;
    MonoCall<int> equippedSlot;
    public bool HaveAmmoForGun()
    {if (currentGun != null)
        {
            if (GetAmmo(currentGun).Count > 0)
            {
                return true;
            }
        }
        return false;
    }
    public struct IntGun
    {
        public IntGun(int i, CollectiveGun g) { this.i = i; this.g = g; }
        public int i;
        public CollectiveGun g;
    }
    MonoCall gunEquipped;
    CollectiveGun currentGun;
    BulletTag tag;
    MonoCall<int> picked;
    Task reloadingTask;
    CancellationTokenSource reloadToken;
    public void Reload(UnityAction callWhenDone)
    {
        if (currentGun != null)
        {

            reloadToken = new CancellationTokenSource();
            CancellationToken t = reloadToken.Token;
            reloadingTask = ReloadTask(callWhenDone, t);
        }
    }
    public void StopReload()
    {
        if (reloadToken != null)
        {

            reloadToken.Cancel();

        }
    }
    public void LoadCurrent()
    {
        currentGun.Shooting.LoadBullets(GetAmmo(currentGun));


    }
    async Task ReloadTask(UnityAction doneCall, CancellationToken t)
    {
        try
        {
            await Task.Delay(currentGun.WeaponData.reloadTime * 1000);
            if (t.IsCancellationRequested) { t.ThrowIfCancellationRequested(); }
            LoadCurrent();
            if(doneCall !=null)
            doneCall();
            
        } catch(OperationCanceledException) { }
        finally
        {
            reloadToken.Dispose();
            reloadToken = null;
        }
       
    }
    public Ammo GetAmmo(CollectiveGun g)
    {
      AmmoSC ammoSC = g.Shooting.ItemData.AmmoSource;
        Ammo ammoResult;
       ammo.TryGetValue(ammoSC, out ammoResult);
        return ammoResult;
    }
    public IMonoCall<int> Picked { get => picked; }
    public IMonoCall<IntGun> GunAdded { get => gunAdded; }
    public IMonoCall GunEquipped { get => gunEquipped; }
    public IMonoCall<int> EquippedSlot { get => equippedSlot; }
    public int CurrentSlot { get => currentSlot; }
    public HotBarItem CurrentEquipped { get => currentEquipped;}
    public CollectiveGun CurrentGun { get => currentGun;  }


    public bool ItemIsInSlot(int i)
    {
        if(items[i] == null)
        {
            return false;
        }
        return true;
    }
    public void ClearCurrentGun()
    {   
        currentGun = null;
    }
    public void SetSlot(int i) { currentSlot = i; }
    public void AddGun(CollectiveGun gun)
    {
      
            UnityAction<int> tempHandler = (int i) => { guns.Add(i, gun);
          gunAdded.Call(new IntGun(i, gun));

            };
        picked.Listen( tempHandler);
        gun.SetTag(tag);
        AddHotBarItem(gun.Shooting.HotBar);     
        picked.Deafen( tempHandler);

    }
   
    void CheckGunEquip(int i)
    {
        CollectiveGun gun;
        guns.TryGetValue(i, out gun);
        if(gun !=null) {
            currentGun = gun;
            if (!gunEquipped.IsNull()) { gunEquipped.Call(); }
            Debug.Log("gun hecked " + currentGun);
        }
       
    }
    public static bool KeyCodeToSelect(KeyCode keycode, out int selectionSlot)
    {
        switch (keycode)
        {
            case KeyCode.Alpha2:
                selectionSlot = 1;
                return true;

            case KeyCode.Alpha1:
                selectionSlot = 0;
                return true;
            case KeyCode.Alpha3:
                selectionSlot = 2;
                return true;
            case KeyCode.Alpha4:
                selectionSlot = 3;
                return true;
            default:
                // throw Exception
                selectionSlot = -1;
                return false;

        }
    }

    public Inventory(MonoCalls.MonoAcessors manager, Transform itemGameObject, Transform hotBarTransform, BulletTag tag) : base(manager)
    {
        this.itemGameObject = itemGameObject;
        items = new HotBarItem[hotBarSize];
        ammo = new Dictionary<AmmoSC, Ammo>();
        this.hotBarTransform = hotBarTransform;
        guns = new Dictionary<int, CollectiveGun>();
        equippedSlot = new MonoCall<int>();
        gunEquipped = new MonoCall();
        gunAdded = new MonoCall<IntGun>();
        picked = new MonoCall<int>();
        equippedSlot.Listen(CheckGunEquip);
        this.tag = tag;
    }
    void DeactivateItem(HotBarItem item)
    {
        item.gameObject.SetActive(false);
    }
    void Activate(HotBarItem item)
    {
        item.gameObject.SetActive(true);
    }

    public HotBarItem EquipHotBar(int index)
    {
        //currentSlot = index;
        
        if(currentEquipped != null) { UnequipHotBar(); } 
        
        
        HotBarItem hotBarItem;
      hotBarItem = items[index]; 
        Activate(hotBarItem);
        Transform itemTransform = hotBarItem.transform;
        itemTransform.SetParent(hotBarTransform);
        itemTransform.localPosition = hotBarItem.ItemData.HoldLocalSpace;
        itemTransform.localRotation = Quaternion.identity;
        currentEquipped = hotBarItem;
        Debug.Log("crrent " + CurrentEquipped);
                    if (!equippedSlot.IsNull()) { equippedSlot.Call(index); }

        return hotBarItem;
    }
    public void UnequipHotBar( )
    {
        if (currentEquipped != null)
        {
            DeactivateItem(currentEquipped);
            currentEquipped = null;
        }
    }
   
   
    public void AddAmmo(Ammo am)
    {
        if(ammo.ContainsKey(am.ItemData))
        {
            ammo[am.ItemData].Count += am.Count;
            
        }
        else
        {
            ammo.Add(am.ItemData, am);
        }
        
    }
  
     bool AddHotBarItem(HotBarItem item)
    {
        for(int i = 0; i < hotBarSize; i ++)
        {
            if(items[i] ==null)
            {
                items[i] = item;
                item.transform.SetParent(itemGameObject);
                DeactivateItem(item);
                if (!picked.IsNull())
                {
                    picked.Call(i);
                }
                return true;
            }
        }
        return false;
    }

    

   

 

}
