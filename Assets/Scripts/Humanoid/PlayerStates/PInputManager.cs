using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Reflection;
using System.Linq;
using Unity;
using System.Collections;
public class MonoCall<T> : IMonoCall<T>
{
  
    UnityAction<T> toCall;
    public void Call(T h)
    {
        if(toCall !=null)
        toCall(h);
    }
    public bool IsNull()
    {
        if (toCall == null)
        {
            return true;
        }
        return false;
    }

    public void Listen(UnityAction<T> add)
    {
        toCall += add;
    }
    public void Deafen(UnityAction<T> remove)
    {
        toCall -= remove;
    }
}

public interface IMonoCall<T>
{

    public void Listen(UnityAction<T> add);
    public void Deafen(UnityAction<T> remove);
}
public class MonoCall : IMonoCall
{

UnityAction toCall;
    public void Call()
    {
        if (toCall != null)
        {
            toCall();
        }
    }
    public bool IsNull()
    {
        if (toCall == null)
        {
            return true;
        }
        return false;
    }
    public void Listen(UnityAction add)
    {
        toCall += add;
     //   Debug.Log("to add "+add +" is true?? " + IsNull());
    }
    public void Deafen(UnityAction remove)
    {
        toCall -= remove;
    }
}
public class MonoCalls
{

    public MonoCall awakeCall = new MonoCall();
    public MonoCall startCall = new MonoCall();
    public MonoCall updateCall = new MonoCall();
    public MonoCall fixedUpdateCall = new MonoCall();
    public MonoCall lateUpdateCall = new MonoCall();
    public MonoCall destroyed = new MonoCall();
    public MonoAcessors accessors;
    public MonoCalls ()
    {
         accessors = new MonoAcessors(this);



    }


    public class MonoAcessors
    {
        public MonoAcessors  (MonoCalls f)
        {
            calls = f;
        }
        MonoCalls calls;
        public IMonoCall Destroyed { get => calls.destroyed; }
        public IMonoCall AwakeCall { get => calls.awakeCall; }
        public IMonoCall StartCall { get => calls.startCall; }
        public IMonoCall UpdateCall { get => calls.updateCall; }
        public IMonoCall FixedUpdateCall { get => calls.fixedUpdateCall; }
        public IMonoCall LateUpdateCall { get => calls.lateUpdateCall; }
    }
}

public interface IMonoCall
{

    public void Listen(UnityAction add);
    public void Deafen(UnityAction remove);
}
[Serializable]
public class TagManager
{
    public Collider[] hitBoxes;
    public BulletTag tag;
    public  void AddTagsToHitBoxes(IShootable shootable, StatusEffect.StatusEffectManager.IStatusEeffectable status)
    {
       for(int i = 0; i < hitBoxes.Length; i ++)
        {
            ShootBox.Create(tag, shootable, hitBoxes[i], status);
        }
    }
     public BulletTag Tag { get => tag; }
}
public class PInputManager : StateManagerIN, StatusEffect.StatusEffectManager.IStunnable, StatusEffect.StatusEffectManager.IStatusEeffectable, SourceProvider
{
 
    [SerializeField]TagManager tagManager;


    public event UnityAction PlayerMoved {
        add { movement.Moved += value; }
        remove { movement.Moved -= value; }
    
    
    }

   public IMonoCall GetMeleeAttackEvent()
    {
        return melee.AttackAttempt;
    }
    [Serializable]
    public struct Parts
    {
        public CharacterController pController;
        public Transform groundCheck;
        public HumanoidParts hParts;
        public LayerMask jumpFloorMask;
        public Transform itemGameObject;
        public Camera mainCamera;
        public Transform hotBarTransform;
        public PlayerSC sC;
        public GameObject collapse;
    }
    MonoCalls calls = new MonoCalls();

  

