using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedGun : Equipped
{
    protected CollectiveGun gun;
   
    public EquippedGun(PInputManager parent, Inventory inventory) : base(parent, inventory)
    {
        
    }
    public override void EnterState()
    {

        SetItem();
        
    }
   protected void SetGun()
    {
        gun = inventory.CurrentGun;

    }
    public override void HandleKeyDownInput( KeyCode keyCode)
    {
        base.HandleKeyDownInput( keyCode);
      
        if(keyCode == KeyCode.R)
        {
            manager.ChangeToState(manager.Reloading, manager.HotBarState);
           
        }
    }
    public override void ExitState()
    {
    }
}
