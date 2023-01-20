using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyStuff;
using UnityEngine.Events;
public class EnemyGunner : MonoBehaviour
{
  public  HumanoidManager.EnemyGunnerStruct s;
    public static event UnityAction<HumanoidManager.EnemyGunnerStruct> Point;
    private void Awake()
    {
        s.estruct.v = transform.position;
        s.estruct.degree = transform.eulerAngles.y;
        Point?.Invoke(s);
        GameManager.SpawnData.gunners.Add(s);
        gameObject.SetActive(false);

    }
}
