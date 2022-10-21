using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    //[SerializeField] List<HotBarItem>  hotBarItems;
    [SerializeField] List<CollectiveGun> guns;
    //  Dictionary<int, HotBarItem> hotBarDict;
    Dictionary<int, CollectiveGun> gunDict;
    [SerializeField] List<Ammo> ammo;


    BulletSpawn bulletSpawn;
    public static ItemManager CreateItemManager(ItemManager prefab, BulletSpawn bulletSpawn)
    {
        ItemManager r = Instantiate(prefab);
       
        r.bulletSpawn = bulletSpawn;
        return r;
    }

    private void Awake()
    {
        // hotBarDict = new Dictionary<int, HotBarItem>();
        gunDict = new Dictionary<int, CollectiveGun>();
        for (int i = 0; i < guns.Count; i++)
        {
            CollectiveGun item = guns[i];
            gunDict.Add(item.WeaponData.ItemID, item);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        CollectiveGun.CreateGun(guns[0], bulletSpawn, new Vector3(3, 3, 0), Quaternion.identity);
        Ammo.CreateAmmo(ammo[0], 100, new Vector3(2, 3, 0), Quaternion.identity);
    }

    // Update is called once per frame
}
