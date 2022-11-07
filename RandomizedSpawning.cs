using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizedSpawning : MonoBehaviour
{
    public GameObject Enemy;
    public int x;
    public int z;
    public enemyCount;

    void Start()
    {
        StartCoroutline(enemyDrop());
    }

    IEnumerator enemyDrop()
    {
        while (enemyCount < 10)
        {
            x = Random.Range(1, 50);
            z = Random.Range(1, 50);
            Instantiate(Enemy, new Vector3(x, 50, z), Quaternion.Identity);
            yield return new WaitForSeconds(0.1f);
            enemyCount++;
        }
    }
}
