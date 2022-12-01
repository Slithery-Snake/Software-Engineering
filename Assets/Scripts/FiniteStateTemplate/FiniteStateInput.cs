using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface Pointer<T, out Y> where T : StateManager where Y : FiniteState<T>
{
    public FiniteState<T> State { get; set; }

}

public interface PointerIN<T, out Y> where T : StateManagerIN where Y : FiniteStateInput<T>
{
    public FiniteStateInput<T> State { get; set; }
}
public class StatePointer<T, Y> : Pointer<T, Y> where T : StateManager where Y : FiniteState<T>
{
    protected Y state;

    public StatePointer(Y state, T stateManager)
    {
        this.state = state;
        state.EnterState(stateManager);


    }

    public FiniteState<T> State { get => state; set => state = (Y)value; }





    //  public Y State { get => state; set => state = (value; }

    public void ModifyState(Y state)
    {
        this.state = state;
    }
}
public class StatePointerIN<T, Y> : PointerIN<T, Y> where T : StateManagerIN where Y : FiniteStateInput<T>
{
    protected Y state;

    public StatePointerIN(Y state, List<PointerIN<T, FiniteStateInput<T>>> listOfPointers, T stateManager)
    {
        if (listOfPointers != null) { listOfPointers.Add(this); }
        this.state = state;
        state.EnterState(stateManager);


    }

    public FiniteStateInput<T> State { get => state; set => state = (Y)value; }





    //  public Y State { get => state; set => state = (value; }

    public void ModifyState(Y state)
    {
        this.state = state;
    }
}
public class PlayerStatePointer<Y> : StatePointerIN<PInputManager, Y> where Y : PlayerState
{

    public PlayerStatePointer(Y state, List<PointerIN<PInputManager, FiniteStateInput<PInputManager>>> listOfPointers, PInputManager stateManager) : base(state, listOfPointers, stateManager)
    {


    }



}

public abstract class PlayerState : FiniteStateInput<PInputManager>
{
    protected PlayerState(PInputManager manager) : base(manager)
    {
    }
}
public interface State<in T> where T : StateManager
{
    public abstract void EnterState(T stateManager);

  

    public abstract void ExitState(T stateManager);



}
public abstract class FiniteState<T> : State<T> where T : StateManager
{
    protected T manager;
    public FiniteState(T manager)
    {
        this.manager = manager;
    }

    public abstract void EnterState(T stateManager);


    public abstract void ExitState(T stateManager);


}

public abstract class FiniteStateInput<T> where T : StateManagerIN
{
    protected T manager;

    public FiniteStateInput(T manager) {
        this.manager = manager;

    }
  
    public abstract  void EnterState(T stateManager);

    public abstract void HandleKeyDownInput(T stateManager, KeyCode keyCode);

    public abstract void HandleKeyPressedInput(T stateManager, KeyCode keyCode);

    public abstract void HandleKeyUpInput(T stateManager, KeyCode keyCode);

    public abstract  void ExitState(T stateManager);


}
