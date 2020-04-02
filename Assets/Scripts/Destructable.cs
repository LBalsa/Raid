using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;
using SpecialEffects.Structures;

//[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]

public class Destructable : MonoBehaviour, IDestructable
{
	public bool hasDrops = true;
	public int dropCount;

    bool hit;
    public float health = 4;
    public SimpleFXStructure fX;
    public LootTable lootTable;

    protected void Dest()
    {
        Destroy(this.gameObject);
    }

    public void TakeDamage(float damage)
    {
        if (!hit)
        {
            health -= damage;
            Effects(transform.position);
        }
    }

    public void TakeDamage(float damage, Collision collision)
    {
        if (!hit)
        {
            health -= damage;
            Effects(collision.contacts[0].point);
        }
    }

    void Effects(Vector3 pos)
    {
        hit = true;
        Invoke("ResetHit", 0.5f);
        if (health <= 0)
        {
            // Disable colliders and mesh renderer.
            foreach (Collider c in GetComponents<Collider>())
            {
                c.enabled = false;
            }

            GetComponent<MeshRenderer>().enabled = false;

            // Explosion sound and visual fx.
            if (fX.vfx_destroy) { Instantiate(fX.vfx_destroy, transform.position, Quaternion.identity); }
            if (fX.sfx_destroy) { fX.sfx_destroy.Play(GetComponent<AudioSource>()); }

            // Drop loot
			if (lootTable && hasDrops)
            {
				if (dropCount < 1) {
					dropCount = 1;
				}
				lootTable.DropLoot (dropCount, transform.position);
                //Instantiate(lootTable.DropLoot().gameObject, transform.position, Quaternion.identity);
            }

            // Delay destruction to preserve sound.
            Invoke("Dest", 2.5f);
        }
        else
        {
            try
            {
                fX.sfx_hit.Play(GetComponent<AudioSource>());
                Instantiate(fX.vfx_hit, pos, Quaternion.identity);
            }
            catch (System.Exception)
            {

                throw;
            }

        }
    }
    void ResetHit()
    {
        hit = false;
    }
}
