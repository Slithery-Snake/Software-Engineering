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
        reload = manager.StartCoroutine(Reload());
    }
    IEnumerator Reload ()
    {
        yield return new WaitForSeconds(gun.WeaponData.reloadTime);
        gun.Shooting.LoadBullets(inventory.GetAmmo(gun));
        manager.ChangeToState(manager.EquippedGun, manager.HotBarState);

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
