using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
public abstract class Poolable<T>: MonoBehaviour
{
    
    protected UnityAction Recycled;
    public void InitiliazeEvet(UnityAction NotifyEl)
    {
        Recycled += NotifyEl;

    }
      protected abstract void RecycleProcess(T t);
}

public class PoolElement<T> where T : Poolable<T>
{
    PoolElement<T> nextAvailable;
    T actual;
    UnityAction<PoolElement<T>> notifyPool;

    public PoolElement<T> NextAvailable { get => nextAvailable;  }
    public T Actual { get => actual;  }
    public UnityAction<PoolElement<T>> NotifyPool { get => notifyPool; }

    public  PoolElement ( T actual, UnityAction<PoolElement<T>> ele) 
    {
        actual.InitiliazeEvet(Recycled);
        this.actual = actual;
        notifyPool += ele;
    }
    void Recycled()
    {
        notifyPool(this);
    }
    public void SetNextBullet(PoolElement<T> t)
    {
        nextAvailable = t;

    }
    public void SetNextNull()
    {
        nextAvailable = default(PoolElement<T>);
    }
}
public class BulletSpawn : MonoBehaviour
{
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] List<BulletSC> bulletTypes;
    Dictionary<BulletSC, BulletPool> idToBulletPool;
    GameManager gameManager;
  
    public void Initialize(GameManager manager)
    {
        gameManager = manager;

    }
    private void Start()
    {
        idToBulletPool = new Dictionary<BulletSC, BulletPool>();
    }
    BulletPool MakePool(BulletSC bulletIdentity)
    {
        GameObject tempTransform = new GameObject();
        tempTransform.transform.SetParent(transform);
        BulletPool pool = new BulletPool(tempTransform.transform, bulletPrefab, bulletIdentity);
        idToBulletPool.Add(bulletIdentity, pool);
        return pool;
    }
    public BulletPool RequestPool(BulletSC bulletIdentity)
    {
        BulletPool result;
        idToBulletPool.TryGetValue(bulletIdentity, out result);
        if (result == null)
        {
            result = MakePool(bulletIdentity);
        }

        return result;
    }
    public class BulletPool
    {
         Bullet bulletPrefab;
        List<PoolElement<Bullet>> bullets;
        BulletSC bulletIdentity;
        Transform worldObject;
        PoolElement<Bullet> firstOnList;
        public BulletPool(Transform worldObject, Bullet prefab, BulletSC bulletIdentity)
        {
            this.worldObject = worldObject;
            this.bulletPrefab = prefab;
            this.bulletIdentity = bulletIdentity;

            MakePool();
            firstOnList = bullets[0];

           SetNext();
        }
       PoolElement<Bullet>  CreateListElement()
        {
            Bullet bullet = Bullet.CreateBullet(bulletIdentity, this, bulletPrefab);
            PoolElement<Bullet> bulletElement = new PoolElement<Bullet>(bullet, AddToFront);
            return bulletElement;
        }
        PoolElement<Bullet> CreateABulletToList()
        {
            PoolElement<Bullet> ele = CreateListElement();
            Bullet temp = ele.Actual;

            temp.transform.SetParent(worldObject, true);
            temp.Deactiveate();
            bullets.Add(ele);
            return ele;
        }
        void AddToFront(PoolElement<Bullet> element)
        {
            element.Actual.Deactiveate();
            element.SetNextBullet(firstOnList);
            firstOnList = element;

        }
        public Bullet RequestBullet()
        {
            try
            {
                firstOnList = firstOnList.NextAvailable;
                return firstOnList.Actual;
            }
            catch (NullReferenceException)
            {
                PoolElement<Bullet> ele = CreateABulletToList();

                return ele.Actual;
                


            }

        }
        void SetNext()
        {
            for (int i = 0; i < bullets.Count - 1; i++)
            {
                bullets[i].SetNextBullet(bullets[i + 1]);

            }
            bullets[bullets.Count - 1].SetNextNull();
        }
        void MakePool()
        {
            bullets = new List<PoolElement<Bullet>>();
            for (int i = 0; i <= 10; i++)
            {
                PoolElement<Bullet> ele = CreateABulletToList();

            }


        }


    }
 
}
