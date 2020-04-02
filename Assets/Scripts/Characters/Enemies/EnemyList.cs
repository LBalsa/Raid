using UnityEngine;

namespace Characters.Enemies
{
    [CreateAssetMenu(fileName = "Enemies", menuName = "EnemyList")]
    public class EnemyList : ScriptableObject
    {
        public GameObject[] enemyL;
        public GameObject[] enemyH;
        public GameObject[] enemyR;
        public GameObject[] enemyS;

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
}