using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reloading : EquippedGun
{
    Coroutine reload;
    
    public Reloading(PInputManager parent, Inventory inventory) : base(parent, inventory)
    {
    
    }

    public override void EnterState(PInputManager stateManager)
    {
        SetItem();
        SetGun();
        inventory.Reload(ReloadDone);
    }

   void ReloadDone()
    {
        PInputManager p = manager;
        p.ChangeToState(p.EquippedGun, p.HotBarState);
    }
    public override void ExitState(PInputManager stateManager)
    {
        inventory.StopReload();
        base.ExitState(stateManager);
    }
    public override void HandleKeyDownInput(PInputManager stateManager, KeyCode keyCode)
    {
        Inventory.KeyCodeToSelect(keyCode, out int hi);
        if (slot == hi)
        {
            if (reload != null) { manager.StopCoroutine(reload); };
            stateManager.ChangeToState(stateManager.NotEquipped, stateManager.HotBarState);
        }
    }
}
