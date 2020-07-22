using Controllers;
using UnityEngine;
using Weapons;

namespace Characters.Player
{
    public class InteractionManager : MonoBehaviour
    {
        private IInteractable availableInteractable;
        private PlayerController playerController;
        private bool isInteracting;

        public event BlockInteraction OnBlockingInteraction;
        public delegate void BlockInteraction();
        public event FreeInteraction OnFreeInteraction;
        public delegate void FreeInteraction();
        public event PickupWeaon OnPickupWeapon;
        public delegate void PickupWeaon(MainWeapon mainWeapon);

        public GameObject weaponPickupTooltip;

        private void Start()
        {
            playerController = GetComponent<PlayerController>();
            GetComponent<HealthManager>().OnDeath += delegate { enabled = false; };
        }

        private void Update()
        {
            if (availableInteractable != null)
            {
                if (Input.GetKeyDown(KeyCode.E) && !isInteracting)
                {
                    StartInteraction();
                }
                else if (Input.GetKeyDown(KeyCode.Q) && isInteracting)
                {
                    EndInteraction();
                }
            }
        }

        private void StartInteraction()
        {
            availableInteractable.StartInteraction(this.gameObject);
            availableInteractable.OnEndInteraction += EndInteraction;
            isInteracting = true;
            OnBlockingInteraction.Invoke();
        }

        private void EndInteraction()
        {
            availableInteractable.OnEndInteraction -= EndInteraction;
            availableInteractable.EndInteraction();
            isInteracting = false;
            OnFreeInteraction?.Invoke();
        }


        private void OnTriggerEnter(Collider other)
        {
            var character = other.gameObject.GetComponent<Character>();
            if (character && (character.characterFaction == CharacterFaction.Ally || character.characterFaction == CharacterFaction.Neutral))
            {
                availableInteractable = other.GetComponent<IInteractable>();
                //OpenCanvas(true, Callout);
                //isTriggered = true;
                //ToggleKeyPrompt(true);
            }
            else if (other.GetComponent<IAnimal>() != null)
            {
                other.GetComponent<IAnimal>().Aproach(gameObject);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (GameController.inst.Paused || isInteracting) { return; }

            if (other.GetComponent<MainWeapon>())
            {
                if (!isInteracting && (Input.GetKeyDown(KeyCode.R) || (Input.GetButton("Circle"))))
                {
                    OnPickupWeapon?.Invoke(other.GetComponent<MainWeapon>());
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var interactable = other.GetComponent<IInteractable>();
            if (interactable == availableInteractable)
            {
                availableInteractable = null;
            }
        }
    }
}