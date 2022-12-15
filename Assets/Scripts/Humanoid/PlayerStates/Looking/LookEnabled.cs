using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookEnabled : LookDisabled
{
    public LookEnabled(PInputManager parent, MouseLook look) : base(parent, look)
    {
    
    }
    public override void EnterState()
    {
        look.Enable();
    }
    public override void HandleKeyDownInput( KeyCode keyCode)
    {
        if(keyCode == KeyCode.E)
        {
            look.PickUp(manager);
        }
    }

}
