using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavySwing : Swing
{
    public HeavySwing(PInputManager manager, MeleeManager melee) : base(manager, melee)
    {
    }
    public override void EnterState()
    {
        melee.MeleeSoureOBJ.Anim.SetTrigger("Heavy");

        melee.MeleeSoureOBJ.SetMelee(melee.MeleeSoureOBJ.HeavyType);
        melee.AttackSetUp(melee.MeleeSoureOBJ.HeavyType) ;
      
    }

    public override void ExitState()
    {
        melee.StopAttack();
        manager.StartCoroutine(CD(sc.Heavy.coolDown));
     


    }

}
