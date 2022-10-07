using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Moving : NotMoving
{
    protected float moveX;
    protected float moveZ;
    protected Movement movement;
    public Moving(PlayerState parent, Movement movement) : base(parent, movement)
    {
        this.movement = movement;
    }

    public override void EnterState(PInputManager stateManager)
    {
        moveX = 0;
        moveZ = 0;

    }

    public override void ExitState(PInputManager stateManager)
    {
    }
    public override void Update(PInputManager stateManager)
    {
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");
        movement.MovingFunction(moveX, moveZ);
        if (moveX == 0 && moveZ == 0)
        {
            stateManager.ChangeToState(stateManager.NotMoving, stateManager.MovementState);
        }
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
