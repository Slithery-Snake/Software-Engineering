using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipped : NotEquipped
{
    protected int slot = 0;
    protected HotBarItem item;
    public Equipped(PInputManager parent, Inventory inventory) : base(parent, inventory)
    {
           
    }
    public override void EnterState()
    {
        inventory.GunEquipped.Listen(GG);

        inventory.EquipHotBar(inventory.CurrentSlot);
        SetItem();
    }
    protected void SetItem()
    {
       
        item = inventory.CurrentEquipped;
     

        slot = inventory.CurrentSlot;
       // Debug.Log(inventory.CurrentEquipped);
       // Debug.Log(item);
    }
    public override void ExitState()
    {
        inventory.GunEquipped.Deafen(GG);
    }
    void GG ()
    {
        manager.ChangeToState(manager.EquippedGun, manager.HotBarState);
    }
    protected void KeyDownUse(KeyCode keyCode)
    {
        if (keyCode == KeyCode.Mouse0)
        {
            item.Use1();
        }
        if (keyCode == KeyCode.Mouse1)
        {
            item.Use2();

        }
    }
    protected void KeyUpUse(KeyCode keyCode)
    {
        if (keyCode == KeyCode.Mouse0)
        {
            item.Use1Up();

        }
        if (keyCode == KeyCode.Mouse1)
        {
            item.Use2Up();

        }
    }
    public override void HandleKeyDownInput( KeyCode keyCode)
    {
        KeyDownUse(keyCode);

        // int hi;
        if (Inventory.KeyCodeToSelect(keyCode, out int hi))
        {
            if (slot == hi && inventory.ItemIsInSlot(hi))
            {
                manager.ChangeToState(manager.NotEquipped, manager.HotBarState);
            }
        }
    }
    public override void HandleKeyUpInput( KeyCode keyCode)
    {
        KeyUpUse(keyCode);
       
    }

}
