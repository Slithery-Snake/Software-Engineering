using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyStuff;
public class HumanoidManager : MonoBehaviour
{
 [SerializeField]   PInputManager playerPrefab;
  [SerializeField] EnemyStuff.EnemyAI enemyPrefab;
     PInputManager player;
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
    public void CreatePlayer(Vector3 v)
    {
        player = Instantiate(playerPrefab, v, Quaternion.identity);
        playerTransform = player.transform;
    }
  
    public static bool IsPlayerCollid(Collider collide)
    {
        if(isPlayerColliderDic.Contains(collide)) { return true; }
        return false;
    }
    
    void Start()
    {
        CreatePlayer(new Vector3(0, 8, -8));
        EnemyAI.CreateEnemy(enemyPrefab, new Vector3(0, 8, 0), itemManager);
        isPlayerColliderDic = new List<Collider>();
        foreach(Collider col in player.TagManager.hitBoxes)
        {
            isPlayerColliderDic.Add(col);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
