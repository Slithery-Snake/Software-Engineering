using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookDisabled : PlayerState
{
    protected MouseLook look;
    public LookDisabled(PInputManager parent, MouseLook look) : base(parent)
    {
        this.look = look;
    }

    public override void EnterState()
    {
        look.Disable();
    }

    public override void ExitState()
    {
    }
 

    public override void HandleKeyDownInput( KeyCode keyCode)
    {

    }

    public override void HandleKeyPressedInput( KeyCode keyCode)
    {

      
    }

    public override void HandleKeyUpInput( KeyCode keyCode)
    {


    }


  
}
