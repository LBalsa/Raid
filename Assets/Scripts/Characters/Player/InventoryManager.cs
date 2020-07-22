using SpecialEffects.Structures;
using UnityEngine;
using UnityEngine.UI;

namespace Characters.Player
{
    public class InventoryManager : MonoBehaviour
    {
        private int money;
        public int Money { get => money; set { money = value; MoneyUI.text = value.ToString(); } }
        public Text MoneyUI;
        private PlayerFXStructure fx;
        private AudioSource audioSource;

        private void Start()
        {
            GetComponent<HealthManager>().OnDeath += delegate { enabled = false; };
            audioSource = GetComponent<AudioSource>();
            MoneyUI.text = "0";

            fx = PlayerController.inst.fx;
        }

        public void InstaHealth(int health) => GetComponent<HealthManager>().Heal(health);
        public void HealthUpgrade(int health) => GetComponent<HealthManager>().HealthUpgrade(health);

        private void OnTriggerEnter(Collider other)
        {
            // Money is added to...money.
            if (other.gameObject.tag == "Silver")
            {
                fx.Play(audioSource, fx.sfx_coin);
                Money++;
                Destroy(other.gameObject);
            }
            else if (other.gameObject.tag == "Gold")
            {
                fx.Play(audioSource, fx.sfx_coin);
                Money += 5;
                Destroy(other.gameObject);
            }
        }
    }
}
