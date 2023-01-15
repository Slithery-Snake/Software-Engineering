using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class WaveManager : MonoBehaviour
{
    public event UnityAction WaveDone;
    Transform spawn;
    [SerializeField] GameObject[,] wayPointList;
    HumanoidManager huManager;
    [SerializeField] int waitBetweenWaveEnd;
    [SerializeField] int waitBetweenSpawn;
    WaitForSecondsRealtime waitSpawn;
    WaitForSecondsRealtime waitEnd;
    int eCount;
    public static WaveManager Make(HumanoidManager hu)
    {
        GameObject obj = new GameObject();
        WaveManager me = obj.AddComponent<WaveManager>();
        me.huManager = hu;
        EnemyGunner.Point += me.CreateGunner;
        EnemySpawn.Point += me.CreateEnemy;
        me.waitEnd = new WaitForSecondsRealtime(me.waitBetweenSpawn);
        me.waitSpawn = new WaitForSecondsRealtime(me.waitBetweenWaveEnd);
        return me;
    }
    private void OnDestroy()
    {
        EnemyGunner.Point -= CreateGunner;
        EnemySpawn.Point -= CreateEnemy;
    }
    void CreateGunner(HumanoidManager.EnemyGunnerStruct e)
    {
        huManager.CreateEnemy(e);
    }
    void CreateEnemy(HumanoidManager.EnemyStruct e)
    {
        huManager.CreateEnemy(e);
    }
    public void StartWave()
    {
        StartCoroutine(Wave());
    }
    IEnumerator Wave()
    {
        for(int i = 0; i < wayPointList.Length;i ++)
        {

            for(int j = 0; j< wayPointList.GetLength(1); j ++)
            {
                wayPointList[i, j].SetActive(true);
                
                yield return waitBetweenSpawn;
            }

            yield return waitBetweenWaveEnd;
        }
    }
   
}
