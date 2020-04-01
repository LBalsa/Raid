using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/WeaponUpgrade")]
public class WeaponUpgrade : ScriptableObject
{
    public enum UpgradeType { none, fire, ice, poison }

    [System.Serializable]
    public class WeaponUpgradeEffects
    {
        public UpgradeType upgradeType;
        // Passive and active VFX.
        public GameObject vfx_passive;
        public GameObject vfx_active;
        public float attackEffect;

        public float ExtraDamage() { return 0; }
    }


    // Passive and active VFX.
    [Header("Fire")]
    public GameObject vfx_passive_fire;
    public GameObject vfx_attack_fire;
    [Range(1,5)]
    public float fireAttackBuff = 1.5f;
    [Header("Ice")]
    public GameObject vfx_passive_ice;
    public GameObject vfx_attack_ice;
    [Header("Poison")]
    public GameObject vfx_attack_poison;
    public GameObject vfx_passive_poison;



    public void ApplyPassiveEffect(MainWeapon mainWeapon, UpgradeType upgradeType)
    {
        switch (upgradeType)
        {
            case UpgradeType.none:
                break;
            case UpgradeType.fire:
                // Increase damage by 50%
                float yielderDamage = mainWeapon.damage-mainWeapon.baseDamage;
                mainWeapon.baseDamage *= 1.5f;
                mainWeapon.damage = mainWeapon.baseDamage + yielderDamage;
                break;
            case UpgradeType.ice:
                mainWeapon.GetComponentInChildren<BoxCollider>().size = mainWeapon.GetComponentInChildren<BoxCollider>().size * 2f;
                break;
            case UpgradeType.poison:
                break;
            default:
                break;
        }
    }

    public void RemovePassiveEffect(MainWeapon mainWeapon, UpgradeType upgradeType)
    {
        switch (upgradeType)
        {
            case UpgradeType.none:
                break;
            case UpgradeType.fire:
                float yielderDamage = mainWeapon.damage-mainWeapon.baseDamage;
                mainWeapon.baseDamage = mainWeapon.baseDamage * (1f / 1.5f);
                mainWeapon.damage = mainWeapon.baseDamage + yielderDamage;
                break;
            case UpgradeType.ice:
                mainWeapon.GetComponentInChildren<BoxCollider>().size = mainWeapon.GetComponentInChildren<BoxCollider>().size * 0.5f;
                break;
            case UpgradeType.poison:
                break;
            default:
                break;
        }
    }

    public void ApplyActiveEffect(MainWeapon mainWeapon, UpgradeType upgradeType, IDestructable destructable)
    {
        switch (upgradeType)
        {
            case UpgradeType.none:
                break;
            case UpgradeType.fire:
                break;
            case UpgradeType.ice:
                break;
            case UpgradeType.poison:
                break;
            default:
                break;
        }
    }
    public GameObject GetPassiveEffect(UpgradeType upgradeType)
    {
        switch (upgradeType)
        {
            case UpgradeType.none:
                return null;
            case UpgradeType.fire:
                return vfx_passive_fire;
            case UpgradeType.ice:
                return vfx_passive_ice;
            case UpgradeType.poison:
                return vfx_passive_poison;
            default:
                return null;
        }
    }
    public GameObject GetActiveEffect(UpgradeType upgradeType)
    {
        switch (upgradeType)
        {
            case UpgradeType.none:
                return null;
            case UpgradeType.fire:
                return vfx_attack_fire;
            case UpgradeType.ice:
                return vfx_attack_ice;
            case UpgradeType.poison:
                return null;
            default:
                return null;
        }
    }
}