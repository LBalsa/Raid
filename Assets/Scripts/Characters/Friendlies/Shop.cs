using UnityEngine;

namespace Characters.Friendlies
{
    [RequireComponent(typeof(AudioSource))]
    [SelectionBase]
    public class Shop : Character
    {
        [Header("Shop SFX")]
        public AudioClip success;
        public AudioClip fail;
        public GameObject shopCanvas;

        private string shopCallout = "Fresh fish! Fresh fish! Who wants to buy some fresh fish! Straight outta de'sea.";

        // Update is called once per frame
        private void Update()
        {
            if (isTriggered)
            {
                if (!isBusy && Input.GetKeyDown(KeyCode.E) || (Input.GetButton("Square")))
                {
                    OpenShop();
                }
            }
        }

        protected void OpenShop()
        {
            Cursor.lockState = CursorLockMode.None;
            isBusy = true;
            shopCanvas.SetActive(true);
            //UnityEngine.EventSystems.EventSystem.current.firstSelectedGameObject = shop.gameObject;
            CheckGold(0);
            Time.timeScale = 0f;
        }

        public void CloseShop()
        {
            Cursor.lockState = CursorLockMode.Locked;
            isBusy = false;
            shopCanvas.SetActive(false);

            Time.timeScale = 1f;
        }

        // Check if player has enough money.
        private bool CheckGold(int gold)
        {
            if (HealthManager.inst.Money >= gold)
            {
                // Update money;
                HealthManager.inst.Money -= gold;
                HealthManager.inst.gold.text = HealthManager.inst.Money.ToString();

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
            if (CheckGold(1))
            {
                HealthManager.inst.Heal(1);
            }
        }

        public void BuyBread()
        {
            if (CheckGold(1))
            {
                HealthManager.inst.BuyBread();
            }
        }

        public void BuyHeart()
        {
            if (CheckGold(5))
            {
                HealthManager.inst.HealthUpgrade();
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