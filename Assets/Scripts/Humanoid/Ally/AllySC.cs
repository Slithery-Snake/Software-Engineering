using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyStuff;
[CreateAssetMenu(fileName = "AllyData", menuName = "AllySC")]
public class AllySC : HumanoidSC
{
    [SerializeField] int turnSpeed;
    [SerializeField] float shootAngle;
    [SerializeField] float folloDistance;
    [SerializeField] int viewingDistance;
    [SerializeField] int shootingDistance;
    public int RotateSpeed { get => turnSpeed; }
    public float ShootAngle { get => shootAngle; }
    public float FolloDistance { get => folloDistance;}
    public int ViewingDistance { get => viewingDistance; }
    public int ShootingDistance { get => shootingDistance; }
}
