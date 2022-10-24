using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class Inventory : StateManagerComponent<PInputManager>
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
    CollectiveGun[] guns;
    MonoCall<int> equippedSlot;
    MonoCall gunEquipped;
    CollectiveGun currentGun;

    MonoCall<int> picked;
    UnityAction[] hotBar;
    public Ammo GetAmmo(CollectiveGun g)
    {
      AmmoSC ammoSC = g.Shooting.ItemData.AmmoSource;
        Ammo ammoResult;
       ammo.TryGetValue(ammoSC, out ammoResult);
        return ammoResult;
     
    }
    public IMonoCall GunEquipped { get => gunEquipped;  }
    public IMonoCall<int> Picked { get => picked; }
    public IMonoCall<int> EquippedSlot { get => equippedSlot; }
    public int CurrentSlot { get => currentSlot; }
    public HotBarItem CurrentEquipped { get => currentEquipped;}
    public CollectiveGun CurrentGun { get => currentGun;  }
    public bool ItemIsInSlot(int i)
    {
        if(hotBar[i] == null)
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
      
            UnityAction<int> tempHandler = (int i) => { guns[i] = gun;  };
        picked.Listen( tempHandler);
        AddHotBarItem(gun.Shooting.HotBar);     
        picked.Deafen( tempHandler);

    }
   
    void CheckGunEquip(int i)
    { CollectiveGun gun = guns[i];
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
    public Inventory(PInputManager manager, List<StateManagerComponent<PInputManager>> list, Transform itemGameObject, Transform hotBarTransform) : base(manager, list)
    {
        this.itemGameObject = itemGameObject;
        items = new HotBarItem[hotBarSize];
        hotBar = new UnityAction[hotBarSize];
        ammo = new Dictionary< AmmoSC, Ammo>();
        this.hotBarTransform = hotBarTransform;
            guns = new CollectiveGun[hotBarSize];
        equippedSlot = new MonoCall<int>();
        gunEquipped = new MonoCall();
        picked = new MonoCall<int>();
        equippedSlot.Listen(CheckGunEquip);

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

    

    public override void OnDisabled()
    {
    }

    public override void OnEnabled()
    {
    }

 

}
