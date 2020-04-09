using UnityEngine;

    namespace Weapons.WeaponUpgrades
    {
        [CreateAssetMenu(menuName = "WeaponUpgrade/Fire")]
        public class Fire : WeaponUpgrade
        {
            [Range(1,2)]
            public float extraDamage;

            public override void ApplyActiveEffect(MainWeapon mainWeapon, IDestructable destructable)
            {
            }

            public override void ApplyPassiveEffect(MainWeapon mainWeapon)
            {
                float yielderDamage = mainWeapon.damage-mainWeapon.baseDamage;
                mainWeapon.baseDamage *= extraDamage;
                mainWeapon.damage = mainWeapon.baseDamage + yielderDamage;
            }

            public override void RemovePassiveEffect(MainWeapon mainWeapon)
            {
                float yielderDamage = mainWeapon.damage-mainWeapon.baseDamage;
                mainWeapon.baseDamage = mainWeapon.baseDamage * (1f / extraDamage);
                mainWeapon.damage = mainWeapon.baseDamage + yielderDamage;
            }
        }
    }