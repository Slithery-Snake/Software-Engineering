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

    public override void EnterState(PInputManager stateManager)
    {
        inventory.UnequipHotBar();
        inventory.ClearCurrentGun();
    }

    public override void ExitState(PInputManager stateManager)
    {
    }

  
    public override void HandleKeyDownInput(PInputManager stateManager, KeyCode keyCode)
    {
        int slot;
       if(Inventory.KeyCodeToSelect(keyCode, out slot))
        {
            inventory.SetSlot(slot);
            stateManager.ChangeToState(stateManager.Equipped, stateManager.HotBarState);      
               
       }
    }

    public override void HandleKeyPressedInput(PInputManager stateManager, KeyCode keyCode)
    {
    }

    public override void HandleKeyUpInput(PInputManager stateManager, KeyCode keyCode)
    {
    }

    
}
