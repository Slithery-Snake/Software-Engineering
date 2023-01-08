using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunData : MonoBehaviour
{
    // gun properties
    public string gunName;
    public int damage;
    public float fireRate;
    public float range;
    public float accuracy;
    public int ammoCapacity;
    public int currentAmmo;
    public bool automatic;
    public GameObject bulletPrefab;

    Vector3int gunVector = new Vector3int(0, 0, 0);
    gunName = 0;
    Vector3 Position = GameObject.Find("Gun").transform.position;
    //Send spawn data to central class
}
