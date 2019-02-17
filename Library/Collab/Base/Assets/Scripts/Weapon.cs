using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[SelectionBase]
public class Weapon : MonoBehaviour
{
    [Tooltip("Base weapon damage.")]
    public float baseDamage;
    [Tooltip("Total weapon+wielder damage.")]
    public float damage;
    public float speed = 1;

    protected bool armed = false;
    protected Rigidbody rb;
    protected Vector3 pos = new Vector3(0, 0, 0);
    protected Vector3 rotation = new Vector3(0, 0, 0);

    // Colliders.
    protected SphereCollider pickupTrigger;
    protected List<Collider> colliders = new List<Collider>();

    public void SetUp(bool isPlayerFriendly, bool isThrowable, float dmg)
    {
        // Set layer for collision.
        if (isPlayerFriendly)
        {
            tag = "PlayerWeapon";
            SetLayer(9);
        }
        else
        {
            tag = "EnemyWeapon";
            SetLayer(10);
        }

        // Arm weapon.
        damage = baseDamage + dmg;
        armed = true;

        // Get colliders and disable colliders.
        foreach (Collider coll in GetComponentsInChildren<Collider>())
        {
            if (!coll.isTrigger)
            {
                colliders.Add(coll);
            }
            coll.enabled = false;
        }

        // Lock rigidbody.
        if (!rb)
        {
            rb = GetComponent<Rigidbody>();
        }
        if (isThrowable)
        {
            rb.useGravity = false; // No droppy droppy.
            rb.isKinematic = false; // So it can be not kinematic.
            rb.constraints = RigidbodyConstraints.None;
        }
        else
        {
            rb.useGravity = false; // No droppy droppy.
            rb.isKinematic = false; // So it can be not kinematic.
            rb.constraints = RigidbodyConstraints.FreezeAll;  // Constrain all transforms.
        }
    }


    public void Enable()
    {
        ResetPos(); // Glue weapon to hand.
        foreach (Collider coll in colliders)
        {
            coll.enabled = true;
        }
    }

    public void Disable()
    {
        ResetPos(); // Glue weapon to hand.
        foreach (Collider coll in colliders)
        {
            coll.enabled = false;
        }
    }

    void ResetPos()
    {
        // Reset weapon position.
        transform.localPosition = pos;
        transform.localEulerAngles = rotation;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<IDestructable>() != null && armed)
        {
            collision.gameObject.GetComponent<IDestructable>().TakeDamage(damage, collision);
        }
    }

    public void SetLayer(int layer, bool includeChildren = true)
    {
        gameObject.layer = layer;
        if (includeChildren)
        {
            foreach (Transform trans in gameObject.transform.GetComponentsInChildren<Transform>(true))
            {
                trans.gameObject.layer = layer;
            }
        }
    }





    //void Reset()
    //{
    //    foreach (Collider coll in GetComponents<Collider>())
    //    {
    //        //coll.isTrigger = true;
    //        coll.enabled = false;
    //    }
    //    if (GetComponent<Rigidbody>())
    //    {
    //        GetComponent<Rigidbody>().isKinematic = true;
    //    }
    //}

}
