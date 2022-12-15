using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "BulletData", menuName = "BulletItem")]
public class BulletSC : ScriptableObject
{
    [SerializeField] int fin;
    [SerializeField] float damage;
    [SerializeField] float forceMagnitude;
    
    public float Damage { get => damage;  }
    public float ForceMagnitude { get => forceMagnitude;  }
}
