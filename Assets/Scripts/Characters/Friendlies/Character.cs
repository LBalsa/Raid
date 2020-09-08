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
    public class Character : MonoBehaviour, IDialogueAction, IInteractable, IDestructable, ICharacter
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

        public event EndInteraction OnEndInteraction;
        protected virtual void RaiseEndInteraction() => OnEndInteraction?.Invoke();

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

        public virtual void StartInteraction(GameObject other)
        {
            if (!isBusy && dialogue != null)
            {
                isBusy = true;
                ToggleKeyPrompt(false);
                dialogue.StartDialogue(dialogueCanvas, this);
            }
        }

        public virtual void EndInteraction()
        {
            if (isBusy)
            {
                RaiseEndInteraction();
                CloseCanvas();
            }
        }

        public virtual void PerformAction(int actionIndex)
        {
            Debug.Log("Perform action " + actionIndex);
            switch (actionIndex)
            {
                default:
                    EndInteraction();
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

        public virtual void TakeDamage(float damage)
        {
            throw new System.NotImplementedException();
        }

        public virtual void TakeDamage(float damage, Collision collision)
        {
            throw new System.NotImplementedException();
        }
    }
}
