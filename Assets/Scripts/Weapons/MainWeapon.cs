using UnityEngine;

[SelectionBase]
public class MainWeapon : Weapon
{
    public WeaponUpgrade weaponUpgradeReference;
    public WeaponUpgrade.UpgradeType upgradeType = WeaponUpgrade.UpgradeType.none;
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


        pos = transform.position;
        rotation = transform.eulerAngles;
    }

    protected override void ApplyDamage(Collision collision)
    {
        // Apply damage.
        base.ApplyDamage(collision);

        // Upgrade effects.
        if (weaponUpgradeReference && upgradeType != WeaponUpgrade.UpgradeType.none)
        {
            // Visual effects.
            Instantiate(weaponUpgradeReference.GetActiveEffect(upgradeType), collision.contacts[0].point, Quaternion.identity);
            // Extra effects.
            weaponUpgradeReference.ApplyActiveEffect(this, upgradeType, collision.gameObject.GetComponent<IDestructable>());
        }

        if (throwable)
        {
            Destroy(this.gameObject);
        }
    }

    public void FireDamage()
    {

    }

    public void Upgrade(WeaponUpgrade upgrade, WeaponUpgrade.UpgradeType type)
    {
        // Check if upgrade reference is null.
        if (upgrade)
        {
            weaponUpgradeReference = upgrade;
            if (upgradeType != WeaponUpgrade.UpgradeType.none)
            {
                // Remove former passive effect and vfx.
                weaponUpgradeReference.RemovePassiveEffect(this, upgradeType);
                Destroy(passiveEffectReference);
            }

            // If there is an upgrade type, apply it.
            if (type != WeaponUpgrade.UpgradeType.none)
            {
                upgradeType = type;

                // Passive effect.
                weaponUpgradeReference.ApplyPassiveEffect(this, upgradeType);

                // Passive vfx around weapon.
                passiveEffectReference = Instantiate(upgrade.GetPassiveEffect(upgradeType), transform.position, Quaternion.identity, transform);
                passiveEffectReference.transform.localRotation = Quaternion.Euler(90, 0, 0);
            }
        }
    }

    //public void Upgrade(UpgradeType upgradeType, GameObject attackEffect, GameObject passiveEffect)
    //{
    //    if (upgrade == UpgradeType.none)
    //    {
    //        upgrade = upgradeType;
    //        vfx_passive = passiveEffect;
    //        vfx_attack = attackEffect;
    //        damage *= 2;
    //        GameObject go = Instantiate(passiveEffect, transform.position, Quaternion.identity, transform);
    //        go.transform.localRotation = Quaternion.Euler(90, 0, 0);
    //    }
    //}

}
