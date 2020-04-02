using UnityEngine;

namespace Weapons.WeaponUpgrades

{
    [CreateAssetMenu(menuName = "WeaponUpgrade/Group")]
        public class WeaponUpgradeGroup : ScriptableObject
        {
            public WeaponUpgrade[] Upgrades;
        }
    }