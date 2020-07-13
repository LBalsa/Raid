using Controllers;
using SpecialEffects.Structures;
using UnityEngine;
using UnityEngine.UI;
using Weapons;

namespace Characters.Player
{
    public class HealthManager : MonoBehaviour, IDestructable
    {
        public static HealthManager inst;

        public bool IsAlive { get; private set; } = true;
        public bool HasBread { get; private set; } = false;

        public int Money { get; set; } = 0;

        public float playerHealth;
        public float playerMaxHealth;


        public Sprite[] healthimages;
        public GameObject weaponPickupTooltip;
        public Image healthbar;
        public Text gold;
        private Animator anim;
        private AudioSource audioSource;
        private PlayerFXStructure fx;

        private void Awake()
        {
            inst = this;
        }

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
            // Money is added to...money.
            if (other.gameObject.tag == "Silver")
            {
                fx.Play(audioSource, fx.sfx_coin);
                Money++;
                gold.text = Money.ToString();
                Destroy(other.gameObject);
            }
            else if (other.gameObject.tag == "Gold")
            {
                fx.Play(audioSource, fx.sfx_coin);
                Money += 5;
                gold.text = Money.ToString();
                Destroy(other.gameObject);
            }

            // Food heals.
            else if (other.gameObject.tag == "Food" && playerHealth < playerMaxHealth)
            {
                Heal(1);
                Destroy(other.gameObject);
            }

            // TODO: move this to weapon object
            // Weapons should make a tooltip come up.
            if (other.GetComponent<MainWeapon>())
            {
                //Vector3 x = new Vector3(0,1,0);
                //weaponPickupTooltip.transform.position = other.transform.position + x;
                //weaponPickupTooltip.SetActive(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // Closes weapon pickup tooltip.
            if (other.GetComponent<MainWeapon>())
            {
                //weaponPickupTooltip.SetActive(false);
            }
        }

        private void Update()
        {
            if (IsAlive && PlayerController.inst.CanMove)// && !GameController.inst.Paused)
            {
                // Do something fun.

                // Updates health bar.
                //Changehealthbar(playerHealth);
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
                Death();
                playerHealth = 0;

            }
            Changehealthbar(playerHealth);
        }

        private void TakeDamageEffects()
        {
            fx.sfx_hurt.Play(audioSource);
            anim.SetTrigger("hit");
            Invoke("ResetTriggers", 0.2f);
        }

        public void HealthUpgrade()
        {
            playerMaxHealth += 2;
            playerHealth = playerMaxHealth;
            Changehealthbar(playerHealth);
        }

        public void Heal(int healAmout)
        {
            playerHealth += healAmout;

            if (playerHealth > playerMaxHealth)
            {
                playerHealth = playerMaxHealth;
            }

            Changehealthbar(playerHealth);
        }

        public void BuyBread()
        {
            HasBread = true;
        }

        //private void ThrowItem(GameObject obj)
        //{
        //    if (obj.GetComponent<MainWeapon>())
        //    {
        //        obj.GetComponent<MainWeapon>().Drop();
        //        PlayerController.inst.mainWeapon = null;
        //    }
        //    else
        //    {
        //        obj.transform.parent = null;
        //        obj.GetComponent<Rigidbody>().useGravity = true;
        //    }
        //}

        private void Changehealthbar(float playerhealth)
        {
            // Convert player health into healthbar sprite range to select from.
            float healthInRange  = playerHealth *  ((float)healthimages.Length-1) / playerMaxHealth;
            healthbar.sprite = healthimages[Mathf.RoundToInt(healthInRange)];
        }

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