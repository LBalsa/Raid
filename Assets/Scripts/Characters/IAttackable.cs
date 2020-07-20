using UnityEngine;

namespace Characters
{
    public delegate void Attack();
    public delegate void Death(bool gameover);
    public interface IAttackable
    {
        event Attack OnAttack;
        event Death OnDeath;
        void OnAttacked();
        bool CanBeAttacked { get; }
        Transform transform { get; }
    }
}