    [SerializeField] Parts playerParts;
    public PlayerSC SC { get =>  playerParts.sC; }
    Movement movement;
    PlayerStatePointer<Grounded> jumpState;
    Grounded onGround;
    InAir falling;
    private PlayerStatePointer<NotMoving> movementState;
    private NotMoving notMoving;
    private Moving moving;
    public PlayerStatePointer<NotMoving> MovementState { get => movementState; }
    public NotMoving NotMoving { get => notMoving; }
    public Moving Moving { get => moving; }
    public Movement Movement { get => movement; }
    TimeController bTime;
    MouseLook look;
    private LookEnabled lookEnabled;
    private LookDisabled lookDisabled;
    private PlayerStatePointer<LookDisabled> lookState;
    public MouseLook Look { get => look; }
    public LookEnabled LookEnabled { get => lookEnabled; }
    public LookDisabled LookDisabled { get => lookDisabled; }
    public PlayerStatePointer<LookDisabled> LookState { get => lookState; }

    Inventory inventory;
    NotEquipped notEquipped;
    Equipped equipped;
    PlayerStatePointer<NotEquipped> hotBarState;
    EquippedGun equippedGun;
    Reloading reloading;
    public Reloading Reloading { get => reloading; }
    public Inventory Inventory { get => inventory;  }
    public NotEquipped NotEquipped { get => notEquipped; }
    public Equipped Equipped { get => equipped;  }
    public EquippedGun EquippedGun { get => equippedGun; }
    public PlayerStatePointer<NotEquipped> HotBarState { get => hotBarState;  }

    Health health;
    public UIInfoBoard uiInfo;
    HandPosManage handposition;

    #region keyEvents
    /*
    KeyInputEvents lShiftEvents;
    KeyInputEvents wEvents;
    KeyInputEvents dEvents;
    KeyInputEvents aEvents;
    KeyInputEvents sEvents;
    KeyInputEvents cEvents;
    KeyInputEvents xEvents;
    KeyInputEvents spaceEvents;
    KeyInputEvents qEvents;
    KeyInputEvents eEvents;
    KeyInputEvents vEvents;
    */
    KeyInputEvents keyInputEvents;
    #endregion

    List<KeyInputEvents> keyInputEventsList;

    List<PointerIN> allRunningStates;
    PointerIN[] allRunningStatesArray;

    

    #region Component Events
    //Event system for components in the future maybe???
    
    public MonoCalls.MonoAcessors MonoAcessors { get => calls.accessors; }
    public TagManager TagManager { get => tagManager; }
    public PlayerStatePointer<Grounded> JumpState { get => jumpState;  }
    public Grounded OnGround { get => onGround; }
    public InAir Falling { get => falling; }
  
    public PlayerStatePointer<TimeDisabled> TimeState { get => timeState; }
    public TimeDisabled TimeDisabled { get => timeDisabled; }
    public TimeNormal NormalTime { get => normalTime;  }
    public TimeSlow SlowTime { get => slowTime;  }
    public StatusEffect.StatusEffectManager Status { get => statusEffectManager; }
  

    PlayerStatePointer<TimeDisabled> timeState;
    TimeDisabled timeDisabled;
    TimeNormal normalTime;
    TimeSlow slowTime;
    #endregion
    protected StatusEffect.StatusEffectManager statusEffectManager;
    MeleeManager melee;

    PlayerStatePointer<NotAttacking> meleePointer;
    NotAttacking notAttackingState;
    Swing swinging;
    HeavySwing swingingHeavy;
    WindUp windUp;
    public PlayerStatePointer<NotAttacking> MeleePointer { get => meleePointer;  }
    public NotAttacking NotAttackingState { get => notAttackingState; }
    public Swing Swinging { get => swinging;  }
    public HeavySwing SwingingHeavy { get => swingingHeavy;  }
    public WindUp WindUp { get => windUp;}

