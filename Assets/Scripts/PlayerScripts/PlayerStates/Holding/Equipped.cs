using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipped : NotEquipped
{
    int slot = 0;
    HotBarItem item;
    public Equipped(PlayerState parent, Inventory inventory, int slot) : base(parent, inventory)
    {
        this.slot = slot;       
    }
    public override void EnterState(PInputManager stateManager)
    {
       item = inventory.EquipHotBar(slot);   
    }
    public override void HandleKeyDownInput(PInputManager stateManager, KeyCode keyCode)
    {
        
        if(keyCode == KeyCode.Mouse0)
        {
            item.Use1();
        }
        if(keyCode == KeyCode.Mouse1)
        {
            item.Use2();

        }
    }
    public override void HandleKeyUpInput(PInputManager stateManager, KeyCode keyCode)
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

}
