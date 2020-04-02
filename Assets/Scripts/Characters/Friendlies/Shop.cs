using UnityEngine;

namespace Characters.Friendlies
{
    [RequireComponent(typeof(AudioSource))]
    [SelectionBase]
    public class Shop : MonoBehaviour
    {
        public static Shop inst = null;

        private bool trigger = false;
        private bool open = false;
        public float degreesPerSecond = -75.0f;
        public float amplitude = 0.1f;
        public float frequency = 1f;
        private Vector3 posOffset = new Vector3();
        private Vector3 tempPos = new Vector3();

        public GameObject icon;
        public GameObject shop;
        [Header("Shop SFX")]
        private    AudioSource aus = null;
        public AudioClip success;
        public AudioClip fail;

        // Use this for initialization
        private void Start()
        {
            inst = this;
            aus = GetComponent<AudioSource>();
            icon.SetActive(false);
            shop.SetActive(false);
            posOffset = icon.transform.position;
        }

        // Update is called once per frame
        private void Update()
        {
            if (trigger)
            {
                if (!open)
                {
                    icon.transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);
                    tempPos = posOffset;
                    tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

                    icon.transform.position = tempPos;
                }

                if (Input.GetKeyDown(KeyCode.E) || (Input.GetButton("Square")))
                {
                    print(open);
                    if (open) { }
                    //CloseShop();
                    else if (!open)
                    {
                        OpenShop();
                    }
                }
            }
        }

        protected void OpenShop()
        {
            Cursor.lockState = CursorLockMode.None;
            open = true;
            icon.SetActive(false);
            shop.SetActive(true);
            //UnityEngine.EventSystems.EventSystem.current.firstSelectedGameObject = shop.gameObject;
            CheckGold(0);
            Time.timeScale = 0f;
        }

        public void CloseShop()
        {
            Cursor.lockState = CursorLockMode.Locked;
            open = false;
            icon.SetActive(true);
            shop.SetActive(false);

            Time.timeScale = 1f;
        }

        protected void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                icon.SetActive(true);
                trigger = true;
            }
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

        protected void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                icon.SetActive(false);
                trigger = false;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.tag == "Player" && (Input.GetKeyDown(KeyCode.E) || (Input.GetButton("Square"))))
            {
                //Time.timeScale = 0.0f;
                //shop.SetActive(true);
            }
        }

        public void Continue()
        {
            shop.SetActive(false);
            icon.SetActive(true);
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