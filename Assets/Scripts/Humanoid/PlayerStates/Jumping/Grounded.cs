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
    public override void EnterState(PInputManager stateManager)
    {
        manager.MonoAcessors.UpdateCall.Listen(Falling);
    }

    public override void ExitState(PInputManager stateManager)
    {
        manager.MonoAcessors.UpdateCall.Deafen(Falling);

    }

    public override void HandleKeyDownInput(PInputManager stateManager, KeyCode keycode)
    {
        if(keycode == KeyCode.Space)
        {
            movement.Jump();
            manager.ChangeToState(manager.Falling, manager.JumpState);
        }
    }

    public override void HandleKeyPressedInput(PInputManager stateManager, KeyCode keyCode)
    {
    }

    public override void HandleKeyUpInput(PInputManager stateManager, KeyCode keyCode)
    {
    }
}
