using Characters.Player;
using UnityEngine;
using UnityEngine.AI;
using UserInterfaces.Dialoguing;
using Utility;

namespace Characters
{
    public enum CharacterFaction { Ally, Neutral, Enemy }
    #region RequireComponent
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(LookAt))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    #endregion

    [SelectionBase]
    public class Character : MonoBehaviour, IDialogueAction
    {
        public CharacterFaction characterFaction = CharacterFaction.Neutral;
        [SerializeField]
        protected CharacterStats stats;

        #region Components
        protected Animator anim;
        protected AudioSource aus;
        protected LookAt lookAt;
        protected NavMeshAgent nav;
        protected Rigidbody rb;
        #endregion

        // Dialogue and canvas
        [Header("UI Components")]
        public GameObject keyPrompt;
        public Dialogue dialogue;
        public GameObject canvasPrefab;
        protected IDialogueCanvas dialogueCanvas;
        protected virtual string Callout { get => "Hey there! I'm a random npc!"; }

        protected bool isBusy;
        protected bool isTriggered;

        protected virtual void Start()
        {
            Initialise();
        }

        protected virtual void Initialise()
        {
            anim = GetComponent<Animator>();
            aus = GetComponent<AudioSource>();
            lookAt = GetComponent<LookAt>();
            nav = GetComponent<NavMeshAgent>();
            rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (isTriggered)
            {
                // Open dialogue.
                if (!isBusy && (Input.GetKeyDown(KeyCode.E) || (Input.GetButtonDown("Square"))))
                {
                    StartInteraction();
                }
                else if (isBusy && Input.GetKeyDown(KeyCode.Q))
                {
                    EndInteraction();
                }
            }
        }

        protected virtual void StartInteraction()
        {
            if (dialogue != null)
            {
                isBusy = true;
                ToggleKeyPrompt(false);
                dialogue.StartDialogue(dialogueCanvas, this);

                // Prevent player movement and handover control to UI.
                PlayerController.inst.CanMove = false;
                //PlayerController.inst.CanBeAttacked = false;
            }
        }

        public virtual void EndInteraction()
        {
            CloseCanvas();
        }

        public virtual void PerformAction(int actionIndex)
        {
            switch (actionIndex)
            {
                default:
                    CloseCanvas();
                    break;
            }
        }

        protected virtual void OpenCanvas(bool withCallout, string callOut)
        {
            if (dialogueCanvas == null && canvasPrefab != null)
            {
                var dialogueCanvasPos = transform.position;
                dialogueCanvasPos.y += 2.2f;

                dialogueCanvas = Instantiate(canvasPrefab, dialogueCanvasPos, Quaternion.identity, this.transform).GetComponent<IDialogueCanvas>();
                dialogueCanvas.Dialogue = dialogue;
            }

            if (withCallout == true) { dialogueCanvas.ToggleCallout(true); dialogueCanvas.SetCallout(callOut); }
        }

        protected virtual void CloseCanvas()
        {
            if (dialogueCanvas != null)
            {
                dialogueCanvas.ToggleCallout(false);
                dialogueCanvas.ToggleCallout(false);
            }
            isBusy = false;
            PlayerController.inst.CanMove = true;
        }

        protected virtual void ToggleKeyPrompt(bool state)
        {
            if (keyPrompt != null) { keyPrompt.SetActive(state); }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player" && (characterFaction == CharacterFaction.Ally || characterFaction == CharacterFaction.Neutral))
            {
                OpenCanvas(true, Callout);
                isTriggered = true;
                ToggleKeyPrompt(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player" && (characterFaction == CharacterFaction.Ally || characterFaction == CharacterFaction.Neutral))
            {
                CloseCanvas();
                isTriggered = false;
                ToggleKeyPrompt(false);
            }
        }

    }
}
