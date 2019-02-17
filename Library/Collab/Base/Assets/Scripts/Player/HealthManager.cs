using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour, IDestructable
{
    public static HealthManager inst;
    public float playerHealth;
    public float playerMaxHealth;

    public float hitforce;
    public float timeToRecover = 1f;
    float counter;
    float holdingTimer = 0;
    public bool hasBread;
    public Transform rightHandHolder;
    public bool holding;
    [HideInInspector]
    public int money = 0;
    int food;
    public Sprite[] healthimages;
    //public GameObject circleButton;
    public Image healthbar;
    public Text gold;
    Animator anim;

    private void Awake()
    {
        inst = this;
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        holding = true;
        counter = 10;
        gold.text = "0";
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<MainWeapon>())
        {
            if ((!holding) && (Input.GetKeyDown(KeyCode.R) || (Input.GetButton("Circle"))))
            {
                holding = true;
                holdingTimer = 1f;
                other.GetComponent<MainWeapon>().Pickup(rightHandHolder, true, 2);
                PlayerController.inst.mainWeapon = other.GetComponent<MainWeapon>();
                anim.speed = PlayerController.inst.mainWeapon.speed;
                //circleButton.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // circleButton.SetActive(false);
    }

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

    void Holding()
    {
        holding = true;

    }


    void Update()
    {

        if (holdingTimer >= 0)
        {
            holdingTimer -= Time.deltaTime;
        }
        if (holding && holdingTimer < 0)
        {
            if (Input.GetKeyDown(KeyCode.R) || (Input.GetButton("Circle")))
            {
                holding = false;
                anim.speed = 1;
                ThrowItem(rightHandHolder.GetChild(0).gameObject);
            }
        }


        Changehealthbar(playerHealth);
    }

    public void TakeDamage(float damage)
    {
        if (playerHealth > 0)
        {
            playerHealth -= damage;

        }
        else if (playerHealth <= 0)
        {
            Death();
            Debug.Log("DEATH");
            playerHealth = 0;
        }

    }

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
            Debug.Log("DEATH");
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

    void ThrowItem(GameObject obj)
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

    void Changehealthbar(float playerhealth)
    {
        healthbar.sprite = healthimages[Mathf.RoundToInt(playerhealth)];
    }

    void Death()
    {
        anim.SetTrigger("Dead");
        Invoke("ResetTriggers", 0.2f);
    }

    void TakeDamage()
    {
        anim.SetTrigger("hit");
        Invoke("ResetTriggers", 0.2f);
    }

    void ResetTriggers()
    {
        anim.ResetTrigger("Dead");
        anim.ResetTrigger("hit");

    }
}
