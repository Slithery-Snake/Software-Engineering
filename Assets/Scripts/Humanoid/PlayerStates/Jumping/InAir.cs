using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAir : Grounded
{
    public InAir(PInputManager manager, Movement movement) : base(manager, movement)
    {

    }
    void Fall()
    {
        movement.GravityApply();
        if (movement.GroundCheck())
        {
            manager.ChangeToState(manager.OnGround, manager.JumpState);
        }

    }
    public override void HandleKeyDownInput(PInputManager stateManager, KeyCode keycode)
    {
       // base.HandleKeyDownInput(stateManager, keycode);
    }
    public override void EnterState(PInputManager stateManager)
    {
        manager.MonoAcessors.UpdateCall.Listen(Fall);
    }
    public override void ExitState(PInputManager stateManager)
    {
        manager.MonoAcessors.UpdateCall.Deafen(Fall);
    }

}
