using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GenericBT;
using System;
using UnityEditor;
using UnityEngine.Events;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.AI;

    /*
     Is Visible? >
     Is Visible? >
    Turn Towards Person and IsWithinRange? >
                            HasBullets? > Shoot Else start reload
    
     
     * */
namespace EnemyStuff
{
    [Serializable] struct EnemyParts
    {
        public Transform hBarTransform;
        public Transform itemGameObject;
        public EnemySC enemySC;
        public TagManager tag;
        public HumanoidParts hParts;
    }
    public class EnemyAI : BehaviourTree, StatusEffect.StatusEffectManager.IStatusEeffectable, StatusEffect.StatusEffectManager.IStunnable
    {
        public static EnemyAI CreateEnemy(EnemyAI prefab, Vector3 v, ItemManager itemManager, int gid, int aid, float degreeDirect, EnemySC sc = null)
        {
            EnemyAI e =Instantiate(prefab, v, Quaternion.Euler(new Vector3(0,degreeDirect,0)));
            if(sc !=null) { e.enemyParts.enemySC = sc; }
            e.Init();
            e.InitWeap(gid, aid, itemManager);

            return e;
            
        }
        
        public static EnemyAI CreateMeleeEnemy(EnemyAI prefab, Vector3 v, float degreeDirect, EnemySC sc = null)
        {
            EnemyAI e = Instantiate(prefab, v, Quaternion.Euler(new Vector3(0, degreeDirect, 0)));
            if (sc != null) { e.enemyParts.enemySC = sc; }

            e.Init();
            e.InitMelee();
            
            return e;
        }

        public static event UnityAction EnemyKilled;
       [SerializeField] EnemyParts enemyParts;
        [SerializeField] NavMeshAgent pathfinder;
        MonoCalls calls = new MonoCalls();
        Inventory inventory;
        LookAtNode lookNode;
        ShootNode shootNode;
        ReloadNode reloadNode;
        Health health;
        StatusEffect.StatusEffectManager status;
        HandPosManage handPos;
        MeleeManager melee;
        Follow follow;
        public StatusEffect.StatusEffectManager Status => status;
        Node second;
        void Die()
        {
            health.HealthBelowZero -= Execute;
            lookNode.Dispose();
            follow.Dispose();
            Destroy(gameObject);
           

        }
        public bool IsExposed()
        {
           return lookNode.IsExposed();
        }
        private void OnDestroy()
        {
            HumanoidManager.listOfEnemies.Remove(this);
            calls.destroyed.Call();
            health.HealthBelowZero -= Execute;
            follow.Dispose();
            reloadNode?.Dispose();
            melee?.Dispose();
        }
        private void Init()
        {
            handPos = new HandPosManage(calls.accessors, enemyParts.hParts.Parts1.rHand, enemyParts.hParts.Parts1.lHand);
            inventory = new Inventory(calls.accessors, enemyParts.itemGameObject, enemyParts.hBarTransform, enemyParts.tag.Tag, handPos, enemyParts.enemySC);
            health = new Health(enemyParts.enemySC);
            health.HealthBelowZero += Execute;
            
            enemyParts.tag.AddTagsToHitBoxes(health, this);
            lookNode = new LookAtNode(enemyParts.hParts.Parts1.body, enemyParts.hParts.Parts1.head, enemyParts.enemySC, health);
            
            status = new StatusEffect.StatusEffectManager(calls.accessors, health, this, enemyParts.tag.tag);
            
            follow = new Follow(pathfinder, lookNode, enemyParts.enemySC, enemyParts.hParts.Parts1.body, HumanoidManager.GetPlayerTransform());


        }

