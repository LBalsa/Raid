using UnityEngine;

namespace Characters
{
    [System.Serializable]
    [CreateAssetMenu(menuName = ("Character/Stats"))]
    public class CharacterStats : ScriptableObject
    {
        [Header("Stats")]
        [SerializeField]    protected float maxHealth;
        [SerializeField]    protected float damage;
        [SerializeField]    protected float attackCooldown;
        [SerializeField]    protected float blockCooldown;
        [SerializeField]    protected bool retreats;

        [Header("Navigation")]
        [SerializeField]    protected float maxdistance;
        [SerializeField]    protected float moveSpeed;
        [SerializeField]    protected float rotationSpeed;
    }
}