    void Test() { Debug.Log("awoken"); }
    void Awake()
    {
        look = new MouseLook(calls.accessors, playerParts.hParts.Parts1.body, playerParts.mainCamera.transform);
        bTime = new TimeController(calls.accessors, playerParts.sC);
        health = new Health(playerParts.sC);
        handposition = new HandPosManage(calls.accessors, playerParts.hParts.Parts1.rHand, playerParts.hParts.Parts1.lHand);

        inventory = new Inventory(calls.accessors, playerParts.itemGameObject, playerParts.hotBarTransform, tagManager.Tag, handposition, playerParts.sC);
        movement = new Movement(calls.accessors, playerParts.pController, playerParts.groundCheck, playerParts.jumpFloorMask, playerParts.hParts.Parts1.body);

        keyInputEventsList = new List<KeyInputEvents>();
        allRunningStates = new List<PointerIN>();
        keyInputEvents = new KeyInputEvents(keyInputEventsList);
        InitializeKeyEvents();
        tagManager.AddTagsToHitBoxes(health, this);
        notMoving = new NotMoving(this, movement);
        moving = new Moving(this, movement,playerParts.sC);
        movementState = new PlayerStatePointer<NotMoving>(notMoving, allRunningStates, this);
        lookDisabled = new LookDisabled(this, look);
        lookEnabled = new LookEnabled(this, look);
        lookState = new PlayerStatePointer<LookDisabled>(lookEnabled, allRunningStates, this);
        notEquipped = new NotEquipped(this, inventory);
       equipped = new Equipped(this, inventory);
        equippedGun = new EquippedGun(this, inventory);
        reloading = new Reloading(this, inventory);
        hotBarState = new PlayerStatePointer<NotEquipped>(notEquipped, allRunningStates, this);
        onGround = new Grounded(this, movement);
        falling = new InAir(this, movement);
        jumpState = new PlayerStatePointer<Grounded>(falling, allRunningStates, this);
        timeDisabled = new TimeDisabled(this, bTime);
        normalTime = new TimeNormal(this, bTime);
        slowTime = new TimeSlow(this, bTime);
        timeState = new PlayerStatePointer<TimeDisabled>(normalTime, allRunningStates,this);
        statusEffectManager = new StatusEffect.StatusEffectManager(calls.accessors, health, this, tagManager.tag);
        playerParts.hParts.Parts1.rHandAnim.updateMode = AnimatorUpdateMode.UnscaledTime;
        playerParts.hParts.Parts1.lHandAnim.updateMode = AnimatorUpdateMode.UnscaledTime;

        melee = new MeleeManager(calls.accessors, playerParts.sC, playerParts.hParts.Parts1.lHandCol, playerParts.hParts.Parts1.rHandCol, playerParts.hParts.Parts1.lHandAnim, playerParts.hParts.Parts1.rHandAnim, tagManager.tag);
        notAttackingState = new NotAttacking(this, melee);
        swinging = new Swing(this, melee);
        windUp = new WindUp(this, melee);
        swingingHeavy = new HeavySwing(this, melee);
        meleePointer = new PlayerStatePointer<NotAttacking>(notAttackingState, allRunningStates, this);
        AwakeComponents();
        uiInfo = new UIInfoBoard(MonoAcessors,this);
        health.HealthBelowZero += Death;
       
    }

    public static event UnityAction PlayerDied;
   void Death()
    {
        Transform transform = playerParts.mainCamera.gameObject.transform;
        transform.parent = playerParts.collapse.transform;
        playerParts.collapse.SetActive(true);
        playerParts.collapse.transform.parent = null;

        PlayerDied?.Invoke();

        gameObject.SetActive(false);

    }
    public class UIInfoBoard : StateManagerComponent
    {

        PInputManager p;
        public UIInfoBoard(MonoCalls.MonoAcessors manager, PInputManager p) : base(manager)
        {
            this.p = p;
        }
        public event UnityAction<float> StaminaChanged
        {

            add { p.moving.StaminaChanged += value; }
            remove { p.moving.StaminaChanged -= value; }
        }
        public event UnityAction<float> HealthChanged {
            
        add { p.health.HealthChanged += value; }
    remove { p.health.HealthChanged -= value; }
        }
        public event UnityAction<float> BulletTimeChanged
        {
            add { p.bTime.ValueUpdated += value; }
            remove { p.bTime.ValueUpdated -= value; }
        }
        public event UnityAction<int, HotBarItemSC> EquippedSlot {
            add { p.inventory.PickedUpSlot += value; }
            remove { p.inventory.PickedUpSlot -= value; }
        }
        public event UnityAction<int> UnequippedSlot
        {
            add { p.inventory.DroppedSlot += value; }
            remove { p.inventory.DroppedSlot -= value; }
        }

