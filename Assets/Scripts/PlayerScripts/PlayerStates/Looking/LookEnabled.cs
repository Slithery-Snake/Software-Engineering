using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookEnabled : LookDisabled
{
    public LookEnabled(PInputManager parent, MouseLook look) : base(parent, look)
    {
    
    }
    public override void EnterState(PInputManager stateManager)
    {
        look.Enable();
    }
    public override void HandleKeyDownInput(PInputManager stateManager, KeyCode keyCode)
    {
        if(keyCode == KeyCode.E)
        {
            look.PickUp(stateManager);
        }
    }

}
