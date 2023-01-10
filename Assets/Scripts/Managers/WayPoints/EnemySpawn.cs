using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyStuff;
public class EnemySpawn : MonoBehaviour
{
    public EnemyAI enemyPReFab;
    private void Awake()
    {
        GameManager.SpawnData.enemy.Add(this);
    }
}
