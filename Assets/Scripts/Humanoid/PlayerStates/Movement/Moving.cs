using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Moving : NotMoving
{
    protected Movement movement;
    public Moving(PInputManager parent, Movement movement) : base(parent, movement)
    {
        this.movement = movement;

    }


    protected override void Update()
    {
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");
        movement.MovingFunction(moveX, moveZ);
        if (moveX == 0 && moveZ == 0)
        {
            manager.ChangeToState(manager.NotMoving, manager.MovementState);
        }
    }
  

    

  
}