        void Execute()
        {
            
            EnemyKilled.Invoke();
            Die();
        }
        void InitWeap(int weap, int amm, ItemManager manager)
        {
            Ammo am = manager.CreateAmmo(new ItemManager.AmmoStruct(Vector3.zero, amm, 1000, true));
            inventory.AddAmmo(am);
            
            Debug.Log(HumanoidManager.GetPlayerTransform());
            reloadNode = new ReloadNode(inventory);
            shootNode = new ShootNode(inventory, enemyParts.hParts.Parts1.body, enemyParts.enemySC.ShootAngle, IsExposed, HumanoidManager.GetPlayerTransform(), enemyParts.enemySC.ShootDistance);
            Debug.Log(HumanoidManager.GetPlayerTransform());
            inventory.AddGun(manager.CreateGun(new ItemManager.GunStruct(Vector3.zero,weap, true)));
           second = new Selector(new List<Node> { shootNode, reloadNode });


        }
        void InitMelee()
        {
            melee = new MeleeManager(calls.accessors, enemyParts.enemySC, enemyParts.hParts.Parts1.lHandCol, enemyParts.hParts.Parts1.rHandCol, enemyParts.hParts.Parts1.lHandAnim, enemyParts.hParts.Parts1.rHandAnim, enemyParts.tag.tag);
            meleeNode = new MeleeNode(enemyParts.hParts.Parts1.body, enemyParts.enemySC, melee);
            canMelee = new ShouldMelee(enemyParts.hParts.Parts1.body, enemyParts.enemySC);



            second = new Sequence(new List<Node> { canMelee, meleeNode });
            
        }
       
      
        protected virtual void Awake()
        {
            calls.awakeCall.Call();
            HumanoidManager.listOfEnemies.Add(this);
            

        }
        protected override void Start()
        {   
            base.Start();
            calls.startCall.Call();
        }
        
        MeleeNode meleeNode;
        ShouldMelee canMelee;
        protected override Node SetUpTree()
        {
            Sequence first = new Sequence(new List<Node> { lookNode, second });
            Node root = first;

            return root;
        }

        public void Stun(double stunTime)
        {
            Task t = Pause((int)(stunTime*1000));
        }
        async Task Pause(int time)
        {
            Pause();
            follow.CeaseFollow();
            await Task.Delay(time);
            StartEval();
            follow.ResumeFollow();

        }

    }
  
    public class Follow: IDisposable
    {
        protected readonly NavMeshAgent navMesh;
        private readonly LookAtNode lookNode;
        protected readonly Transform body;
        Transform target;
        float stopDistance;
        public Follow(NavMeshAgent navMesh, LookAtNode lookNode, EnemySC sc, Transform body, Transform target)
        {
            this.navMesh = navMesh;
            this.lookNode = lookNode;
            this.body = body;

            lookNode.FirstSighted += StartFollow;
            navMesh.stoppingDistance = sc.FollowStoppingDistance;
            stopDistance = sc.FollowStoppingDistance;
            navMesh.speed = sc.Speed;
            this.target = target;


            //  navMesh.updatePosition = false;
        }



        protected virtual void StartFollow()
        {
            lookNode.FirstSighted -= StartFollow;
            Debug.Log("start follow");
            HumanoidManager.PlayerMovedCall.Listen( SetPlayerDestination);
            SetPlayerDestination();

        }
        public void ResumeFollow()
        {
            navMesh.isStopped = false;
        }
        public void CeaseFollow()
        {
            navMesh.isStopped = true;
        }
   
        void SetPlayerDestination()
        {
            Vector3 pPos = target.position;
          //  if ((body.position - pPos).magnitude > stopDistance)
         //   {
           //     Debug.Log("setting Destination");

                navMesh.SetDestination(pPos);
        //    }

        }

        public void Dispose()
        {
            HumanoidManager.PlayerMovedCall.Deafen( SetPlayerDestination);
            lookNode.FirstSighted -= StartFollow;


        }
    }
   
    public class MeleeNode : Node
    {
        private readonly Transform transform;
        private readonly EnemySC sc;
        private readonly MeleeManager melee;
        MeleeStats currentStats;
        bool light;
        public MeleeStats CurrentStats { get => currentStats;  }

        public MeleeNode(Transform transform, EnemySC sc, MeleeManager melee)
        {
            this.transform = transform;
            this.sc = sc;
            this.melee = melee;
            state = NodeState.FAILURE;
        }

        void StartMelee()
        {
            state = NodeState.RUNNING;
            Task task = Wait();

        }

