using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : StateManagerComponent<PInputManager>
{
    int hotBarSize = 4;
     HotBarItem[] items;
    Transform itemGameObject;
    Dictionary<int, Ammo> ammo;
    HotBarItem currentEquipped;
    Transform hotBarTransform;
    public static bool KeyCodeToSelect(KeyCode keycode, out int selectionSlot)
    {
        switch (keycode)
        {
            case KeyCode.Alpha2:
                selectionSlot = 2;
                return true;

            case KeyCode.Alpha1:
                selectionSlot = 1;
                return true;
            case KeyCode.Alpha3:
                selectionSlot = 3;
                return true;
            case KeyCode.Alpha4:
                selectionSlot = 4;
                return true;
            default:
                selectionSlot = 0;
                return false;

        }
    }
    public Inventory(PInputManager manager, List<StateManagerComponent<PInputManager>> list, Transform itemGameObject, Transform hotBarTransform) : base(manager, list)
    {
        this.itemGameObject = itemGameObject;
        items = new HotBarItem[4];
        ammo = new Dictionary<int, Ammo>();
        this.hotBarTransform = hotBarTransform;
       
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
        if(currentEquipped != null) { UnequipHotBar(); }
        HotBarItem hotBarItem;
      hotBarItem = items[index-1]; 
        Activate(hotBarItem);
        Transform itemTransform = hotBarItem.transform;
        itemTransform.SetParent(hotBarTransform);
        itemTransform.localPosition = hotBarItem.ItemData.HoldLocalSpace;
        itemTransform.localRotation = Quaternion.identity;
        currentEquipped = hotBarItem;
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
        Ammo tryGet;
        ammo.TryGetValue(am.ItemID, out tryGet);
        if (tryGet == null)
        {
            ammo.Add(am.ItemID, am);
        }
        else
        if (tryGet != null)
        {
            tryGet.Count += am.Count;
        }
        
    }
  
    public void AddHotBarItem(HotBarItem item)
    {
        for(int i = 0; i < hotBarSize; i ++)
        {
            if(items[i] ==null)
            {
                items[i] = item;
                item.transform.SetParent(itemGameObject);
                DeactivateItem(item);
                return;
            }
        }
    }

    public override void Awake()
    {
    }

    public override void FixedUpdate()
    {
    }

    public override void LateUpdate()
    {
   
    }

    public override void OnDisabled()
    {
    }

    public override void OnEnabled()
    {
    }

    public override void Start()
    {
    }

    public override void Update()
    {
    }

}
