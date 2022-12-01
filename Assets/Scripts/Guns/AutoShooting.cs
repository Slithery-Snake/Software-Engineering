using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoShooting : Shooting
{
    Coroutine shootRout;
    protected override void Shoot()
    {
        shootRout = StartCoroutine(Shooting());
    }
  
    IEnumerator Shooting()
    {
        base.Shoot();

        yield return invoke;
    }
    protected override void TriggerRelease()
    {
        if(shootRout !=null)
        {
            StopCoroutine(shootRout);
        }
    }
}
