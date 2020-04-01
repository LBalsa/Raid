using UnityEngine;

namespace Weapons
{
    namespace WeaponUpgrades
    {
        [CreateAssetMenu(menuName = "WeaponUpgrade/Fire")]
        public class Fire : WeaponUpgrade
        {
            [Range(1,5)]
            public float attackEffect;

            public override void ApplyActiveEffect(MainWeapon mainWeapon, IDestructable destructable)
            {
                throw new System.NotImplementedException();
            }

            public override void ApplyPassiveEffect(MainWeapon mainWeapon)
            {
                // Increase damage by 50%
                float yielderDamage = mainWeapon.damage-mainWeapon.baseDamage;
                mainWeapon.baseDamage *= 1.5f;
                mainWeapon.damage = mainWeapon.baseDamage + yielderDamage;
            }

            public override void RemovePassiveEffect(MainWeapon mainWeapon)
            {

                float yielderDamage = mainWeapon.damage-mainWeapon.baseDamage;
                mainWeapon.baseDamage = mainWeapon.baseDamage * (1f / 1.5f);
                mainWeapon.damage = mainWeapon.baseDamage + yielderDamage;
            }
        }
    }
}