using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManagerIN: MonoBehaviour
{
    
  
    public virtual void ChangeToState(FiniteStateInput<StateManagerIN> newState, FiniteStateInput<StateManagerIN> stateToChange)
    {
        stateToChange.ExitState(this);
        stateToChange = newState;
        stateToChange.EnterState(this);

    }
   
}
public interface IStateManager<T>
{

    public void ChangeToState(T newState, T stateToChange);
 
}
public class StateManager
{


    public virtual void ChangeToState(FiniteState<StateManager> newState, FiniteState<StateManager> stateToChange)
    {
        stateToChange.ExitState(this);
        stateToChange = newState;
        stateToChange.EnterState(this);

    }

}

interface ComponentOfManager<out T> where T :StateManagerIN
{

}
public abstract class StateManagerComponent//: ComponentOfManager<T> where T: StateManager
{
    protected MonoCalls.MonoAcessors manager;
    public StateManagerComponent(MonoCalls.MonoAcessors manager)
    {
        this.manager = manager;
    }
 




}