using Controllers;
using SpecialEffects.Structures;
using UnityEngine;
using UnityEngine.UI;
using Weapons;

namespace Characters.Player
{
    public class HealthManager : MonoBehaviour, IDestructable
    {
        public Death OnDeath;

        public bool IsAlive { get; private set; } = true;
        [SerializeField]
        private float playerHealth;
        [SerializeField]
        private float playerMaxHealth;


        public Sprite[] healthimages;
        public Image healthbar;
        public Text gold;
        private Animator anim;
        private AudioSource audioSource;
        private PlayerFXStructure fx;

        private void Start()
        {
            anim = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            gold.text = "0";

            fx = PlayerController.inst.fx;
        }

        // Pick up objects.
        private void OnTriggerEnter(Collider other)
        {
            // Food heals.
             if (other.gameObject.tag == "Food" && playerHealth < playerMaxHealth)
            {
                Heal(1);
                Destroy(other.gameObject);
            }
        }

        // Damage from other sources. N/A
        public void TakeDamage(float damage)
        {
            return;
            /*
            if (playerHealth > 0)
            {
                playerHealth -= damage;

            }
            else if (playerHealth <= 0)
            {
                Death();
                playerHealth = 0;
            }
            */
        }

        // Damage from enemies.
        public void TakeDamage(float damage, Collision collision)
        {
            if (playerHealth <= 0)
            {
                return;
            }

            playerHealth -= damage;

            if (playerHealth > 0)
            {
                Instantiate(fx.vfx_hurt, collision.contacts[0].point, Quaternion.identity);
                TakeDamageEffects();
            }
            else if (playerHealth <= 0)
            {
                playerHealth = 0;
                OnDeath?.Invoke(true);
                Death();
            }
            UpdateHealthbar(playerHealth);
        }

        private void TakeDamageEffects()
        {
            fx.sfx_hurt.Play(audioSource);
            anim.SetTrigger("hit");
            Invoke("ResetTriggers", 0.2f);
        }

        public void HealthUpgrade(int health)
        {
            playerMaxHealth += health;
            playerHealth = playerMaxHealth;
            UpdateHealthbar(playerHealth);
        }

        public void Heal(int healAmout)
        {
            playerHealth += healAmout;

            if (playerHealth > playerMaxHealth)
            {
                playerHealth = playerMaxHealth;
            }

            UpdateHealthbar(playerHealth);
        }

        private void UpdateHealthbar(float playerhealth)
        {
            // Convert player health into healthbar sprite range to select from.
            float healthInRange = playerHealth * ((float)healthimages.Length - 1) / playerMaxHealth;
            healthbar.sprite = healthimages[Mathf.RoundToInt(healthInRange)];
        }

        // Move to player controller
        private void Death()
        {
            IsAlive = false;
            anim.SetTrigger("Dead");
            Invoke("ResetTriggers", 0.2f);
            PlayerController.inst.CanMove = false;
            GameController.inst.GameOver();

            Instantiate(fx.vfx_despawn, transform.position, Quaternion.identity);

        }

        private void ResetTriggers()
        {
            //anim.ResetTrigger("Dead");
            anim.ResetTrigger("hit");

        }
    }
}