       async Task Wait()
        {
            bool whichHand;
            int toss = UnityEngine.Random.Range(0, 2);
           int lightToss = UnityEngine.Random.Range(1, 5);
            light = lightToss != 1;
            whichHand = toss == 1;
            melee.SetHand(whichHand);
            string triggerType = light ? "Light" : "Heavy";
            MeleeManager.MeleeType type = light ? melee.MeleeSoureOBJ.LightType : melee.MeleeSoureOBJ.HeavyType;
            currentStats = type.Stats;
            melee.MeleeSoureOBJ.Anim.SetTrigger("WindUp");

            await Task.Delay((int)(currentStats.windUp * 1000));
            melee.AttackSetUp(type);
            
            melee.MeleeSoureOBJ.Anim.SetTrigger(triggerType);


            await Task.Delay((int)(currentStats.length * 1000));
            melee.StopAttack();
            await Task.Delay((int)(CurrentStats.coolDown* 1000));
            state = NodeState.SUCCESS;

        }
        public override NodeState Evaluate()
        {
            if(state != NodeState.RUNNING)
            {
                StartMelee();
            }
            return state;
        }
    }

    public class ShouldMelee : Node
    {
        private readonly Transform transform;
        private readonly EnemySC sc;
        Transform pPos;
        public ShouldMelee(Transform transform, EnemySC sc)
        {
            this.transform = transform;
            this.sc = sc;
            this.transform = transform;
            pPos = HumanoidManager.GetPlayerTransform();
            
        }
        bool CanMelee()
        {
            if (sc.MeleeLength >= Vector3.Magnitude(pPos.position - transform.position))
            {
                return true;
            }
            return false;
        }
        public override NodeState Evaluate()
        {
            state = NodeState.FAILURE;
            if(CanMelee())
            {
                state = NodeState.SUCCESS;
            }
            return state;
        }
    }
    public class LookAtNode : Node, IDisposable
    {
        Transform transform;
        Transform head;
        EnemySC SC;
        private readonly Health health;
        Transform player;
        bool isExposed;
        public LookAtNode(Transform transform,Transform head, EnemySC SC, Health health = null)
        {
            this.transform = transform;
            this.SC = SC;
            this.health = health;
            player = HumanoidManager.GetPlayerTransform();
            //radianView = SC.ViewAngle/2 * Mathf.Deg2Rad;

            this.head = head;
            EvalFunction = FirstEval;
            //   EvalFunction = DefaultEvaluation;
            if (health != null)
            {
                health.HealthChanged += FirstHit;
            }
            
        }
        int ignoreAllButSolidCoverAndPlayer = (1 << Constants.environment | 1 << Constants.playerMask);
    void FirstHit(float f)
        {
            Debug.Log("firstHit");
            EvalFunction = DefaultEvaluation;
            FirstSighted?.Invoke();
            if (health != null)
            {
                health.HealthChanged -= FirstHit;
            }

        }
        NodeState FirstEval()
        {
           

            if (ExtraSensory() || (IsVisible() && IsExposed()) && IsWithinAngle())
            {
                FirstSighted?.Invoke();
                
                EvalFunction = DefaultEvaluation;
                if (health != null)
                {
                    health.HealthChanged -= FirstHit;
                }
            }

            return NodeState.RUNNING;
        }
        public bool IsExposed()
        {
            
            RaycastHit hit;
                Physics.Linecast(head.position, player.position, out hit, ignoreAllButSolidCoverAndPlayer,QueryTriggerInteraction.Collide);
            // Debug.DrawRay(transform.position, (transform.position - player.position).normalized, Color.red, 1);
            Debug.DrawLine(head.position, hit.point, Color.black);
            if(hit.collider == null)
            {
                isExposed = false;
                return false;
            } else
            {
               
                if(hit.collider.gameObject.layer == Constants.playerMask)
                {
                    isExposed = true;

                    return true; 
                }
            }
            isExposed = false;

            return false;
        }
        
        bool ExtraSensory()
        {
            Vector3 pPos = player.transform.position;
            Vector3 myPos = transform.position;
            if(Vector3.Magnitude(pPos - myPos) <= SC.ExtraSensoryLength)
            {
                return true;
            }
            return false;
        }
        bool IsWithinAngle()
        {
            Vector3 pPos = player.transform.position;
            Vector3 myPos = transform.position;
            Vector3 myRot = transform.eulerAngles;
            
            //        double angle = Math.Atan(yDistance/xDistance );
            float angle = Vector3.Angle(pPos - myPos, transform.forward);

            if (angle <= SC.ViewAngle)
            {
                return true;
            }
           
          //  double minX = Math.Cos(Mathf.Deg2Rad * SC.ViewAngle) * SC.ViewLength;
          //   double minY = Math.Sin(Mathf.Deg2Rad * SC.ViewAngle) * SC.ViewLength;
        
            return false;

        }
        public event UnityAction FirstSighted;

