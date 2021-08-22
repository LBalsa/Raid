using SpecialEffects;
using UnityEngine;
using Weapons;

namespace Skills
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(SphereCollider))]

    public class CannonBall : MonoBehaviour
    {
        private float damage;
        private float radius;
        private VariableVolumePitch cannonSFX;
        private VariableVolumePitch fallSFX;
        private VariableVolumePitch explosionSFX;
        private GameObject explosionVFX;

        // Use this for initialization
        private void Start()
        {
            GetComponentInChildren<MeshRenderer>().enabled = false;
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }

        public void Set(float delay, float dmg, float explosionRadius, VariableVolumePitch cannonSfx, VariableVolumePitch fallSfx, VariableVolumePitch explosionSfx, GameObject explosionVfx)
        {
            damage = dmg;
            radius = explosionRadius;
            // Sound effects.
            cannonSFX = cannonSfx;
            fallSFX = fallSfx;
            explosionSFX = explosionSfx;

            // Visual effects.
            explosionVFX = explosionVfx;

            SetLayer(WeaponLayers.PlayerWeapon, true);

            // Inter shot elay
            Invoke("Fire", delay / 2);
            Invoke("Fall", 2 + delay);

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

        public void Fire()
        {
            cannonSFX.Play(GetComponent<AudioSource>());
            //Invoke("Fall", 1);
        }

        public void Fall()
        {
            fallSFX.Play(GetComponent<AudioSource>());
            GetComponentInChildren<MeshRenderer>().enabled = true;
            GetComponent<Rigidbody>().isKinematic = false;
        }

        private void OnCollisionEnter(Collision collision)
        {
            explosionSFX.Play(GetComponent<AudioSource>());
            Instantiate(explosionVFX, transform.position, Quaternion.identity);

            DealDirectDamage(collision);
            DealRadialDamage();

            // Leave mark if hit static objects;
            if (!collision.gameObject.CompareTag("Enemy") && !collision.gameObject.CompareTag("Player"))
            {
                // Instantiate(print);
            }
            GetComponentInChildren<MeshRenderer>().enabled = false;
            GetComponent<SphereCollider>().enabled = false;
            Invoke("DestroyThis", 2);
        }

        // TODO : Replace with layer to avoid unintended collisions
        private void DealDirectDamage(Collision collision)
        {
            //if (collision.gameObject.CompareTag("Enemy"))
            //{
                collision.gameObject.GetComponent<IDestructable>().TakeDamage(damage);
            //}
        }

        private void DealRadialDamage()
        {
            Collider[] objectsInRange = Physics.OverlapSphere(transform.position, radius);
            foreach (Collider col in objectsInRange)
            {
                var destructible = col.GetComponent<IDestructable>();
                if (destructible != null && col.name != "Player")
                {
                    var proximity = (transform.position - col.gameObject.transform.position).magnitude;
                    var falloffDamage = 1.5f - (proximity / radius);

                    if (damage > 0)
                    {
                        destructible.TakeDamage(damage * falloffDamage);
                    }
                }
            }
        }

        private void DestroyThis()
        {
            Destroy(this.gameObject);
        }
    }
}