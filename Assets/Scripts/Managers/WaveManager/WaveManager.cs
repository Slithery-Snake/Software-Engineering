using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class WaveManager : MonoBehaviour
{
    public event UnityAction WaveDone;
    [SerializeField] int enemiesPerWave;
    [SerializeField] int wavesAmount;
    HumanoidManager huManager;
    [SerializeField] int waitBetweenWaveEnd;
    [SerializeField] int waitBetweenSpawn;
    WaitForSecondsRealtime waitSpawn;
    WaitForSecondsRealtime waitEnd;
    int eCount;
    Vector3 spawnPos;
    public static WaveManager Make(HumanoidManager hu, Vector3 spawnPos, int enemiesPerWave,int wavesAmount, int waitBetweenWave, int waitBetweenSpan)
    {
        Debug.Log("wave manager made");
        GameObject obj = new GameObject();
        WaveManager me = obj.AddComponent<WaveManager>();
        me.spawnPos = spawnPos;
        me.huManager = hu;
        EnemyGunner.Point += me.CreateGunner;
        EnemySpawn.Point += me.CreateEnemy;
        me.enemiesPerWave = enemiesPerWave;
        me.wavesAmount = wavesAmount;
        me.waitBetweenSpawn = waitBetweenSpan;
        me.waitBetweenWaveEnd = waitBetweenWave;
        me.waitEnd = new WaitForSecondsRealtime(me.waitBetweenWaveEnd);
        me.waitSpawn = new WaitForSecondsRealtime(me.waitBetweenSpawn);
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
        for(int i = 0; i < wavesAmount;i ++)
        {

            for(int j = 0; j< enemiesPerWave; j ++)
            {
                int mOrG = Random.Range(1, 6);
               if(mOrG == 1)
                {
                    huManager.CreateEnemy(spawnPos, 0, 3);
                } else
                {
                    int whichGun = Random.Range(1, 4);
                    huManager.CreateEnemy(spawnPos, whichGun, whichGun + 19,0, 2);
                }
                Debug.Log("spawned");
                yield return waitSpawn;
            }

            yield return waitEnd ;
        }
        WaveDone?.Invoke();
    }
   
}