        Func<NodeState> EvalFunction;
       NodeState DefaultEvaluation()
        {

            if (ExtraSensory() || (IsVisible()  && IsExposed()))
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
        bool IsVisible() { if((transform.position - player.position).magnitude <= SC.ViewLength) { return true; } return false; }
        public override NodeState Evaluate()
        {
          
            return EvalFunction();
        }

        public void Dispose()
        {
            if (health != null)
            {
                health.HealthChanged -= FirstHit;
            }
        }
    }
    
    public class ShootNode : Node
    {
        HotBarItem g;
        Transform transform;
        private readonly float sangle;
        private readonly Func<bool> exposeCheck;
        protected  Transform target;
        CollectiveGun gun;
        Shooting shoot;
        int shootDistance;
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
            Vector3 pPos = target.position;
            Vector3 myPos = transform.position;
            
            float angle = Vector3.Angle(pPos - myPos, transform.forward);

            if (angle <= sangle && exposeCheck() && (myPos - pPos).magnitude < shootDistance)
            {
                return true;
            }

            //  double minX = Math.Cos(Mathf.Deg2Rad * SC.ViewAngle) * SC.ViewLength;
            //   double minY = Math.Sin(Mathf.Deg2Rad * SC.ViewAngle) * SC.ViewLength;

            return false;




        }
        public void SetTarget(Transform t)
        {
            target = t;
        }
        public ShootNode(Inventory inventory, Transform transform, float sangle, Func<bool> m, Transform hi, int sc)
        {
            this.inventory = inventory;
            linkedSlot = new LinkedList<int>();
            inventory.GunAdded.Listen(RecordAdded);
            this.transform = transform;
            this.sangle = sangle;
            this.exposeCheck = m;
            this.target = hi;
            this.shootDistance = sc;
            
        }
        
        public override NodeState Evaluate()
        {
            
            if (ShouldShoot() && !shoot.IsReloading && shoot.GetCanFire() )
            {
                Shoot();
                state = NodeState.SUCCESS;
            } else
            {
                state = NodeState.RUNNING;

            }
            if(shoot.NeedReload())
            {
                state = NodeState.FAILURE;

            }
            return state;
        }
    }
    public class ReloadNode : Node, IDisposable
    {   
       
        Inventory inventory;
        enum RelState {doneReload, notReload, reloading }
        RelState rel = RelState.notReload;
        bool completeIdeal = false;
        Shooting myGun;
        public ReloadNode(Inventory inventory) 
        {
            this.inventory = inventory;

        }
        void ReloadDone()
        {

            rel = RelState.doneReload;
            completeIdeal = false;
            inventory.CurrentGun.Shooting.IdealReloadState -= ReloadDone;
            
            return;
        }
        //once reload is done, if not ideal, reload again
        void ReloadAgain()
        { Shooting shooting = inventory.CurrentGun.Shooting;
            if (!shooting.IsEmpty() && !completeIdeal)
            {

                rel = RelState.doneReload;

                inventory.CurrentGun.Shooting.Reloaded.Deafen(ReloadAgain);
                inventory.CurrentGun.Shooting.IdealReloadState -= ReloadDone;


            }
            if (rel != RelState.doneReload)
            {
                completeIdeal = true;
                inventory.Reload();
            } else
            {
                inventory.CurrentGun.Shooting.Reloaded.Deafen( ReloadAgain);

            }

        }
        public void Dispose()
        {
            inventory.CurrentGun.Shooting.IdealReloadState -= ReloadDone;

        }
        public override NodeState Evaluate()
        {   
            if(rel ==RelState.doneReload) { state = NodeState.SUCCESS; rel = RelState.notReload;  return state; }
            if(inventory.HaveAmmoForGun() && rel == RelState.notReload)
            {
                state = NodeState.RUNNING;
                rel = RelState.reloading;
                inventory.Reload();
               
                inventory.CurrentGun.Shooting.Reloaded.Listen(ReloadAgain);
                inventory.CurrentGun.Shooting.IdealReloadState += ReloadDone;
            

            }

            return state;
        }
    }
}