using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public interface Pointer<T, out Y> where T : StateManager where Y : FiniteState<T>
{
    public State<T> State { get; set; }

}

public class StatePointer<T, Y> : Pointer<T, Y> where T : StateManager where Y : FiniteState<T>
{
    protected Y state;

    public StatePointer(Y state, List<Pointer<T, FiniteState<T>>> listOfPointers, T stateManager)
    {
        if (listOfPointers != null) { listOfPointers.Add(this); }
        this.state = state;
        state.EnterState(stateManager);


    }




    public State<T> State { get => state; set => state = (Y)value; }

    public void ModifyState(Y state)
    {
        this.state = state;
    }
}
public class PlayerStatePointer<Y> : StatePointer<PInputManager, Y> where Y : PlayerState
{

    public PlayerStatePointer(Y state, List<Pointer<PInputManager, FiniteState<PInputManager>>> listOfPointers, PInputManager stateManager) : base(state, listOfPointers, stateManager)
    {


    }



}

public abstract class PlayerState : FiniteState<PInputManager>
{
    protected PlayerState(PInputManager manager) : base(manager)
    {
    }
}
public interface State<in T> where T : StateManager
{
    public abstract void EnterState(T stateManager);

    public abstract void HandleKeyDownInput(T stateManager, KeyCode keyCode);

    public abstract void HandleKeyPressedInput(T stateManager, KeyCode keyCode);

    public abstract void HandleKeyUpInput(T stateManager, KeyCode keyCode);

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

    public abstract void HandleKeyDownInput(T stateManager, KeyCode keyCode);

    public abstract void HandleKeyPressedInput(T stateManager, KeyCode keyCode);

    public abstract void HandleKeyUpInput(T stateManager, KeyCode keyCode);

    public abstract void ExitState(T stateManager);


}
