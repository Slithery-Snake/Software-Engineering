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
public abstract class StateManagerComponent<T>: ComponentOfManager<T> where T: StateManager
{
    protected T manager;
    public StateManagerComponent(T manager, List<StateManagerComponent<T>> list)
    {
        this.manager = manager;
        list.Add(this);
    }
    protected void StopCoroutine(Coroutine routine) 
    {
        manager.StopCoroutine(routine);
    
    }
   
    protected void StopCoroutine(IEnumerator routine)
    {
        manager.StopCoroutine(routine);

    }

    protected Coroutine StartCoroutine(IEnumerator routine)
    {
      Coroutine routineReturn =  manager.StartCoroutine(routine);
        return routineReturn;
    }
    public abstract void Update();
    public abstract void Awake();
    public abstract void Start();

    public abstract void LateUpdate();

    public abstract void FixedUpdate();

    public abstract void OnEnabled();

    public abstract void OnDisabled();

}