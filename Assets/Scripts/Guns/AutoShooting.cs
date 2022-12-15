using System.Collections;
using System;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.Events;
public class AutoShooting : Shooting
{
    Coroutine shootRout;
    protected override void Shoot()
    {if (canFire)
        {
            shootRout = StartCoroutine(Shooting());
        }
    }
  
    IEnumerator Shooting()
    {
        do
        {
            if (hasAmmo == false)
            {
                break;
            }
            base.Shoot();

            yield return invoke;
        } while (true);
    }
   void StopRout()
    {
        if (shootRout != null)
        {
            StopCoroutine(shootRout);
        }
    }
    protected override void TriggerRelease()
    {
        StopRout();
    }
}
