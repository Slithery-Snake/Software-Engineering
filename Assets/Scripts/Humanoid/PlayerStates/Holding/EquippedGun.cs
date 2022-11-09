using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedGun : Equipped
{
    protected CollectiveGun gun;
   
    public EquippedGun(PInputManager parent, Inventory inventory) : base(parent, inventory)
    {
        
    }
    public override void EnterState(PInputManager stateManager)
    {

        SetItem();
        
    }
   protected void SetGun()
    {
        gun = inventory.CurrentGun;

    }
    public override void HandleKeyDownInput(PInputManager stateManager, KeyCode keyCode)
    {
        base.HandleKeyDownInput(stateManager, keyCode);
      
        if(keyCode == KeyCode.R)
        {
            manager.ChangeToState(manager.Reloading, manager.HotBarState);
           
        }
    }
    public override void ExitState(PInputManager stateManager)
    {
    }
}
