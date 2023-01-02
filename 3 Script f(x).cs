using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 3Scriptf(x) : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public void Update()
    {
        int gunID,
        int ammoID, ammoType, ammoAmount;

        public static void GunData()
        {
            Vector3int gunVector = new Vector3int(0, 0, 0);
            int gunID = 0;
            Vector3 Position = GameObject.Find("Gun").transform.position;
            //Send spawn data to central class
        }

        public static void EnemyAndPlayerSpawnData()
        {
            Vector3int EAPSD = new Vector3int(0, 0, 0);
            int EAPSDID = 0;
            Vector3int PlayerSpawn = new Vector3int(0, 0, 0);
            //Send spawn data to central class
        }

        public static void AmmoData()
        {
            Vector3int ammoVector = new Vector3int(0, 0, 0);
            int[] ammo = new ammo[2];
            ammo[0] = ammoID;
            ammo[1] = ammoType;
            ammo[2] = ammoAmount;
            //Send spawn data to central class
        }
    }

private Vector3 Min;
private Vector3 Max;
private float _xAxis;
private float _yAxis;
private Vector3 _randomPosition;
public bool _canInstantiate;

private void Start()
{
    SetRanges();
}

private void Update()
{
    _xAxis = UnityEngine.Random.Range(Min.x, Max.x);
    _yAxis = UnityEngine.Random.Range(Min.y, Max.y);
    _zAxis = UnityEngine.Random.Range(Min.z, Max.z);
    _randomPosition = new Vector3(_xAxis, _yAxis, _zAxis);
}

//Here put the ranges where your object will appear, or put it in the inspector.
private void SetRanges()
{
    Min = new Vector3(2, 4, 0); //Random value.
    Max = new Vector3(20, 40, 30); //Another ramdon value, just for the example.
}
private void InstantiateRandomObjects()
{
    if (_canInstantiate)
    {
        Instantiate(gameObject, _randomPosition, Quaternion.identity);
    }

}