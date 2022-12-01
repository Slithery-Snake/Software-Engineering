using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



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
    public  void AddTagsToHitBoxes(IShootable shootable)
    {
       for(int i = 0; i < hitBoxes.Length; i ++)
        {
            ShootBox.Create(tag, shootable, hitBoxes[i]);
        }
    }
     public BulletTag Tag { get => tag; }
}
public class PInputManager : StateManagerIN
{
 
    [SerializeField]TagManager tagManager;

  



    [Serializable]
    public struct Parts
    {
        public CharacterController pController;
        public Transform groundCheck;
        public Transform body;
        public LayerMask jumpFloorMask;
        public Transform itemGameObject;
        public Camera mainCamera;
        public Transform hotBarTransform;
        public PlayerSC sC;

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

    List<PointerIN<PInputManager, FiniteStateInput<PInputManager>>> allRunningStates;
    PointerIN<PInputManager, FiniteStateInput<PInputManager>>[] allRunningStatesArray;

 

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

    PlayerStatePointer<TimeDisabled> timeState;
    TimeDisabled timeDisabled;
    TimeNormal normalTime;
    TimeSlow slowTime;
    #endregion
    void Test() { Debug.Log("awoken"); }
    void Awake()
    {
        movement = new Movement(calls.accessors,  playerParts.pController, playerParts.groundCheck, playerParts.jumpFloorMask, playerParts.body);
        look = new MouseLook(calls.accessors, playerParts.body, playerParts.mainCamera.transform);
        bTime = new TimeController(calls.accessors, playerParts.sC);
        inventory = new Inventory(calls.accessors, playerParts.itemGameObject, playerParts.hotBarTransform, tagManager.Tag);
        health = new Health(playerParts.sC);
        keyInputEventsList = new List<KeyInputEvents>();
        allRunningStates = new List<PointerIN<PInputManager, FiniteStateInput<PInputManager>>>();
        keyInputEvents = new KeyInputEvents(keyInputEventsList);
        InitializeKeyEvents();
        tagManager.AddTagsToHitBoxes(health);
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
        AwakeComponents();
        uiInfo = new UIInfoBoard(this);
    }
    public class UIInfoBoard
    {
        public  UnityAction<float> StaminaChanged;
        public UnityAction<float> HealthChanged;
        public UnityAction<float> BulletTimeChanged;

        public UIInfoBoard(PInputManager p)
        {

            p.moving.StaminaChanged += (object a, float f) => { StaminaChanged(f); };
            p.health.HealthChanged += (object a, float f) => { HealthChanged(f); };
            p.bTime.ValueUpdated += (object a, float f) => { BulletTimeChanged(f); };

        }

    }



    void Start()
    {

        InitializeStatesArray();
        StartComponents();


    }
 
    public virtual void ChangeToState(PlayerState newState, PointerIN<PInputManager, PlayerState> stateToChange)
    {
        Debug.Log(newState);

        stateToChange.State.ExitState(this);
        stateToChange.State = newState;
        stateToChange.State.EnterState(this);


    }


    public virtual void ChangeToNewState(PlayerState newState, PointerIN<PInputManager, PlayerState> stateToChange)
    {
        Debug.Log(newState);

        if (!(newState == stateToChange.State))
        {
            stateToChange.State.ExitState(this);
            stateToChange.State = newState;
            stateToChange.State.EnterState(this);
        }

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
            allRunningStatesArray[x].State.HandleKeyDownInput(this, keyCode);
        }
    }
    void InputKeyUp(KeyCode keyCode)
    {
        for (int x = 0; x < allRunningStatesArray.Length; x++)
        {
            allRunningStatesArray[x].State.HandleKeyUpInput(this, keyCode);
        }
    }
    void InputKeyPressed(KeyCode keyCode)
    {
        for (int x = 0; x < allRunningStatesArray.Length; x++)
        {
            allRunningStatesArray[x].State.HandleKeyPressedInput(this, keyCode);
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
        if (Input.GetKeyDown(KeyCode.C))
        {
            keyInputEvents.OnKeyDown(KeyCode.C);
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            keyInputEvents.OnKeyUp(KeyCode.C);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            keyInputEvents.OnKeyDown(KeyCode.X);
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            keyInputEvents.OnKeyUp(KeyCode.X);
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
        if (Input.GetKey(KeyCode.Space))
        {
            keyInputEvents.OnKeyPressed(KeyCode.Space);
            ;
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