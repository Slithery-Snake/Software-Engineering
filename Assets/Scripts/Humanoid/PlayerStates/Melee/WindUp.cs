using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindUp : NotAttacking
{
    Coroutine windupRout;
    MeleeStats whatStats;
    NotAttacking nextState;
    Coroutine attackProcess;
    bool heavy = false;
    float startTime;
    public WindUp(PInputManager manager, MeleeManager melee) : base(manager, melee)
    {
        whatStats = sc.Light;
        nextState = manager.Swinging;
    }
    IEnumerator AttackProcess(float upTime)
    {
        yield return new WaitForSecondsRealtime(upTime);
        manager.ChangeToState(manager.NotAttackingState, manager.MeleePointer);
    }
    void StartAttackProcess(float upT)
    {
        attackProcess = manager.StartCoroutine(AttackProcess(upT));
    }
    public override void EnterState()
    {
        melee.MeleeSoureOBJ.Anim.SetTrigger("WindUp");

       windupRout = manager.StartCoroutine(WindUpRout(whatStats.windUp, nextState));

    }
    IEnumerator WindUpRout(float wait, NotAttacking next )
    {
         startTime = Time.time;

        yield return new WaitForSecondsRealtime(wait);
        manager.ChangeToState(next, manager.MeleePointer);
        StartAttackProcess(whatStats.length);
        
    }
   void StopWindUp()
    {
        if (windupRout != null)
        {
            manager.StopCoroutine(windupRout);
        }

    }
    public override void ExitState()
    {
        this.whatStats = sc.Light;
        nextState = manager.Swinging;
        StopWindUp();
        heavy = false;
    }
    public override void HandleKeyDownInput(KeyCode keyCode)
    {
        if (!heavy && (keyCode == KeyCode.Mouse0 || keyCode == KeyCode.Mouse1))
        { 
            
            StopWindUp();
            heavy = true;
            this.whatStats = sc.Heavy;
            nextState = manager.SwingingHeavy;
            float wait = whatStats.windUp - (Time.time - startTime);
            windupRout = manager.StartCoroutine(WindUpRout(wait, nextState));

        }
    }
}
