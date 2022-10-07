using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookDisabled : PlayerState
{
    protected MouseLook look;
    public LookDisabled(PlayerState parent, MouseLook look) : base(parent)
    {
        this.look = look;
    }

    public override void EnterState(PInputManager stateManager)
    {
        look.Disable();
    }

    public override void ExitState(PInputManager stateManager)
    {
    }
    public override void Update(PInputManager stateManager)
    {

    }
    public override void FixedUpdate(PInputManager stateManager)
    {
    }

    public override void HandleKeyDownInput(PInputManager stateManager, KeyCode keyCode)
    {

    }

    public override void HandleKeyPressedInput(PInputManager stateManager, KeyCode keyCode)
    {

      
    }

    public override void HandleKeyUpInput(PInputManager stateManager, KeyCode keyCode)
    {


    }


    public override void LateUpdate(PInputManager stateManager)
    {
    }

}
