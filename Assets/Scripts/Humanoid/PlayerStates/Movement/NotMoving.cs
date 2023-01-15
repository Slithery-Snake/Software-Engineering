using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class NotMoving : PlayerState
{
    protected float moveX;
    protected float moveZ;
    protected Movement movement;
    public NotMoving(PInputManager parent, Movement movement) : base(parent)
    {
        this.movement = movement;
    }



    public override void EnterState()
    {
        moveX = 0;
        moveZ = 0;
        manager.MonoAcessors.UpdateCall.Listen(Update);

    }

    public override void ExitState()
    {
        manager.MonoAcessors.UpdateCall.Deafen(Update);

    }
    protected virtual void Update()
    {
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");
        if (moveX != 0 || moveZ != 0)
        {
            manager.ChangeToState(manager.Moving, manager.MovementState);
        }
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