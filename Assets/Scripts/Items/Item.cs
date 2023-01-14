using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Creatable: MonoBehaviour
{

    public abstract int ItemID { get; }
    [SerializeField] Collider myCollider;
    [SerializeField] Rigidbody myRigidbody;
    public void OnFloor()
    {
        myRigidbody.isKinematic = false;

        myCollider.isTrigger = false;

        myRigidbody.constraints = RigidbodyConstraints.None;
        myRigidbody.velocity = Vector3.zero;
    }
    public void ApplyForce(Vector3 v)
    {
        myRigidbody.AddForce(v, ForceMode.Acceleration);
    }
    public void Held()
    {
        myRigidbody.isKinematic = true;
        myCollider.isTrigger = true;
        myRigidbody.constraints = RigidbodyConstraints.FreezePosition;
        myRigidbody.freezeRotation = true;

    }
}
public abstract class Item<T> : Creatable where T : ItemSC
{
    [SerializeField] protected T itemData;
    public T ItemData { get => itemData; }
    public event UnityAction Destroyed;
   public override int ItemID { get => itemData.ItemID; }
    protected void OnDestroy()
    {
        Destroyed?.Invoke(); 
    }

}
