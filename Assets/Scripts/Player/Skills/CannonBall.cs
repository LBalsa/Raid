using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]

public class CannonBall : MonoBehaviour
{
    float damage;
    float radius;
    VariableVolumePitch cannonSFX;
    VariableVolumePitch fallSFX;
    VariableVolumePitch explosionSFX;
    GameObject explosionVFX;

    // Use this for initialization
    void Start()
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

        // Inter shot elay
        Invoke("Fire", delay / 2);
        Invoke("Fall", 2 + delay);

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

        // Deal extra damage if enemy was hit directly.
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }

        // Search for enemeis in collision radius.
        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider col in objectsInRange)
        {
            IDestructable destructable = col.GetComponent<IDestructable>();
            if (destructable != null && col.name != "Player")
            {
                // Linear damage falloff.
                float proximity = (transform.position - col.gameObject.transform.position).magnitude;
                float actualDamage = 1.5f - (proximity / radius);

                //Debug.Log("Cannonball damage: " + damage * actualDamage);
                if (damage > 0)
                {
                    destructable.TakeDamage(damage * actualDamage);
                }
            }
        }




        // Leave mark if hit static objects;
        if (!collision.gameObject.CompareTag("Enemy") && !collision.gameObject.CompareTag("Player"))
        {
            // Instantiate(print);
        }
        GetComponentInChildren<MeshRenderer>().enabled = false;
        GetComponent<SphereCollider>().enabled = false;
        Invoke("DestroyThis", 2);
    }

    void DestroyThis()
    {
        Destroy(this.gameObject);
    }
}
