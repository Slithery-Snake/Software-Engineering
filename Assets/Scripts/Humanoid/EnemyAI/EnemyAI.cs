using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GenericBT;
using System;
using UnityEditor;
    /*
     Is Visible? >
    Turn Towards Person and IsWithinRange? >
                            HasBullets? > Shoot Else start reload
    
     
     * */
namespace EnemyStuff
{
    [Serializable] struct EnemyParts
    {
        public Transform body;
        public Transform hBarTransform;
        public Transform itemGameObject;
        public EnemySC enemySC;
        public TagManager tag;
        public Transform head;
    }
    public class EnemyAI : BehaviourTree
    {
        public static EnemyAI CreateEnemy(EnemyAI prefab, Vector3 v, ItemManager itemManager)
        {
            EnemyAI e =Instantiate(prefab, v, Quaternion.identity);
            e.Init(itemManager);
            
            return e;
        }

        public static event EventHandler EnemyKilled;
       [SerializeField] EnemyParts enemyParts;
        MonoCalls calls = new MonoCalls();
        Inventory inventory;
        LookAtNode lookNode;
        ShootNode shootNode;
        ReloadNode reloadNode;
        Health health;

     
        private void Init(ItemManager manager)
        {
            inventory = new Inventory(calls.accessors, enemyParts.itemGameObject, enemyParts.hBarTransform, enemyParts.tag.Tag);
            inventory.AddAmmo(manager.CreateAmmo(Vector3.zero, 20,1000));
            health = new Health(enemyParts.enemySC);
            health.HealthBelowZero += () => { EnemyKilled.Invoke(this, null); };
            EnemyKilled += Die;
            enemyParts.tag.AddTagsToHitBoxes(health);

            shootNode = new ShootNode(inventory, enemyParts.body, enemyParts.enemySC);
            lookNode = new LookAtNode(enemyParts.body,enemyParts.head, enemyParts.enemySC);
            reloadNode = new ReloadNode(inventory);
            inventory.AddGun(manager.CreateGun(Vector3.zero, 1,true));


        }
        void Die(object obj, EventArgs e)
        {
            Debug.Log("I AM DEAD :(");
            Destroy(gameObject);
        }
        protected virtual void Awake()
        {
            calls.awakeCall.Call();

        }
        protected override void Start()
        {   
            base.Start();
            calls.startCall.Call();
        }
        
        protected override Node SetUpTree()
        {
            Selector second = new Selector(new List<Node>{ shootNode, reloadNode});
            Sequence first = new Sequence(new List<Node> { lookNode, second });
            Node root = first;

            return root;
        }
    }
    public class LookAtNode : Node
    {
        Transform transform;
        Transform head;
        EnemySC SC;
        Transform player;
        float radianView;
        public LookAtNode(Transform transform,Transform head, EnemySC SC)
        {
            this.transform = transform;
            this.SC = SC;
            player = HumanoidManager.PlayerTransform;
            radianView = SC.ViewAngle/2 * Mathf.Deg2Rad;

            this.head = head;
        }
        int ignoreAllButSolidCoverAndPlayer = (1 << Constants.environment | 1 << Constants.playerMask);
    

        bool IsExposed()
        {
            
            RaycastHit hit;
                Physics.Linecast(head.position, player.position, out hit, ignoreAllButSolidCoverAndPlayer,QueryTriggerInteraction.Collide);
            // Debug.DrawRay(transform.position, (transform.position - player.position).normalized, Color.red, 1);
            Debug.DrawLine(head.position, hit.point, Color.black);
            if(hit.collider == null)
            {
                return false;
            } else
            {
               
                if(hit.collider.gameObject.layer == Constants.playerMask)
                {

                    return true; 
                }
            }
            return false;
        }
        
        bool IsWithinAngle()
        {
            Vector3 pPos = player.transform.position;
            Vector3 myPos = transform.position;
            double xDistance = pPos.x - myPos.x;
            double yDistance = pPos.z - myPos.z;
            double angle = Math.Atan(yDistance/xDistance );
            if(angle <= radianView)
            {
                return true;
            }
           
          //  double minX = Math.Cos(Mathf.Deg2Rad * SC.ViewAngle) * SC.ViewLength;
          //   double minY = Math.Sin(Mathf.Deg2Rad * SC.ViewAngle) * SC.ViewLength;
         

            return false;

        } 
        bool IsVisible() { if((transform.position - player.position).magnitude <= SC.ViewLength) { return true; } return false; }
        public override NodeState Evaluate()
        {
          
                if (IsVisible()/* && IsWithinAngle() */&& IsExposed() )
                {
                    Vector3 direct = -(transform.position - player.position);
                    float time = 0;
                    time += Time.deltaTime * SC.RotateSpeed;
                Vector3 look = Vector3.RotateTowards(transform.forward, direct, time, 0);
                //   transform.Rotate(Vector3.RotateTowards(player.position))
                transform.rotation = Quaternion.LookRotation(look);
                // Quaternion.Slerp(transform.rotation, lookRotation, time);
                }
            state = NodeState.RUNNING;
            return state;
        }
    }
    public class ShootNode : Node
    {
        HotBarItem g;
        Transform transform;
        EnemySC SC;
        Transform player = HumanoidManager.PlayerTransform;
        CollectiveGun gun;
        Shooting shoot;
         void RecordAdded(Inventory.IntGun h)
        {
            linkedSlot.AddFirst(h.i);
            EquipGun();
             }
        void EquipGun()
        {
           g = inventory.EquipHotBar(linkedSlot.First.Value);
            gun = inventory.CurrentGun;
            shoot = gun.Shooting;
        }
        
        void Shoot()
        {
            g.Use1();
        }
       
        LinkedList<int> linkedSlot;
        Inventory inventory;
        bool ShouldShoot()
        {
            Vector3 pPos = player.position;
            Vector3 myPos = transform.position;
            double xDistance = pPos.x - myPos.x;
            double yDistance = pPos.z - myPos.z;
            double angle = Math.Atan(yDistance/ xDistance);
            if (angle <= SC.ShootAngle)
            {
                return true;
            }




            return false;
        }
      
        public ShootNode(Inventory inventory, Transform transform, EnemySC sC)
        {
            this.inventory = inventory;
            linkedSlot = new LinkedList<int>();
            inventory.GunAdded.Listen(RecordAdded);
            this.transform = transform;
            SC = sC;
        }
        public override NodeState Evaluate()
        {
            
            if (ShouldShoot() && shoot.HasAmmo && !shoot.IsReloading && shoot.CanFire)
            {
                Shoot();
                state = NodeState.SUCCESS;
            } else
            {
                state = NodeState.FAILURE;

            }

            return state;
        }
    }
    public class ReloadNode : Node
    {
       
        Inventory inventory;
        enum RelState {doneReload, notReload, reloading }
        RelState rel = RelState.notReload;
        public ReloadNode(Inventory inventory) 
        {
            this.inventory = inventory;
        }
        void ReloadDone()
        {
            rel = RelState.notReload;
        }
       public override NodeState Evaluate()
        {   
            if(rel ==RelState.doneReload) { state = NodeState.SUCCESS; rel = RelState.notReload;  return state; }
            if(inventory.HaveAmmoForGun() && rel == RelState.notReload)
            {
                Debug.Log("reload");
                state = NodeState.RUNNING;
                rel = RelState.reloading;
                inventory.Reload(ReloadDone);

            }
            
            return state;
        }
    }
}