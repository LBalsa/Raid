using UnityEngine;

namespace Weapons
{
    [SelectionBase]
    public class MainWeapon : Weapon
    {
        public IWeaponUpgrade weaponUpgradeReference;
        public GameObject passiveEffectReference;

        public void Pickup(Transform parentHand, bool isPlayerFriendly, float dmg)
        {
            // Disable trigger.
            GetComponent<SphereCollider>().enabled = false;

            // Setup.
            SetUp(isPlayerFriendly, false, dmg);

            // Set parent and position.
            pos = new Vector3(0, 0, 0);
            rotation = new Vector3(0, 0, 0);

            transform.parent = parentHand;
            transform.localPosition = pos;
            transform.localEulerAngles = rotation;
        }

        public void Drop()
        {
            // Set as universal weapon.
            armed = false;
            SetLayer(Layers.Weapon);

            // Enable collider.
            Enable();

            // Free rigidbody.
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.None;

            // Remove parent.
            transform.parent = null;

            // Activate trigger.
            SphereCollider sc = GetComponent<SphereCollider>();
            sc.enabled = true;
            sc.isTrigger = true;

            // Save position
            pos = transform.position;
            rotation = transform.eulerAngles;
        }

        protected override void ApplyDamage(Collision collision)
        {
            // Apply damage.
            base.ApplyDamage(collision);

            // Apply upgrade effects.
            if (weaponUpgradeReference != null)
            {
                Instantiate(weaponUpgradeReference.ActiveVfx, collision.contacts[0].point, Quaternion.identity);
                weaponUpgradeReference.ApplyActiveEffect(this, collision.gameObject.GetComponent<IDestructable>());
            }

            if (throwable)
            {
                Destroy(this.gameObject);
            }
        }


        public void Upgrade(IWeaponUpgrade upgrade)
        {
            // Remove former upgrade and vfx.
            if (weaponUpgradeReference != null)
            {
                weaponUpgradeReference.RemovePassiveEffect(this);
                Destroy(passiveEffectReference);
            }

            // Save and apply new upgrade
            weaponUpgradeReference = upgrade;
            weaponUpgradeReference.ApplyPassiveEffect(this);

            // Passive vfx around weapon.
            passiveEffectReference = Instantiate(upgrade.PassiveVfx, transform.position, Quaternion.identity, transform);
            passiveEffectReference.transform.localRotation = Quaternion.Euler(90, 0, 0);
        }
    }
}
