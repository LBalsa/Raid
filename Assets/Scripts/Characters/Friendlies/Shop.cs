using Characters.Player;
using UnityEngine;

namespace Characters.Friendlies
{
    [RequireComponent(typeof(AudioSource))]
    [SelectionBase]
    public class Shop : Character
    {
        protected override string Callout { get => "Fresh fish! Fresh fish! Who wants to buy some fresh fish! Straight outta de'sea."; }

        [Header("Shop SFX")]
        public AudioClip success;
        public AudioClip fail;
        public GameObject shopCanvas;
        public InventoryManager client;
        public override void StartInteraction(GameObject other)
        {
            if (!isBusy)
            {
                client = other?.GetComponent<InventoryManager>();
                if (client != null)
                {
                    isBusy = true;
                    shopCanvas.SetActive(true);
                    //Time.timeScale = 0f;
                    Cursor.lockState = CursorLockMode.None;

                    PlaySFX(success);
                }
            }
        }

        public override void EndInteraction()
        {
            if (isBusy)
            {
                RaiseEndInteraction();
                client = null;
                isBusy = false;
                shopCanvas.SetActive(false);
                //Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;

                PlaySFX(success);
            }
        }

        // Check if player has enough money.
        private bool CollectMoney(int gold)
        {
            if (client.Money >= gold)
            {
                // Update money;
                client.Money -= gold;

                // Play coin sfx.
                PlaySFX(success);
                return true;
            }
            else
            {
                // Play unsuccessful sfx.
                PlaySFX(fail);
                return false;
            }
        }

        public void BuyFish()
        {
            if (CollectMoney(1))
            {
                client.InstaHealth(1);
            }
        }

        public void BuyBread()
        {
            if (CollectMoney(1))
            {
                //HealthManager.inst.BuyBread();
            }
        }

        public void BuyHeart()
        {
            if (CollectMoney(5))
            {
                client.HealthUpgrade(2);
            }
        }

        public void Continue()
        {
            shopCanvas.SetActive(false);
            Time.timeScale = 1.0f;
        }

        private void PlaySFX(AudioClip clip)
        {
            if (clip)
            {
                aus.clip = clip;
                aus.Play();
            }
        }
    }
}