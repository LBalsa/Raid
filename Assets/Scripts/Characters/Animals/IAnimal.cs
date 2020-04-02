using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimal
{
    void Aproach(GameObject other);
    void Hit(GameObject other);
}