        protected override void CleanUp()
        {
           
        }
    }



    void Start()
    {

        InitializeStatesArray();
        StartComponents();


    }
 
   

  
    #region Initialization, Update, and Event Methods
    void InitializeKeyEvents()
    {
        foreach (KeyInputEvents keyEvent in keyInputEventsList)
        {
            keyEvent.KeyDown += InputKeyDown;
            keyEvent.KeyPress += InputKeyPressed;
            keyEvent.KeyUp += InputKeyUp;
        }
    }
    void OnDestroy()
    {
        calls.destroyed.Call();
    }
    void InitializeStatesArray()
    {
        allRunningStatesArray = allRunningStates.ToArray();
    }
    void AwakeComponents()
    {
       
            calls.awakeCall.Call();
        
    }
    void StartComponents()
    {
            calls.startCall.Call();
    }
    void UpdateComponents()
    {
            calls.updateCall.Call();
    }
    void LateUpdateComponents()
    {
            calls.lateUpdateCall.Call();
    }
    void FixedUpdateComponents()
    {
            calls.fixedUpdateCall.Call();
    }
  
   
    void InputKeyDown(KeyCode keyCode)
    {
        for (int x = 0; x < allRunningStatesArray.Length; x++)
        {
            allRunningStatesArray[x].Inputs.HandleKeyDownInput( keyCode);
        }
    }
    void InputKeyUp(KeyCode keyCode)
    {
        for (int x = 0; x < allRunningStatesArray.Length; x++)
        {
            allRunningStatesArray[x].Inputs.HandleKeyUpInput( keyCode);
        }
    }
    void InputKeyPressed(KeyCode keyCode)
    {
        for (int x = 0; x < allRunningStatesArray.Length; x++)
        {
            allRunningStatesArray[x].Inputs.HandleKeyPressedInput( keyCode);
        }
    }

