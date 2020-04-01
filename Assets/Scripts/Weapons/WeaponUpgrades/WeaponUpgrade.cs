using UnityEngine;

namespace Weapons
{
    namespace WeaponUpgrades
    {
        [CreateAssetMenu(menuName = "WeaponUpgrade/WeaponUpgrade")]
        public abstract class WeaponUpgrade : ScriptableObject, IWeaponUpgrade
        {
            // Passive and active VFX.
            [SerializeField]
            private GameObject vfx_active;
            [SerializeField]
            private GameObject vfx_passive;

            public GameObject ActiveVfx { get { return vfx_active; } }
            public GameObject PassiveVfx { get { return vfx_passive; } }
            public abstract void ApplyPassiveEffect(MainWeapon mainWeapon);
            public abstract void RemovePassiveEffect(MainWeapon mainWeapon);
            public abstract void ApplyActiveEffect(MainWeapon mainWeapon, IDestructable destructable);
        }
    }
}