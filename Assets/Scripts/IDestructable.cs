using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDestructable
{
    /// <summary>
    /// Allows object to receive damage.
    /// </summary>
    /// <param name="damage"></param>
    void TakeDamage(float damage);


    /// <summary>
    /// Allows object to receive damage and
    /// provides collision details so sfx can be instantiated at hit location.
    /// </summary>
    /// <param name="Received damage"></param>
    /// <param name="collision"></param>
    void TakeDamage(float damage, Collision collision);
}
