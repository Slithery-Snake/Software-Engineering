using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
   //[SerializeField] List<HotBarItem>  hotBarItems;
   [SerializeField] List<Shooting> guns;
  //  Dictionary<int, HotBarItem> hotBarDict;
    Dictionary<int, Shooting> gunDict;

    BulletSpawn bulletSpawn;
    public static ItemManager CreateItemManager(ItemManager prefab ,BulletSpawn bulletSpawn)
    {
       ItemManager r = Instantiate(prefab);
        r.bulletSpawn = bulletSpawn;
        return r;

    }

    private void Awake()
    {
       // hotBarDict = new Dictionary<int, HotBarItem>();
        gunDict = new Dictionary<int, Shooting>();
        for(int i = 0; i < guns.Count; i ++)
        {
            Shooting item = guns[i];
            gunDict.Add(item.ItemID, item);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Shooting.CreateGun(guns[0], bulletSpawn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
