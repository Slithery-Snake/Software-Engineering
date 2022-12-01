using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Central : SingletonBaseClass<Central>
{
  public  enum Messages { EnemyKilled, }
    Queue queue;
    public static void Create()
    {
        GameObject obj = new GameObject();
        obj.AddComponent<Central>();
    }

}