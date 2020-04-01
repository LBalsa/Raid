using UnityEngine;

namespace Weapons
{
    namespace WeaponUpgrades
    {
        [CreateAssetMenu(menuName = "WeaponUpgrade/Group")]
        public class WeaponUpgradeGroup : ScriptableObject
        {
            public WeaponUpgrade[] Upgrades;
        }
    }
}