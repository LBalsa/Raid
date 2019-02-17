using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[SelectionBase]
public class Weapon : MonoBehaviour
{
    protected bool throwable = false;
    [Tooltip("Base weapon damage.")]
    public float baseDamage;
    public float yielderDamage;
    [Tooltip("Total weapon+wielder damage.")]
    [HideInInspector]
    public float damage;
    public float speed = 1;
    public bool armed = false;
    protected Rigidbody rb;
    protected Vector3 pos = new Vector3(0, 0, 0);
    protected Vector3 rotation = new Vector3(0, 0, 0);
    protected List< GameObject> gameObjectsHit = new List<GameObject>();
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

        throwable = isThrowable;

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
            rb.isKinematic = false; // So it can collide.
            rb.constraints = RigidbodyConstraints.None;
        }
        else
        {
            rb.useGravity = false; // No droppy droppy.
            rb.isKinematic = false; // So it can collide.
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
        gameObjectsHit.Clear();
        ResetPos(); // Glue weapon to hand.
        foreach (Collider coll in colliders)
        {
            coll.enabled = false;
        }
    }

    public void Throw()
    {
        foreach (Collider coll in colliders)
        {
            rb.useGravity = true;
            coll.enabled = true;
        }
    }

    private void ResetPos()
    {
        // Reset weapon position.
        transform.localPosition = pos;
        transform.localEulerAngles = rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<IDestructable>() != null && armed)
        {
            // Prevent double damage.
            foreach (var item in gameObjectsHit)
            {
                if (collision.gameObject == item)
                {
                    return;
                }
            }
            gameObjectsHit.Add(collision.gameObject);

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
