using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotEquipped : PlayerState
{
    protected Inventory inventory;
    public NotEquipped(PInputManager parent, Inventory inventory) : base(parent)
    {
        this.inventory = inventory;
    }

    public override void EnterState()
    {
        inventory.UnequipHotBar();
        inventory.ClearCurrentGun();
    }

    public override void ExitState()
    {
    }

  
    public override void HandleKeyDownInput( KeyCode keyCode)
    {
        int slot;
       if(Inventory.KeyCodeToSelect(keyCode, out slot))
        {
           

            if (inventory.ItemIsInSlot(slot))
            {
                
                inventory.SetSlot(slot);
                manager.ChangeToState(manager.Equipped, manager.HotBarState);
            }
       }
    }

    public override void HandleKeyPressedInput( KeyCode keyCode)
    {
    }

    public override void HandleKeyUpInput( KeyCode keyCode)
    {
    }

    
}
