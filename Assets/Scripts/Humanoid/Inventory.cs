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

     HotBarItem[] items;
    Transform itemGameObject;
    Dictionary<AmmoSC, Ammo> ammo;
    HotBarItem currentEquipped;
    int currentSlot;
    Transform hotBarTransform;
    MonoCall<IntGun> gunAdded;
    //Stack<int> gunSlots s` new Stack<int>();
    Dictionary<int, CollectiveGun> guns;
    public event UnityAction<int, HotBarItemSC > EquippedSlot;
    public event UnityAction<int> UnequippedSlot;
    public event UnityAction<int, HotBarItemSC> PickedUpSlot;
    public event UnityAction<int> DroppedSlot;
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
    public void Reload()
    {
        StopReload();
        if (currentGun != null)
        {

            reloadToken = new CancellationTokenSource();
            CancellationToken t = reloadToken.Token;
            reloadingTask = currentGun.Shooting.ReloadTask( t, GetAmmo(currentGun));
        }
    }
 
    //reload, cancel, reload (cancel from first reload cancels second reload)

    public void StopReload()
    {
        if (reloadToken != null)
        {
            reloadToken.Cancel();
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
    public int CurrentSlot { get => currentSlot; }
    public HotBarItem CurrentEquipped { get => currentEquipped;}
    public CollectiveGun CurrentGun { get => currentGun;  }

    Health health;
    private readonly HandPosManage handPos;

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
        picked.Listen(tempHandler);
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
    int hotBarSlots;
    public Inventory(MonoCalls.MonoAcessors manager, Transform itemGameObject, Transform hotBarTransform, BulletTag tag, Health health, HandPosManage handPos, HumanoidSC sc) : base(manager)
    {
        this.itemGameObject = itemGameObject;
         hotBarSlots = sc.InventorySlots;
        items = new HotBarItem[hotBarSlots];
        ammo = new Dictionary<AmmoSC, Ammo>();
        this.hotBarTransform = hotBarTransform;
        guns = new Dictionary<int, CollectiveGun>();
        gunEquipped = new MonoCall();
        gunAdded = new MonoCall<IntGun>();
        picked = new MonoCall<int>();
        EquippedSlot+= (int i, HotBarItemSC s)=> { CheckGunEquip(i); };
        DroppedSlot += RemoveGun;
        this.tag = tag;
        this.health = health;
        this.handPos = handPos;
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
        Debug.Log(index);
        
        HotBarItem hotBarItem;
      hotBarItem = items[index]; 
        Activate(hotBarItem);
        Transform itemTransform = hotBarItem.transform;
        itemTransform.SetParent(hotBarTransform);
        itemTransform.localPosition = hotBarItem.ItemData.HoldLocalSpace;
        itemTransform.localEulerAngles = hotBarItem.ItemData.HoldRotation;
        currentEquipped = hotBarItem;
        handPos.SetLHand( hotBarItem.ItemData.LHandPos);
        handPos.SetRHand(hotBarItem.ItemData.RHandPos);
        EquippedSlot?.Invoke(index, hotBarItem.ItemData);
        hotBarItem.Destroyed += VoidCurrentEquipped;
        
        return hotBarItem;
    }
    void VoidCurrentEquipped()
    {
        currentEquipped.Destroyed -= VoidCurrentEquipped;

        guns[currentSlot] = null;
        items[currentSlot] = null;
        currentEquipped = null;
        handPos.SetDefault();  
            DroppedSlot?.Invoke(CurrentSlot);


    }
    public void UnequipHotBar( )
    {
        if (currentEquipped != null)
        {
            currentEquipped.Destroyed -= VoidCurrentEquipped;

            UnequippedSlot?.Invoke(currentSlot);
            DeactivateItem(currentEquipped);
            currentEquipped = null;
            handPos.SetDefault();
        }
    }
   
   
    public void AddAmmo(Ammo am)
    {
        if(ammo.ContainsKey(am.ItemData))
        {
            ammo.TryGetValue(am.ItemData, out Ammo amm);
            amm.SetCount(amm.Count + am.Count);

            
        }
        else
        {
            ammo.Add(am.ItemData, am);
        }
        
    }
    void RemoveGun(int i)
    {
        guns.Remove(i);
    }
    public void DropCurrent()
    {if (currentEquipped != null)
        {

            currentEquipped.transform.SetParent(null);
            currentEquipped.OgItem.OnFloor();
            
            currentEquipped.Interact.enabled = true;
            VoidCurrentEquipped();


        }
    }
 public void TryAddHotBar(HotBarItem i)
    {
        AddHotBarItem(i);
    }
     bool AddHotBarItem(HotBarItem item)
    {
        for(int i = 0; i < hotBarSlots; i ++)
        {
            if(items[i] ==null)
            {
                items[i] = item;
                item.transform.SetParent(itemGameObject, true);
                item.OgItem.Held();
                item.Interact.enabled = false;
                DeactivateItem(item);
                PickedUpSlot?.Invoke(i, item.ItemData);

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
