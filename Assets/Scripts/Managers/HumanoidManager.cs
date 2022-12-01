using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyStuff;
using System;
public class HumanoidManager : MonoBehaviour
{
 [SerializeField]   PInputManager playerPrefab;
  [SerializeField] EnemyAI enemyPrefab;
     static PInputManager player;
    static List<Collider> isPlayerColliderDic;
    static Transform playerTransform;
    ItemManager itemManager;

    public static Transform PlayerTransform { get => playerTransform; }
    public static HumanoidManager Create(HumanoidManager inst, ItemManager itemManager)
    {
       HumanoidManager h = Instantiate(inst);
        h.itemManager = itemManager;
        return h;
    }
   
    public EnemyAI CreateEnemy(Vector3 v)
    {
        EnemyAI enemy = EnemyAI.CreateEnemy(enemyPrefab, v, itemManager);
        return enemy;
    }
    
    public PInputManager CreatePlayer(Vector3 v)
    {
        player = Instantiate(playerPrefab, v, Quaternion.identity);
        playerTransform = player.transform;
        return player;
    }
  
    
    public static bool IsPlayerCollid(Collider collide)
    {
        if(isPlayerColliderDic.Contains(collide)) { return true; }
        return false;
    }

 /*   private void Start()
    {
     CreatePlayer(new Vector3(0, 8, -8));
        CreateEnemy(new Vector3(0, 8, 0));
    }*/

}
