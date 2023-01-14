using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyStuff;
using UnityEngine.Events;
public class EnemySpawn : MonoBehaviour
{
   public HumanoidManager.EnemyStruct s;

    public static event UnityAction<HumanoidManager.EnemyStruct> Point;

    private void Awake()
    {
        s.v = transform.position;
        s.degree = transform.eulerAngles.y;
        GameManager.SpawnData.enemy.Add(s);
    }
}
