    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyStuff;
using UnityEngine.AI;
using GenericBT;
using UnityEngine.UI;

using TMPro;
using System;
public class AllyAgent : BehaviourTree
{
    [Serializable]
    struct AllyParts
    {
        public Transform hBarTransform;
        public Transform itemGameObject;
        public AllySC SC;
        public TagManager tag;
        public HumanoidParts hParts;
    }
    [SerializeField] AllyParts parts;

    [SerializeField] NavMeshAgent pathfinder;
    MonoCalls calls = new MonoCalls();
    Inventory inventory;
    ShootNode shootNode;
    TrackTarget targetTrack;
    FindTarget findTarget;
    ReloadNode reloadNode;
    HandPosManage handPos;
    MeleeManager melee;
    [SerializeField] TextMeshPro nameTag;
    int ignoreAllButSolidCoverAndPlayer = (1 << Constants.environment | 1 << Constants.enemyMask | 1<< Constants.agentMask);
    Transform head;
    Transform target = null;
    private void OnDestroy()
    {
        calls.destroyed.Call();
        reloadNode?.Dispose();
        melee?.Dispose();
        HumanoidManager.PlayerMovedCall.Deafen(StalkPlayer);
    }
    void StalkPlayer()
    {
        pathfinder.SetDestination(HumanoidManager.GetPlayerTransform().position);
    }
    public static AllyAgent Create(AllyAgent prefab, Vector3 v, ItemManager itemManager, int gid, int aid, float degreeDirect, string name)
    {
        AllyAgent e = Instantiate(prefab, v, Quaternion.Euler(new Vector3(0, degreeDirect, 0)));
        e.Init();
        e.InitWeapon(gid, aid, itemManager);
        e.nameTag.text = name;
        HumanoidManager.PlayerMovedCall.Listen(e.StalkPlayer);

        return e;

    }
  
    bool IsExposed()
    {
        Debug.Log(target);
        return SeeExposed(target);
    }
    bool SeeExposed(Transform target)
    {
        RaycastHit hit;
        Physics.Linecast(head.position, target.position, out hit, ignoreAllButSolidCoverAndPlayer, QueryTriggerInteraction.Collide);
        if (hit.transform?.gameObject.layer == Constants.enemyMask)
        {
            return true;
        }
        return false;
    }
    public class TrackTarget : FindTarget
    {
        private readonly Transform transform;
        private readonly AllySC sc;

        public TrackTarget(AllyAgent self, Transform transform, AllySC sc, ShootNode s) : base(self, s)
        {
            this.transform = transform;
            this.sc = sc;
        }

        public override NodeState Evaluate()
        {
            state = NodeState.FAILURE;
            if(self.target == null)
            {
                return state;
            }
            Vector3 direct = -(transform.position - self.target.position);
            float time = 0;
            time += Time.deltaTime * sc.RotateSpeed;
            Vector3 look = Vector3.RotateTowards(transform.forward, direct, time, 0);
            //   transform.Rotate(Vector3.RotateTowards(player.position))
            transform.rotation = Quaternion.LookRotation(look);
            // Quaternion.Slerp(transform.rotation, lookRotation, time);
            if (self.IsExposed())
            {
                state = NodeState.SUCCESS;
            } else
            {
                self.target = null;


            }
            return state;
        }
    }
    public class FindTarget : Node
    {
        protected readonly AllyAgent self;
        private readonly ShootNode shoot;

        public FindTarget( AllyAgent self, ShootNode shoot)
        {
            this.self = self;
            this.shoot = shoot;
        }
        public override NodeState Evaluate()
        {
            state = NodeState.FAILURE;
            if(DetermineTarget())
            {
                state = NodeState.SUCCESS;
            } 
            return state;
        }

        bool DetermineTarget()
        {
            for(int i = 0; i < HumanoidManager.listOfEnemies.Count; i ++)
            {
                Transform t = HumanoidManager.listOfEnemies[i].transform;

              if(self.SeeExposed(t))
                {
                    self.target = t;
                    shoot.SetTarget(t);
                    return true;
                }
               
            }
            return false;
        }
       
    }
    private void Init()
    {
       
        handPos = new HandPosManage(calls.accessors, parts.hParts.Parts1.rHand, parts.hParts.Parts1.lHand);
        inventory = new Inventory(calls.accessors, parts.itemGameObject, parts.hBarTransform, parts.tag.Tag, handPos, parts.SC);
        head = parts.hParts.Parts1.head;
        pathfinder.stoppingDistance = parts.SC.FolloDistance;


    }
   Transform Target { get { return target; }  }

    public int IgnoreAllButSolidCoverAndPlayer { get => ignoreAllButSolidCoverAndPlayer; set => ignoreAllButSolidCoverAndPlayer = value; }

    void InitWeapon(int gid, int aid, ItemManager manager)
    {
        Ammo am = manager.CreateAmmo(new ItemManager.AmmoStruct(Vector3.zero, aid, 1000, true));
        inventory.AddAmmo(am);
        shootNode = new ShootNode(inventory,parts.hParts.Parts1.body,parts.SC.ShootAngle,IsExposed, Target);
        reloadNode = new ReloadNode(inventory);
        inventory.AddGun(manager.CreateGun(new ItemManager.GunStruct(Vector3.zero, gid, true)));
        targetTrack = new TrackTarget(this, parts.hParts.Parts1.body, parts.SC, shootNode);
        findTarget = new FindTarget(this, shootNode);
    }
    Sequence second;
    Selector first;
    protected override Node SetUpTree()
    {
        second = new Sequence(new List<Node>() { targetTrack, new Selector(new List<Node>() { shootNode, reloadNode }) });
        first = new Selector(new List<Node>() { second, findTarget });
        return first;
     }
}
