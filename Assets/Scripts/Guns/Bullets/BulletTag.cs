using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
[CreateAssetMenu(fileName = "BulletTagFile", menuName ="BulletTag")]

public class BulletTag : ScriptableObject
{
   [SerializeField] BulletTag[] hit;
    [SerializeField] BulletTag[] ignore;
    [SerializeField]  BulletTag[] collide;

    public IReadOnlyList<BulletTag> Ignore { get => ignore;  }
    public IReadOnlyList<BulletTag> Collide { get => collide; }
    public IReadOnlyList<BulletTag> Hit { get => hit; }
    Dictionary<BulletTag, UnityAction<IShootable, Bullet>> tagToAction;
    public void RequestTagAction(BulletTag tag, out UnityAction<IShootable, Bullet> act)
    {
     
         tagToAction.TryGetValue(tag, out act);
        

       
    }

   
    void OnEnable()
    {
        Debug.Log("BULLETTAG AWAKE");
        tagToAction = new Dictionary<BulletTag, UnityAction<IShootable, Bullet>>();
       
        
            FillDictionary(tagToAction, Collide, (IShootable d, Bullet b) => Bullet.Collided(b));
            FillDictionary(tagToAction, Ignore, (IShootable d, Bullet b) => Bullet.Ignore(b));
            FillDictionary(tagToAction, Hit, Bullet.Hit);

        
        void FillDictionary(Dictionary<BulletTag, UnityAction<IShootable, Bullet>> dict, IReadOnlyList<BulletTag> arr, UnityAction<IShootable, Bullet> action)
        {
            for (int i = 0; i < arr.Count
              ; i++)
            {
                dict.TryAdd(arr[i], action);
            }
        }
    }





}