    #endregion
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            keyInputEvents.OnKeyDown(KeyCode.Mouse0);
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            keyInputEvents.OnKeyDown(KeyCode.Mouse1);
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            keyInputEvents.OnKeyUp(KeyCode.Mouse0);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            keyInputEvents.OnKeyDown(KeyCode.R);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            keyInputEvents.OnKeyDown(KeyCode.LeftShift);

        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            keyInputEvents.OnKeyUp(KeyCode.LeftShift);

        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            keyInputEvents.OnKeyDown(KeyCode.Alpha1);

        }
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            keyInputEvents.OnKeyUp(KeyCode.Alpha1);

        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            keyInputEvents.OnKeyDown(KeyCode.Alpha3);

        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            keyInputEvents.OnKeyUp(KeyCode.Alpha3);

        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            keyInputEvents.OnKeyDown(KeyCode.Alpha4);

        }
        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            keyInputEvents.OnKeyUp(KeyCode.Alpha4);

        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            keyInputEvents.OnKeyDown(KeyCode.C);
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            keyInputEvents.OnKeyUp(KeyCode.C);
        }
       

        if (Input.GetKeyDown(KeyCode.W))
        {
            keyInputEvents.OnKeyDown(KeyCode.W);
        }
        if (Input.GetKey(KeyCode.W))
        {
            keyInputEvents.OnKeyPressed(KeyCode.W);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            keyInputEvents.OnKeyUp(KeyCode.W);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            keyInputEvents.OnKeyDown(KeyCode.D);
        }
        if (Input.GetKey(KeyCode.D))
        {
            keyInputEvents.OnKeyPressed(KeyCode.D);
        }


        if (Input.GetKeyUp(KeyCode.D))
        {
            keyInputEvents.OnKeyUp(KeyCode.D);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            keyInputEvents.OnKeyDown(KeyCode.A);
        }
        if (Input.GetKey(KeyCode.A))
        {
            keyInputEvents.OnKeyPressed(KeyCode.A);
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            keyInputEvents.OnKeyUp(KeyCode.A);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            keyInputEvents.OnKeyDown(KeyCode.S);

        }
        if (Input.GetKey(KeyCode.S))
        {
            keyInputEvents.OnKeyPressed(KeyCode.S);
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            keyInputEvents.OnKeyUp(KeyCode.S);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            keyInputEvents.OnKeyDown(KeyCode.Space);

        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            keyInputEvents.OnKeyDown(KeyCode.X);
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            keyInputEvents.OnKeyUp(KeyCode.X);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            keyInputEvents.OnKeyUp(KeyCode.Space);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            keyInputEvents.OnKeyDown(KeyCode.Q);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            keyInputEvents.OnKeyPressed(KeyCode.Q);
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            keyInputEvents.OnKeyUp(KeyCode.Q);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            keyInputEvents.OnKeyDown(KeyCode.E);
        }
        if (Input.GetKey(KeyCode.E))
        {
            keyInputEvents.OnKeyPressed(KeyCode.E);
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            keyInputEvents.OnKeyUp(KeyCode.E);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            keyInputEvents.OnKeyDown(KeyCode.V);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            keyInputEvents.OnKeyDown(KeyCode.Alpha2);

        }
      
        if (Input.GetKeyDown(KeyCode.Tab))
        {

            keyInputEvents.OnKeyDown(KeyCode.Tab);
        }

        UpdateComponents();
    }
    void LateUpdate()
    {
        LateUpdateComponents();
    }
    void FixedUpdate()
    {
        FixedUpdateComponents();
    }

    Coroutine stunRout;
    public void Stun(double stunTime)
    {
        if(stunRout !=null)
        {
            StopCoroutine(stunRout);

        }
        if (gameObject.activeInHierarchy == true) {
            stunRout = StartCoroutine(StunProcedure(stunTime)); }
    }
   protected IEnumerator StunProcedure(double stunTime)
    {
        moving.Stun(true);
        yield return new WaitForSeconds((float)stunTime);
        moving.Stun(false);

    }
}
public class KeyInputEvents
{

    float pressedDuration = 0;
    public UnityAction<KeyCode> KeyDown;
    public UnityAction<KeyPressAndDuration> KeyPressDuration;
    public UnityAction<KeyCode> KeyPress;

    public UnityAction<KeyCode> KeyUp;
    public UnityAction CheckInputs;
    public KeyInputEvents(List<KeyInputEvents> listToAdd)
    {
        listToAdd.Add(this);
    }

    public void OnKeyDown(KeyCode keyCode)
    {
        if (KeyDown != null)
            KeyDown(keyCode);
    }
    public void OnKeyUp(KeyCode keyCode)
    {
        if (KeyUp != null)
        {
            KeyUp(keyCode);
        }
        if (KeyPressDuration != null)
            KeyPressDuration(new KeyPressAndDuration(keyCode, pressedDuration));

        if (CheckInputs != null)
        {
            CheckInputs();
        }
        pressedDuration = 0;

    }
    public void OnKeyPressed(KeyCode keyCode)
    {
        pressedDuration += Time.deltaTime;
        if (KeyPress != null)
        {
            KeyPress(keyCode);
        }
    }
}
public struct KeyPressAndDuration // key press duration data holder
{
    public KeyCode keyCode;
    public float timer;
    public KeyPressAndDuration(KeyCode keyCode, float timer)
    {
        this.keyCode = keyCode;
        this.timer = timer;
    }
}
public abstract class StatusEffect
{

    public readonly float lengthMS;
    public readonly int ticks;

    protected StatusEffect(int ticks = 0, float lengthMS = 0)
    {
        this.ticks = ticks;
        this.lengthMS = lengthMS;
    }
    //     protected abstract void EffectFinish(StatusEffectManager manager); 
    protected abstract void ApplyEffect(StatusEffectManager manager);


   
    public class StatusEffectManager : StateManagerComponent
    {
        static int HealthHeal = 100;
        public class StunType
        : Enumeration
        {
            public readonly double stunTimeSec;

            public StunType(int id, string name, double stunTimeSec) : base(id, name)
            {
                this.stunTimeSec = stunTimeSec;
            }
        }

