using UnityEngine;

namespace Characters
{
    [CreateAssetMenu(menuName = ("Character/Stats"))]
    public class CharacterStats : ScriptableObject
    {
        [Header("Stats")]
        [SerializeField]    public float maxHealth;
        [SerializeField]    public float damage;
        [SerializeField]    public float attackCooldown;
        [SerializeField]    public int maxBlockHits;
        [SerializeField]    public float blockCooldown;
        [SerializeField]    public bool retreats;

        [Header("Navigation")]
        [SerializeField]    public float maxdistance;
        [SerializeField]    public float moveSpeed;
        [SerializeField]    public float rotationSpeed;
    }
}
