using UnityEngine;
using Weapons;

public interface IWeaponUpgrade
{
    GameObject ActiveVfx { get; }
    GameObject PassiveVfx { get; }
    void ApplyActiveEffect(MainWeapon mainWeapon, IDestructable destructable);
    void ApplyPassiveEffect(MainWeapon mainWeapon);
    void RemovePassiveEffect(MainWeapon mainWeapon);
}
