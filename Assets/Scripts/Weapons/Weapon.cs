using Characters;
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
        [Tooltip("Total weapon+wielder damage.")]
        [HideInInspector]
        public float damage;
        public float speed = 1;
        public bool armed = false;
        protected Rigidbody rb;
        protected Vector3 pos = new Vector3(0, 0, 0);
        protected Vector3 rotation = new Vector3(0, 0, 0);
        protected List<GameObject> gameObjectsHit = new List<GameObject>();


        // Colliders.
        protected SphereCollider pickupTrigger; 
        protected List<Collider> colliders = new List<Collider>();

        public void SetUp(CharacterFaction characterFaction, bool isThrowable, float dmg)
        {
            WeaponLayers weaponLayer;
            switch (characterFaction)
            {
                case CharacterFaction.Ally: weaponLayer = WeaponLayers.PlayerWeapon; break;
                case CharacterFaction.Neutral: weaponLayer = WeaponLayers.PlayerWeapon; break;
                case CharacterFaction.Enemy: weaponLayer = WeaponLayers.EnemyWeapon; break;
                default: weaponLayer = WeaponLayers.Weapon; break;
            }

            SetLayer(weaponLayer);
            SetDamage(dmg);
            SetupColliders();
            ConfigureThrowable(isThrowable);
            armed = true;
        }

        private void SetDamage(float dmg)
        {
            damage = baseDamage + dmg;
        }

        private void ConfigureThrowable(bool isThrowable)
        {
            throwable = isThrowable;

            if (!rb)
            {
                rb = GetComponent<Rigidbody>();
            }

            rb.useGravity = false;
            rb.isKinematic = false;
            rb.constraints = isThrowable ? RigidbodyConstraints.None : RigidbodyConstraints.FreezeAll;
        }

        private void SetupColliders()
        {
            foreach (var coll in GetComponentsInChildren<Collider>())
            {
                if (!coll.isTrigger)
                {
                    colliders.Add(coll);
                }
                coll.enabled = false;
            }
        }

        public void Enable()
        {
            ResetPosition(); // Glue weapon to hand.
            foreach (Collider coll in colliders)
            {
                coll.enabled = true;
            }
        }

        public void Disable()
        {
            gameObjectsHit.Clear();
            ResetPosition();
            foreach (var coll in colliders)
            {
                coll.enabled = false;
            }
        }

        public void Throw()
        {
            foreach (var coll in colliders)
            {
                rb.useGravity = true;
                coll.enabled = true;
            }
        }

        private void ResetPosition()
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
                foreach (var hit in gameObjectsHit)
                {
                    if (collision.gameObject == hit)
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

        public void SetLayer(WeaponLayers weaponLayer, bool includeChildren = true)
        {
            gameObject.layer = (int)weaponLayer;
            if (includeChildren)
            {
                foreach (Transform trans in gameObject.transform.GetComponentsInChildren<Transform>(true))
                {
                    trans.gameObject.layer = (int)weaponLayer;
                }
            }
            tag = weaponLayer.ToString();
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
