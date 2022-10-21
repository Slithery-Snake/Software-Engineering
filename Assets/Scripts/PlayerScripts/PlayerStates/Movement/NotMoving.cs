using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class NotMoving : PlayerState
{
    protected Movement movement;
    public NotMoving(PInputManager parent, Movement movement) : base(parent)
    {
        this.movement = movement;
    }

    public override void EnterState(PInputManager stateManager)
    {
    }

    public override void ExitState(PInputManager stateManager)
    {
    }
  

    public override void HandleKeyDownInput(PInputManager stateManager, KeyCode keyCode)
    {
        if (keyCode == KeyCode.W || keyCode == KeyCode.D || keyCode == KeyCode.S || keyCode == KeyCode.A)
        {
            stateManager.ChangeToState(stateManager.Moving, stateManager.MovementState);

        }
    }

    public override void HandleKeyPressedInput(PInputManager stateManager, KeyCode keyCode)
    {

      
    }

    public override void HandleKeyUpInput(PInputManager stateManager, KeyCode keyCode)
    {


    }


   


}