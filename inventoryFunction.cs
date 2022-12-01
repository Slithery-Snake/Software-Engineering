using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inventoryFunction : MonoBehaviour
{ 
    string[5] store = new string [];
    bool equipped;
    int weaponNumber;
    string weaponName;
    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            weaponNumber = 1;
            equipped = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weaponNumber = 2;
            equipped = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            weaponNumber = 3;
            equipped = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            weaponNumber = 4;
            equipped = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            weaponNumber = 5;
            equipped = true;
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            Unequip(weaponNumber);
        }
        if(Input.GetKeyDown(KeyCode.E)
        {
            Equip(weaponNumber);
        }
    }

    void Unequip(int wN)
    {
        if(equipped)
        {
            equipped = false;
            store[wN] = "";
        }
    }

    void Equip(int wN)
    {
        if(!equipped)
        {
            equipped = true;
            store[wN] = weaponName;
        }
    }
}
