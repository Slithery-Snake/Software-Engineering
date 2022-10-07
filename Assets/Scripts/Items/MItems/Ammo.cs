using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : Item<AmmoSC>, Iinteractable
{
    [SerializeField] int count = 0;

    public int Count { get => count; set => count = value; }

    public void Interacted(PInputManager source)
    {
        source.Inventory.AddAmmo(this);
        Destroy(gameObject);
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
