using UnityEngine;
 
namespace Weapons.WeaponUpgrades
{
    [CreateAssetMenu(menuName = "WeaponUpgrade/Ice")]
        public class MysticalAura : WeaponUpgrade
        {
            public override void ApplyActiveEffect(MainWeapon mainWeapon, IDestructable destructable)
            {
                throw new System.NotImplementedException();
            }

            public override void ApplyPassiveEffect(MainWeapon mainWeapon)
            {
                mainWeapon.GetComponentInChildren<BoxCollider>().size = mainWeapon.GetComponentInChildren<BoxCollider>().size * 2f;
            }

            public override void RemovePassiveEffect(MainWeapon mainWeapon)
            {
                mainWeapon.GetComponentInChildren<BoxCollider>().size = mainWeapon.GetComponentInChildren<BoxCollider>().size * 0.5f;
            }
        }
    }