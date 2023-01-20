using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotAttacking : PlayerState
{
    protected readonly MeleeManager melee;
    protected PlayerSC sc;
     Coroutine attackProcess;


    public NotAttacking(PInputManager manager, MeleeManager melee) : base(manager)
    {
        this.melee = melee;
        sc = manager.SC;
    }

    public override void EnterState()
    {

        
    }

    public override void ExitState()
    {
    }
    void StartSwing(bool r)
    {
        melee.SetHand(r);
        manager.ChangeToState(manager.WindUp, manager.MeleePointer);
        
    }
    public override void HandleKeyDownInput(KeyCode keyCode)
    {
        if( manager.HotBarState.State == manager.NotEquipped)
        {
            if (keyCode == KeyCode.Mouse0)
            {
                StartSwing(true);
            }
            if(keyCode == KeyCode.Mouse1)
            {
                StartSwing(false);

            }

        }
    }

    public override void HandleKeyPressedInput(KeyCode keyCode)
    {
    }

    public override void HandleKeyUpInput(KeyCode keyCode)
    {
    }
}
