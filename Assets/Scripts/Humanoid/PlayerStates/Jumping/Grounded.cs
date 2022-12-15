using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grounded : PlayerState
{
    protected Movement movement;
    public Grounded(PInputManager manager, Movement movement) : base(manager)
    {
        this.movement = movement;
    }
    void Falling()
    {
        if (!movement.GroundCheck())
        {
            movement.Fall();
            manager.ChangeToState(manager.Falling, manager.JumpState);
        }
    }
    public override void EnterState()
    {
        manager.MonoAcessors.UpdateCall.Listen(Falling);
    }

    public override void ExitState()
    {
        manager.MonoAcessors.UpdateCall.Deafen(Falling);

    }

    public override void HandleKeyDownInput( KeyCode keycode)
    {
        if(keycode == KeyCode.Space)
        {
            movement.Jump();
            manager.ChangeToState(manager.Falling, manager.JumpState);
        }
    }

    public override void HandleKeyPressedInput( KeyCode keyCode)
    {
    }

    public override void HandleKeyUpInput( KeyCode keyCode)
    {
    }
}
