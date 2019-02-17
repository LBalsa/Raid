using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Triggers an enemy spawner when the player is detected.
public class EnemyTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameController.inst && GameController.inst.spawnEnemies)
            {
                foreach (EnemySpawner sp in GetComponentsInChildren<EnemySpawner>())
                {
                    sp.Activate();
                    enabled = false;
                }
            }
        }
    }
}