        Health health;
        IStunnable stun;
        private readonly BulletTag myTag;

        public interface IStatusEeffectable
        {
            public StatusEffectManager Status { get; }
        }
        public interface IStunnable
        {
            public void Stun(double stunTime);
        }
        public StatusEffectManager(MonoCalls.MonoAcessors manager, Health health, IStunnable stun, BulletTag myTag) : base(manager)
        {
            statusEffects = new HashSet<StatusEffect>();
            this.manager = manager;
            this.health = health;
            this.stun = stun;
            this.myTag = myTag;
        }
        HashSet<StatusEffect> statusEffects; // statuseffectcs should be stackable but like not happening here so lazy implementation aw yeah penguins are cool i like monkeys software engineering is funky
        public void AddStatusEffect(StatusEffect effect, BulletTag tag)
        {
            if (effect != null )
            {
                if (tag == null || tag.Hit.Contains(myTag)) {
                    if (effect.lengthMS <= 0)
                    {
                        effect.ApplyEffect(this);
                    }
                }
              
            }

        }

        protected override void CleanUp()
        {
        }

        public class Melee : StatusEffect
        {
            int damage;
            public Melee(int damage) : base()
            {
                this.damage = damage;
            }

            protected override void ApplyEffect(StatusEffectManager manager)
            {
                manager.health.Remove(damage);
            }
        }

        public class StunApply : StatusEffect
        {
            private readonly double stunTime;

            public StunApply(double  stunTime) : base()
            {
                this.stunTime = stunTime;
              //  Debug.Log("stunned applied");
            }

            protected override void ApplyEffect(StatusEffectManager manager)
            {
                manager.stun.Stun(stunTime);
            }
        }
        public class HealthApply : StatusEffect
        {


            protected override void ApplyEffect(StatusEffectManager manager)
            {
                manager.health.AddHealth(HealthHeal);
                Debug.Log("healed AW MAN");
            }
        }
    }
}
//https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/enumeration-classes-over-enum-types
public abstract class Enumeration : IComparable
{
    public string Name { get; private set; }
    public int Id { get; private set; }
    protected Enumeration(int id, string name) => (Id, Name) = (id, name);
    public override string ToString() => Name;
    public static IEnumerable<T> GetAll<T>() where T : Enumeration =>
        typeof(T).GetFields(BindingFlags.Public |
                            BindingFlags.Static |
                            BindingFlags.DeclaredOnly)
                    .Select(f => f.GetValue(null))
                    .Cast<T>();

    public override bool Equals(object obj)
    {
        if (obj is not Enumeration otherValue)
        {
            return false;
        }

        var typeMatches = GetType().Equals(obj.GetType());
        var valueMatches = Id.Equals(otherValue.Id);

        return typeMatches && valueMatches;
    }

    public override int GetHashCode() => Id.GetHashCode();

    public static int AbsoluteDifference(Enumeration firstValue, Enumeration secondValue)
    {
        var absoluteDifference = Math.Abs(firstValue.Id - secondValue.Id);
        return absoluteDifference;
    }

    public static T FromValue<T>(int value) where T : Enumeration
    {
        var matchingItem = Parse<T, int>(value, "value", item => item.Id == value);
        return matchingItem;
    }

    public static T FromDisplayName<T>(string displayName) where T : Enumeration
    {
        var matchingItem = Parse<T, string>(displayName, "display name", item => item.Name == displayName);
        return matchingItem;
    }

    private static T Parse<T, K>(K value, string description, Func<T, bool> predicate) where T : Enumeration
    {
        var matchingItem = GetAll<T>().FirstOrDefault(predicate);

        if (matchingItem == null)
            throw new InvalidOperationException($"'{value}' is not a valid {description} in {typeof(T)}");

        return matchingItem;
    }

    public int CompareTo(object other) => Id.CompareTo(((Enumeration)other).Id);
}