using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EnemyData", menuName = "EnemySC")]
public class EnemySC : HumanoidSC
{

    [SerializeField] float rotateSpeed;
    [SerializeField]float viewLength;
    [SerializeField] int viewAngle;
    [SerializeField] int shootAngle;
    public float RotateSpeed { get => rotateSpeed; }
    public float ViewLength { get => viewLength; }
    public int ViewAngle { get => viewAngle; }
    public int ShootAngle { get => shootAngle; }
}
