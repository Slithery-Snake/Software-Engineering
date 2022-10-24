using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Moving : NotMoving
{
    protected float moveX;
    protected float moveZ;
    public Moving(PInputManager parent, Movement movement) : base(parent, movement)
    {
       
  }

    public override void EnterState(PInputManager stateManager)
    {
        moveX = 0;
        moveZ = 0;
        manager.UpdateCall.Listen(Update);

    }

    public override void ExitState(PInputManager stateManager)
    {
        manager.UpdateCall.Deafen(Update);

    }
    void Update()
    {
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");
        movement.MovingFunction(moveX, moveZ);
        if (moveX == 0 && moveZ == 0)
        {
            manager.ChangeToState(manager.NotMoving, manager.MovementState);
        }
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


    

  
}
