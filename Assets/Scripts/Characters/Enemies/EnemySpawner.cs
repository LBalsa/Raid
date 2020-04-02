using System.Collections;
using UnityEngine;
using Controllers;

namespace Characters.Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [Range(1, 5)]
        public int numberOfWaves;
        public bool lastSpawner;
        [SerializeField]
        private bool active = false;

        public int dif = 1;
        private int wave = 0;
        private int enemyCount = 0;
        private float delay = 0.5f;
        private float delay2 = 0.5f;

        // Enemy prefabs & wave information;
        public Spawner spawner;


        public void Start()
        {
            if (active)
            {
                wave = 3;
                StartCoroutine("SpawnDelay");
            }
        }

        public void Activate()
        {
            if (GameController.inst && GameController.inst.spawnEnemies)
            {
                if (!active)
                {
                    active = true;
                    dif = GameController.inst.Difficulty;
                    spawner.Validate();
                    StartCoroutine("SpawnDelay");
                }
            }
        }


        protected void SpawnEnemies()
        {
            enemyCount = 0;
            for (int i = 0; i < spawner.waves[wave].light * dif; i++)
            {
                StartCoroutine(InstantiateDelay(spawner.Light()));
            }
            for (int i = 0; i < spawner.waves[wave].heavy * dif; i++)
            {
                StartCoroutine(InstantiateDelay(spawner.Heavy()));
            }
            for (int i = 0; i < spawner.waves[wave].ranged * dif; i++)
            {
                StartCoroutine(InstantiateDelay(spawner.Ranged()));
            }
            for (int i = 0; i < spawner.waves[wave].swarmer * dif; i++)
            {
                StartCoroutine(InstantiateDelay(spawner.Swarmer()));
            }
            wave++;
        }

        // Used to delay time between round.
        protected IEnumerator SpawnDelay()
        {
            yield return new WaitForSeconds(1f);
            SpawnEnemies();
        }

        // TODO: this should possibly be changed/moved somewhere else, spawner should not depend on highly volatile Enemy, only on GameObject, interface needed?
        // This delay instantiates enemies only after their spawn cloud effect has begun.
        private IEnumerator InstantiateDelay(GameObject enemy)
        {
            delay2 += 0.5f;
            yield return new WaitForSeconds(delay2);
            Vector3 position = new Vector3(Random.insideUnitSphere.x, transform.position.y, Random.insideUnitSphere.z) + transform.position;
            // Instantiate enemy spawn effect.
            if (!enemy.GetComponent<Enemy>().fX)
            {
                Debug.LogError("FX structure missing!");
            }
            else if (!enemy.GetComponent<Enemy>().fX.vfx_spawn)
            {
                Debug.LogError("FX missing!");
            }
            else
            {
                Instantiate(enemy.GetComponent<Enemy>().fX.vfx_spawn, position, Quaternion.identity);
            }

            // Delay.
            yield return new WaitForSeconds(delay);

            // Instantiate enemy.
            GameObject e = Instantiate(enemy, position, Quaternion.identity) as GameObject;
            e.GetComponent<Enemy>().spawner = this;
            e.GetComponent<UnityEngine.AI.NavMeshAgent>().Warp(position);
            enemyCount++;
            GameController.inst.EnemySpawnCount++;
        }

        public void EnemyDown()
        {
            enemyCount--;
            GameController.inst.EnemyDeathCount++;
            if (enemyCount <= 0)
            {
                FinishWave();
            }
        }

        private void FinishWave()
        {
            Debug.Log(wave + " wave finished.");
            if (wave >= numberOfWaves)
            {
                if (lastSpawner)
                {
                    GameController.inst.LevelFinished();
                }
                this.enabled = false;
                return;
            }
            else
            {
                StartCoroutine("SpawnDelay");
            }
        }

        /* For testing purposes, disabled to prevent mistakes.
        protected void SpawnEnemies(int light, int heavy, int ranged, int swarmer)
        {
            for (int i = 0; i < light; i++)
            {
                Instantiate(enemyL[Random.Range(0, enemyL.Length)], new Vector3(Random.insideUnitSphere.x * 2, transform.position.y, Random.insideUnitSphere.z * 2) + transform.position, Quaternion.identity);
            }
            for (int i = 0; i < heavy; i++)
            {
                Instantiate(enemyH[Random.Range(0, enemyH.Length)], new Vector3(Random.insideUnitSphere.x * 2, transform.position.y, Random.insideUnitSphere.z * 2) + transform.position, Quaternion.identity);
            }
            for (int i = 0; i < ranged; i++)
            {
                Instantiate(enemyR[Random.Range(0, enemyR.Length)], new Vector3(Random.insideUnitSphere.x * 2, transform.position.y, Random.insideUnitSphere.z * 2) + transform.position, Quaternion.identity);
            }
            for (int i = 0; i < swarmer; i++)
            {
                Instantiate(enemyS[Random.Range(0, enemyS.Length)], new Vector3(Random.insideUnitSphere.x * 2, transform.position.y, Random.insideUnitSphere.z * 2) + transform.position, Quaternion.identity);
            }
            wave++;
        }
        */
    }
}