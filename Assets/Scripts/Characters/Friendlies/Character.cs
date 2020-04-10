using UnityEngine;
using UnityEngine.AI;
using UserInterfaces.Dialogues;
using Utility;

namespace Characters
{
    #region RequireComponent
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(LookAt))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    #endregion

    [SelectionBase]
    public class Character : MonoBehaviour
    {
        public enum CharacterFaction { Ally, Neutral, Enemy }
        protected CharacterFaction characterFaction = CharacterFaction.Neutral;
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

        protected bool isBusy;
        protected bool isTriggered = false;

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

        protected virtual void OpenCanvas(bool? withCalloutOrDialogue)
        {
            if (dialogueCanvas == null && canvasPrefab != null)
            {
                var dialogueCanvasPos = transform.position;
                dialogueCanvasPos.y += 2.2f;

                dialogueCanvas = Instantiate(canvasPrefab, dialogueCanvasPos, Quaternion.identity, this.transform).GetComponent<IDialogueCanvas>();
                dialogueCanvas.Dialogue = dialogue;
            }

            if (withCalloutOrDialogue == true) { dialogueCanvas.ToggleDialogue(true); }
            else if (withCalloutOrDialogue == false) { dialogueCanvas.ToggleCallout(true); }
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
                OpenCanvas(false);
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
