using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyStuff;
using System;
using UnityEngine.Events;

public class HumanoidManager : MonoBehaviour
{
 [SerializeField]   PInputManager playerPrefab;
  [SerializeField] EnemyAI enemyPrefab;
     static PInputManager player;
    static List<Collider> isPlayerColliderDic;
    static Transform playerTransform;
  
    static MonoCall playerMovedCall = new MonoCall();
    ItemManager itemManager;
    private void OnDestroy()
    {
        player.PlayerMoved -= playerMovedCall.Call;

    }
    public static Transform PlayerTransform { get => playerTransform; }
    public static IMonoCall PlayerMovedCall { get => playerMovedCall; }

    public static HumanoidManager Create(HumanoidManager inst, ItemManager itemManager)
    {
       HumanoidManager h = Instantiate(inst);
        h.itemManager = itemManager;
        return h;
    }
    [Serializable]
   public struct EnemyGunnerStruct
    {
        public EnemyStruct estruct;
        public int guid;
        public int auid;
    }
    [Serializable]
    public struct EnemyStruct
    {
      public Vector3 v;
        
        public float degree;
        public EnemyAI prefab;
    }
    public EnemyAI CreateEnemy(Vector3 v, int guid, int auid, float degree)
    {
        EnemyAI enemy = EnemyAI.CreateEnemy(enemyPrefab, v, itemManager, guid, auid, degree);
        return enemy;
    }
    public EnemyAI CreateEnemy(EnemyGunnerStruct e)
    {
        EnemyAI enemy = EnemyAI.CreateEnemy(e.estruct.prefab, e.estruct.v, itemManager, e.guid, e.auid, e.estruct.degree);
        return enemy;
    }
    public EnemyAI CreateEnemy(EnemyStruct e)
    {
        EnemyAI enemy = EnemyAI.CreateMeleeEnemy(e.prefab, e.v, e.degree);
        return enemy;
    }

    public EnemyAI CreateEnemy(Vector3 v, float degree)
    {
        EnemyAI enemy = EnemyAI.CreateMeleeEnemy(enemyPrefab, v, degree);
        return enemy;
    }
    public PInputManager CreatePlayer(Vector3 v)
    {
        if(player != null)
        {
            Destroy(player);
        }
       
        player = Instantiate(playerPrefab, v, Quaternion.identity);
        playerTransform = player.transform;
        player.PlayerMoved += playerMovedCall.Call;
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
