using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PInputManager : StateManager
{
 
  
    
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
     
    }
    [SerializeField] Parts playerParts;
    Movement movement;
    
    private PlayerStatePointer<NotMoving> movementState;
    private NotMoving notMoving;
    private Moving moving;
    public PlayerStatePointer<NotMoving> MovementState { get => movementState; }
    public NotMoving NotMoving { get => notMoving; }
    public Moving Moving { get => moving; }
    public Movement Movement { get => movement; }

    MouseLook look;
    private LookEnabled lookEnabled;
    private LookDisabled lookDisabled;
    private PlayerStatePointer<LookDisabled> lookState;
    public MouseLook Look { get => look; }
    public LookEnabled LookEnabled { get => lookEnabled; }
    public LookDisabled LookDisabled { get => lookDisabled; }
    public PlayerStatePointer<LookDisabled> LookState { get => lookState; }
  
    
    public Inventory Inventory { get => inventory;  }
    public NotEquipped NotEquipped { get => notEquipped; }
    public Equipped Equipped { get => equipped;  }
    public PlayerStatePointer<NotEquipped> HotBarState { get => hotBarState;  }

    Inventory inventory;
    NotEquipped notEquipped;
    Equipped equipped;
    PlayerStatePointer<NotEquipped> hotBarState;
    

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

    List<Pointer<PInputManager, FiniteState<PInputManager>>> allRunningStates;
    Pointer<PInputManager, FiniteState<PInputManager>>[] allRunningStatesArray;

    List<StateManagerComponent<PInputManager>> components;

    StateManagerComponent<PInputManager>[] componentsArray;

    #region Component Events
    //Event system for components in the future maybe???
    public void FireComponentEvent()
    {

    }
    public void SubscribeComponentEvent()
    {

    }
    public void UnsubcribeCompnentEvent()
    {

    }
    public void SetComponentEvent()
    {

    }

    public struct WhichEvent<T> where T : ComponentArgs
    {
        public UnityAction<T> Event;
        public T ComponentArgs { get; private set; }
        WhichEvent(T componentArgs)
        {
            ComponentArgs = componentArgs;
            Event = null;
        }
    }
    public class ComponentArgs : EventArgs
    {
        public StateManagerComponent<PInputManager> Source { get; private set; }

        ComponentArgs(StateManagerComponent<PInputManager> source)
        {
            Source = source;
        }
    }


    #endregion
    void Awake()
    {
        components = new List<StateManagerComponent<PInputManager>>();
        movement = new Movement(this, components, playerParts.pController, playerParts.groundCheck, playerParts.jumpFloorMask, playerParts.body);
        look = new MouseLook(this, components, playerParts.body, playerParts.mainCamera.transform);
        inventory = new Inventory(this, components, playerParts.itemGameObject, playerParts.hotBarTransform);
        InitializeComponentsArray();
        AwakeComponents();
       
        keyInputEventsList = new List<KeyInputEvents>();
        allRunningStates = new List<Pointer<PInputManager, FiniteState<PInputManager>>>();
        keyInputEvents = new KeyInputEvents(keyInputEventsList);
        InitializeKeyEvents();

        notMoving = new NotMoving(null, movement);
        moving = new Moving(notMoving, movement);
        movementState = new PlayerStatePointer<NotMoving>(notMoving, allRunningStates, this);
        lookDisabled = new LookDisabled(null, look);
        lookEnabled = new LookEnabled(lookDisabled, look);
        lookState = new PlayerStatePointer<LookDisabled>(lookEnabled, allRunningStates, this);
        notEquipped = new NotEquipped(null, inventory);
        hotBarState = new PlayerStatePointer<NotEquipped>(notEquipped, allRunningStates, this);

    }


    void Start()
    {

        InitializeStatesArray();
        StartComponents();


    }
    public virtual void ChangeToState(PlayerState newState, Pointer<PInputManager, PlayerState> stateToChange)
    {
        stateToChange.State.ExitState(this);
        stateToChange.State = newState;
        stateToChange.State.EnterState(this);
        Debug.Log(newState);


    }
    public virtual void ChangeToState(Equipped newState, Pointer<PInputManager, NotEquipped> stateToChange, int slot)
    {
        stateToChange.State.ExitState(this);
        newState = new Equipped(notEquipped, inventory, slot);
        stateToChange.State = newState;
        stateToChange.State.EnterState(this);
        Debug.Log(newState);


    }
    public virtual void ChangeToNewState(PlayerState newState, Pointer<PInputManager, PlayerState> stateToChange)
    {
        if (!(newState == stateToChange.State))
        {
            stateToChange.State.ExitState(this);
            stateToChange.State = newState;
            stateToChange.State.EnterState(this);
            Debug.Log(newState);
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
        for (int x = 0; x < componentsArray.Length; x++)
        {
            componentsArray[x].Awake();
        }
    }
    void StartComponents()
    {
        for (int x = 0; x < componentsArray.Length; x++)
        {
            componentsArray[x].Start();
        }
    }
    void UpdateComponents()
    {
        for (int x = 0; x < componentsArray.Length; x++)
        {
            componentsArray[x].Update();
        }
    }
    void LateUpdateComponents()
    {
        for (int x = 0; x < componentsArray.Length; x++)
        {
            componentsArray[x].LateUpdate();
        }
    }
    void FixedUpdateComponents()
    {
        for (int x = 0; x < componentsArray.Length; x++)
        {
            componentsArray[x].FixedUpdate();
        }
    }
    void InitializeComponentsArray()
    {

        componentsArray = components.ToArray();
    }
    void UpdateStates()
    {

        for (int x = 0; x < allRunningStatesArray.Length; x++)
        {
            allRunningStatesArray[x].State.Update(this);
        }

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


        UpdateStates();
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