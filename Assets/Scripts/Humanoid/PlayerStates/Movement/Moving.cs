using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Moving : NotMoving
{
    protected PlayerSC sc;
    float modifier;
    float stamina;
    Submoving submoving;
    public event EventHandler<float> StaminaChanged;

    public Moving(PInputManager parent, Movement movement, PlayerSC sc) : base(parent, movement)
    {
        this.movement = movement;
        this.sc = sc;
        modifier = sc.WalkSpeed;
        stamina = sc.StaminaBarMax;
        submoving = new Submoving(this);

    }

  
    Coroutine routine;
    bool isRunning;

    void StopRoutine()
    {
        if (routine != null) { manager.StopCoroutine(routine); }
    }
    void StartRegen()
    {
        StopRoutine();
        routine = manager.StartCoroutine(Regen());
    }
    void StartDecay()
    {
        StopRoutine();

        routine = manager.StartCoroutine(Decay());
    }
    IEnumerator Decay()
    {
        WaitForSeconds wait = new WaitForSeconds(sc.StaminaBarTick);
        float inc = sc.StaminaBarDecrement;
        float end = 0;
        do
        {
            stamina -= inc;
            StaminaChanged.Invoke(this, stamina);

            if(stamina <= end )
            {

                stamina = end;
                break;
            }
            yield return wait;
        }
        while (true);


    }
    IEnumerator Regen()
    { WaitForSeconds wait = new WaitForSeconds(sc.StaminaBarTick);
        float inc = sc.StaminaIncrement;
        float end = sc.StaminaBarMax;
      do  {
            stamina += inc;
            StaminaChanged.Invoke(this, stamina);

            if(stamina >= end)
            {
                stamina = end;
                break;
            }
            yield return wait;

        }
        while (true) ;

        
    }
    void Sprint() { }

    public override void HandleKeyDownInput(PInputManager stateManager, KeyCode keyCode)
    {
        if (keyCode == KeyCode.LeftShift)
        {
            submoving.ChangeToState(submoving.Running, submoving.State);
        }
    }
   
    public override void HandleKeyUpInput(PInputManager stateManager, KeyCode keyCode)
    {
        if (keyCode == KeyCode.LeftShift)
        {
            submoving.ChangeToState(submoving.Walking, submoving.State);

        }
    }
    protected override void Update()
    {
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");
        
        movement.MovingFunction(moveX * modifier, moveZ*modifier);
        if (moveX == 0 && moveZ == 0)
        {
            manager.ChangeToState(manager.NotMoving, manager.MovementState);
        }
    }


    public class Submoving : StateManager
    {
       
        StatePointer<Submoving, Walking> state;
        Running running;
        Walking walking;
        Moving parent;
        public virtual void ChangeToState(FiniteState<Submoving> newState, StatePointer<Submoving, Walking> stateToChange)
        {
            Debug.Log(newState);

            stateToChange.State.ExitState(this);
            stateToChange.State = newState;
            stateToChange.State.EnterState(this);


        }
        public Submoving(Moving parent)
        {
            this.parent = parent;

            running = new Running(this);
            walking = new Walking(this);
            state = new StatePointer<Submoving, Walking>(walking, this);
        }

        public StatePointer<Submoving, Walking> State { get => state;  }
        public Running Running { get => running;  }
        public Walking Walking { get => walking; }
        public Moving Moving { get => parent; }
    }
    public class Running : Walking
    {
        public Running(Submoving manager) : base(manager)
        {
            
        }
        public override void EnterState(Submoving stateManager)
        {
            Moving m = manager.Moving;
            float current = m.stamina;
            if (current > 0)
            {
                m.StartDecay();
                m.modifier = m.sc.RunSpeed;
            } else
            {
                manager.ChangeToState(manager.Walking, manager.State);
            }
           

        }
    }
    public class Walking : FiniteState<Submoving>
    {
        public Walking(Submoving manager) : base(manager)
        {
      
        }

        public override void EnterState(Submoving stateManager)
        {
            Moving m = manager.Moving;
            float current = m.stamina;
            if(current < m.sc.StaminaBarMax)
            {
                m.StartRegen();
            }
            m.modifier = m.sc.WalkSpeed;
           
        }

        public override void ExitState(Submoving stateManager)
        {
            manager.Moving.StopRoutine();
        }

      
        
    }


}
