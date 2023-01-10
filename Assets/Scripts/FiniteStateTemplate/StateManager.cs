using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StateManagerIN: MonoBehaviour
{
    
  
    public virtual void ChangeToState(State newState, Pointer stateToChange)
    {
        stateToChange.State.ExitState();
        stateToChange.State = newState;
        stateToChange.State.EnterState();

    }
   
}
public interface IStateManager
{

    public void ChangeToState(State newState, Pointer stateToChange);
 
}
public class StateManager : IStateManager
{


    public virtual void ChangeToState(State newState, Pointer stateToChange)
    {
        stateToChange.State.ExitState();
        stateToChange.State = newState;
        stateToChange.State.EnterState();

    }

}

interface ComponentOfManager<out T> where T :StateManagerIN
{

}
public abstract class StateManagerComponent: IDisposable //: ComponentOfManager<T> where T: StateManager,
{
    protected MonoCalls.MonoAcessors manager;
    public StateManagerComponent(MonoCalls.MonoAcessors manager)
    {
        this.manager = manager;
        manager.Destroyed.Listen(Dispose);
    }
    protected abstract void CleanUp();
    public void Dispose()
    {
        manager.Destroyed.Deafen(Dispose);

        CleanUp();
    }
}