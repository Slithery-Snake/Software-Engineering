using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonBaseClass<T> : MonoBehaviour where T : SingletonBaseClass<T>
{   private static T instance;
    public static T Instance
    {
        get { return instance; }
    }
    public static bool IsInitialized
    {
        get { return instance != null; }
    }
    protected virtual void Awake()
    {
         if(instance!= null)
         {
            Debug.Log("double instantiation of singleton");
         }
         else
         { 
            instance = (T)this;
        
         }
    }
    protected virtual void OnDestroy()
    {
        if(instance == this)
        {
            instance = null;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
