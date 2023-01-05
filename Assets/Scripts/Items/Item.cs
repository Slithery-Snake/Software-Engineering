using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Creatable: MonoBehaviour
{

    public abstract int ItemID { get; }
    [SerializeField] Collider myCollider;
    [SerializeField] Rigidbody myRigidbody;
    public void OnFloor()
    {
        myCollider.isTrigger = false;
        myRigidbody.constraints = RigidbodyConstraints.None;
    }
    public void Held()
    {
        myCollider.isTrigger = true;
        myRigidbody.constraints = RigidbodyConstraints.FreezePosition;
        myRigidbody.freezeRotation = true;

    }
}
public abstract class Item<T> : Creatable where T : ItemSC
{
    [SerializeField] protected T itemData;
    public T ItemData { get => itemData; }
   public override int ItemID { get => itemData.ItemID; }

}
