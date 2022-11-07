using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager: MonoBehaviour
{
    
  
    public virtual void ChangeToState(FiniteState<StateManager> newState, FiniteState<StateManager> stateToChange)
    {
        stateToChange.ExitState(this);
        stateToChange = newState;
        stateToChange.EnterState(this);

    }
   
}

interface ComponentOfManager<out T> where T :StateManager
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