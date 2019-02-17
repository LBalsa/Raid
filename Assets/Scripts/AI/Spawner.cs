using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawner", menuName = "Spawner")]
public class Spawner : ScriptableObject
{
    [System.Serializable]
    public struct EnemyMix
    {
        [Range(0, 5)]
        public int light;
        [Range(0, 5)]
        public int heavy;
        [Range(0, 5)]
        public int ranged;
        [Range(0, 5)]
        public int swarmer;

        public EnemyMix(int l, int h, int r, int s)
        {
            light = l;
            heavy = h;
            ranged = r;
            swarmer = s;
        }
    }

    [Header("Waves")]
    public EnemyMix[] waves = new EnemyMix[5];

    [Header("Enemy Prefabs")]

    public GameObject[] enemyL;
    public GameObject[] enemyH;
    public GameObject[] enemyR;
    public GameObject[] enemyS;
    public void Validate()
    {
        foreach (EnemyMix wave in waves)
        {
            int sum = wave.light + wave.heavy + wave.ranged + wave.swarmer;
            if (sum <= 0)
            {
                Debug.LogError(name + " has an empty wave.");
            }
        }
    }
    
    public GameObject Light()
    {
        return enemyL[Random.Range(0, enemyL.Length)];
    }
    public GameObject Heavy()
    {
        return enemyH[Random.Range(0, enemyH.Length)];
    }
    public GameObject Ranged()
    {
        return enemyR[Random.Range(0, enemyR.Length)];
    }
    public GameObject Swarmer()
    {
        return enemyS[Random.Range(0, enemyS.Length)];
    }


}
