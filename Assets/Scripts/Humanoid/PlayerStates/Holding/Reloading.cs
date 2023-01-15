using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reloading : EquippedGun
{
    Coroutine reload;
    
    public Reloading(PInputManager parent, Inventory inventory) : base(parent, inventory)
    {
    
    }

    public override void EnterState()
    {
        SetItem();
        SetGun();
        inventory.CurrentGun.Shooting.Reloaded.Listen( ReloadDone);
        inventory.Reload();

        
    }

   void ReloadDone()
    {
        PInputManager p = manager;

        p.ChangeToState(p.EquippedGun, p.HotBarState);
    }
    public override void ExitState()
    {
        inventory.CurrentGun.Shooting.Reloaded.Deafen(ReloadDone);

        inventory.StopReload();
        base.ExitState();
    }
    public override void HandleKeyDownInput( KeyCode keyCode)
    {
        Inventory.KeyCodeToSelect(keyCode, out int hi);
        if (slot == hi)
        {
            if (reload != null) { manager.StopCoroutine(reload); };
            manager.ChangeToState(manager.NotEquipped, manager.HotBarState);
        }
    }
}
