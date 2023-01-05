using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Swing : NotAttacking
{
    protected bool cDoff = true;
    protected IEnumerator CD(float wait)
    {
        cDoff = false;
        yield return new WaitForSeconds(wait);
        cDoff = true;
        Debug.Log("cd over");
    }
    public Swing(PInputManager manager, MeleeManager melee) : base(manager, melee)
    {
        
       
    }
    public override void EnterState()
    {
        Debug.Log("light triggered");
        melee.MeleeSoureOBJ.Anim.SetTrigger("Light");

        melee.AttackSetUp(melee.MeleeSoureOBJ.LightType);

    }
   
    public override void ExitState()
    {
        melee.StopAttack();
        manager.StartCoroutine(CD(sc.Light.coolDown));
        Debug.Log("light done");

    }
    public override void HandleKeyDownInput(KeyCode keyCode)
    {
     
    }
}
