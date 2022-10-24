using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : Poolable<Bullet>
{

    [SerializeField]BulletSC sC;
    BulletSpawn.BulletPool pool;
    [SerializeField]Rigidbody rg;

    public BulletSC SC { get => sC;}
    public Rigidbody Rg { get => rg; set => rg = value; }
    
    public void Shoot(Vector3 worldPos, Vector3 direction)
    {
        Activate();
        transform.position = worldPos;
        rg.AddForce(direction.normalized * SC.forceMagnitude, ForceMode.Impulse);

    }
    public static Bullet CreateBullet(BulletSC sC, BulletSpawn.BulletPool pool, Bullet prefab)
    {
        Bullet bul = Instantiate(prefab);
        bul.InitializeItem(sC, pool);
        return bul;
    }
   
    void InitializeItem(BulletSC sC, BulletSpawn.BulletPool pool)
    {
        this.sC = sC;
        this.pool = pool;
    }
    
    private void OnTriggerEnter(Collider other) 
    {
       
        IShootable shot = other.GetComponent<IShootable>();
        if (shot != null)
        {
            if (shot.Shoot == Shootable.shotAt)
            {
                shot.ShotAt(this);
            } else if(shot.Shoot == Shootable.ignore)
            {
                return;
            }
        }
        RecycleProcess(this);
    }
    public void Activate()
    {
        gameObject.SetActive(true);
    }
    public void Deactiveate()
    {
        gameObject.SetActive(false);
    }
    protected override void RecycleProcess(Bullet t)
    {
        Recycled();
        Deactiveate();
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
