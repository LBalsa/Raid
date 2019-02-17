using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour, IDestructable
{
    public static HealthManager inst;

    public bool isAlive = true;
    public float playerHealth;
    public float playerMaxHealth;

    public float hitforce;
    public float timeToRecover = 1f;
    private float counter;
    private float holdingTimer = 0;
    public bool hasBread;

    public Transform rightHandHolder;
    public bool hasWeapon;
    [HideInInspector]
    public int money = 0;
    public Sprite[] healthimages;
    //public GameObject circleButton;
    public Image healthbar;
    public Text gold;
    private Animator anim;

    private void Awake()
    {
        inst = this;
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        hasWeapon = true;
        counter = 10;
        gold.text = "0";
    }

    private void OnTriggerStay(Collider other)
    {
        // Allow player to pick up a weapon if he collides 
        // with one and does not have one.
        if (other.GetComponent<MainWeapon>())
        {
            if ((!hasWeapon) && (Input.GetKeyDown(KeyCode.R) || (Input.GetButton("Circle"))))
            {
                // Prevent player from picking up more weapons.
                hasWeapon = true;
                holdingTimer = 1f;

                // Pick up weapon and assign it as the weapon.
                other.GetComponent<MainWeapon>().Pickup(rightHandHolder, true, 2);
                PlayerController.inst.mainWeapon = other.GetComponent<MainWeapon>();

                // Asjust animation speed to weapon weight.
                anim.speed = PlayerController.inst.mainWeapon.speed;

                //circleButton.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // circleButton.SetActive(false);
    }

    // Pick up objects.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Silver")
        {
            money++;
            gold.text = money.ToString();
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Gold")
        {
            money += 5;
            gold.text = money.ToString();
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Food" && playerHealth < playerMaxHealth)
        {
            Heal(1);
            Destroy(other.gameObject);
        }
        if (other.GetComponent<MainWeapon>())
        {
            //circleButton.SetActive(true);
        }
    }

    private void Holding()
    {
        hasWeapon = true;
    }

    private void Update()
    {
        // Handles picking up and dropping of weapons.
        WeaponHandler();

        // Updates health bar.
        Changehealthbar(playerHealth);
    }

    private void WeaponHandler()
    {
        // Count down weapon drop cooldown, to prevent instant pickup and drop of weapon.
        if (holdingTimer >= 0) { holdingTimer -= Time.deltaTime; }

        // Drop current weapon.
        if (hasWeapon && holdingTimer < 0)
        {
            if (Input.GetKeyDown(KeyCode.R) || (Input.GetButton("Circle")))
            {
                hasWeapon = false; // Enable player to pick up another weapon.
                anim.speed = 1; // Reset animator speed.
                ThrowItem(rightHandHolder.GetChild(0).gameObject);
            }
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
    public void TakeDamage(float damage, Collision Collider)
    {
        if (playerHealth <= 0)
        {
            return;
        }

        playerHealth -= damage;

        if (playerHealth > 0)
        {
            TakeDamage();
        }
        else if (playerHealth <= 0)
        {
            Death();
            playerHealth = 0;

        }
    }

    public void HealthUpgrade()
    {
        playerMaxHealth += 2;
        playerHealth = playerMaxHealth;
    }

    public void Heal(int healAmout)
    {
        playerHealth += healAmout;

        if (playerHealth > playerMaxHealth)
        {
            playerHealth = playerMaxHealth;
        }
    }

    public void BuyBread()
    {
        hasBread = true;
    }

    private void ThrowItem(GameObject obj)
    {
        if (obj.GetComponent<MainWeapon>())
        {
            obj.GetComponent<MainWeapon>().Drop();
            PlayerController.inst.mainWeapon = null;
        }
        else
        {
            obj.transform.parent = null;
            obj.GetComponent<Rigidbody>().useGravity = true;
        }
    }

    private void Changehealthbar(float playerhealth)
    {
        healthbar.sprite = healthimages[Mathf.RoundToInt(playerhealth)];
    }

    private void Death()
    {
        isAlive = false;
        anim.SetTrigger("Dead");
        Invoke("ResetTriggers", 0.2f);
    }

    private void TakeDamage()
    {
        anim.SetTrigger("hit");
        Invoke("ResetTriggers", 0.2f);
    }

    private void ResetTriggers()
    {
        anim.ResetTrigger("Dead");
        anim.ResetTrigger("hit");

    }
}
