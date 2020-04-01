using System.Collections.Generic;
using UnityEngine;

namespace Weapons
{
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

        // weapon layers
        public enum Layers { Weapon = 0, PlayerWeapon = 9, EnemyWeapon = 10 };

        // Colliders.
        protected SphereCollider pickupTrigger;
        protected List<Collider> colliders = new List<Collider>();

        public void SetUp(bool isPlayerFriendly, bool isThrowable, float dmg)
        {
            SetLayer(isPlayerFriendly ? Layers.PlayerWeapon : Layers.EnemyWeapon);

            throwable = isThrowable;

            // Arm weapon.
            damage = baseDamage + dmg;
            armed = true;

            // Get and disable colliders.
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
            if (CanApplyDamage(collision))
            {
                ApplyDamage(collision);
            }
        }

        protected bool CanApplyDamage(Collision collision)
        {
            if (collision.gameObject.GetComponent<IDestructable>() != null && armed)
            {
                // Prevent double damage by caching already hit objects.
                foreach (var item in gameObjectsHit)
                {
                    if (collision.gameObject == item)
                    {
                        return false;
                    }
                }
                gameObjectsHit.Add(collision.gameObject);
                return true;
            }
            else
            {
                return false;
            }
        }

        protected virtual void ApplyDamage(Collision collision)
        {
            collision.gameObject.GetComponent<IDestructable>().TakeDamage(damage, collision);
        }

        public void SetLayer(Layers layer, bool includeChildren = true)
        {
            gameObject.layer = (int)layer;
            if (includeChildren)
            {
                foreach (Transform trans in gameObject.transform.GetComponentsInChildren<Transform>(true))
                {
                    trans.gameObject.layer = (int)layer;
                }
            }
            tag = layer.ToString();
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
}
