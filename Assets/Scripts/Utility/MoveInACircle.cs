using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public class MoveInACircle : MonoBehaviour
    {

        [Range(0f, 6.28f)]
        public float offset;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            float xPos = Mathf.Cos(offset);
            float zPos = Mathf.Sin(offset);

            this.transform.position = new Vector3(xPos, this.transform.position.y, zPos);
        }

        // Update is called once per frame
        private Vector3 GetCirclePos(float offset, float radius)
        {
            float xPos = Mathf.Cos(offset);
            float zPos = Mathf.Sin(offset);

            return new Vector3(xPos * radius, 0f, zPos * radius);
        }

        //THIS ENEMY'S ATTACK POSITION = GetCirclePos( 2pi / numEnemies) * myEnemyNumber );
    }
}