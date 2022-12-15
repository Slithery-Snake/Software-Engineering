using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public interface IStateInputs
{
    public  void HandleKeyDownInput(KeyCode keyCode);

    public  void HandleKeyPressedInput(KeyCode keyCode);

    public  void HandleKeyUpInput(KeyCode keyCode);
}

public interface Pointer
{
    public State State { get; set; }

}
public interface PointerIN: Pointer
{
   
    IStateInputs Inputs { get; }
}

public class StatePointer<T, Y> : Pointer where T : StateManager where Y : FiniteState<T>
{
    protected Y state;

    public StatePointer(Y state, T stateManager)
    {
        this.state = state;
        state.EnterState();


    }

    public State State { get => state; set => state = (Y)value; }





    //  public Y State { get => state; set => state = (value; }

    public void ModifyState(Y state)
    {
        this.state = state;
    }
}
public class StatePointerIN<T, Y> :  PointerIN where T : StateManagerIN where Y : FiniteStateInput<T>
{
    protected Y state;

    public StatePointerIN(Y state, List<PointerIN> listOfPointers)
    {
        if (listOfPointers != null) { listOfPointers.Add(this); }
        this.state = state;
        state.EnterState();


    }

    public State State { get => state; set => state = (Y)value; }
    public IStateInputs Inputs { get => state;  }





    //  public Y State { get => state; set => state = (value; }

    public void ModifyState(Y state)
    {
        this.state = state;
    }
}
public class PlayerStatePointer<Y> : StatePointerIN<PInputManager, Y> where Y : PlayerState
{

    public PlayerStatePointer(Y state, List<PointerIN> listOfPointers, PInputManager stateManager) : base(state, listOfPointers)
    {


    }



}

public abstract class PlayerState : FiniteStateInput<PInputManager>
{
    protected PlayerState(PInputManager manager) : base(manager)
    {
    }
}
public interface State
{
    public abstract void EnterState();

  

    public abstract void ExitState();



}
public interface StateInput : State, IStateInputs
{
  
}
public abstract class FiniteState<T> : State where T : StateManager
{
    protected T manager;
    public FiniteState(T manager)
    {
        this.manager = manager;
    }

    public abstract void EnterState();


    public abstract void ExitState();


}

public abstract class FiniteStateInput<T> : StateInput where T : StateManagerIN
{
    protected T manager;

    public FiniteStateInput(T manager) {
        this.manager = manager;

    }
  
    public abstract  void EnterState();

    public abstract void HandleKeyDownInput( KeyCode keyCode);

    public abstract void HandleKeyPressedInput( KeyCode keyCode);

    public abstract void HandleKeyUpInput(KeyCode keyCode);

    public abstract  void ExitState();


}
