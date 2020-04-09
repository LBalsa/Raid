using UnityEngine;
using UserInterfaces.Dialogues;
using Weapons.WeaponUpgrades;

namespace Characters.Friendlies
{
    [SelectionBase]
    public class Magician : Character, IDialogueAction
    {
        private bool triggered = false;
        private bool open = false;

        // Dialogue and canvas
        public Dialogue dialogue;
        public GameObject dialogueCanvasPrefab;
        public IDialogueCanvas dialogueCanvas;
        // Key prompt.
        public GameObject keyPrompt;

        private int weaponUpgradeIndex;
        public WeaponUpgradeGroup weaponUpgradeGroup;

        // Magician dialogue.
        private readonly DialogueEntry greeting = new DialogueEntry("I can upgrade your weapon if you wish.All I ask for in return is a loaf of bread...", "Sure thing", "Bugger off");
        private readonly DialogueEntry greetingAlt = new DialogueEntry("You again... Wanna change your magiks?", "Please.", "Nah");
        private readonly DialogueEntry upgradeChoice= new DialogueEntry("Excellent. What kind of upgrade would you like?", "Fire!", "Mystic Aura", "Bye!");
        private readonly DialogueEntry fireUp = new DialogueEntry("Fire will schorch your enemies over time, mind your clothes though. Are you sure this is what you want?", "Aye!", "Actually...", "Bye!");
        private readonly DialogueEntry iceUp = new DialogueEntry("This will empower you weapon with the gods! Are you sure this is what you want?", "Ayeyeyeye!", "Ac-tually...", "Bye!");
        private readonly DialogueEntry upgradeChoiceAlt = new DialogueEntry("Once again. What kind of upgrade would you like?", "Fire!", "My stick arrgh", "Bye!");

        private void Update()
        {
            if (triggered)
            {
                // Open dialogue.
                if (!open && (Input.GetKeyDown(KeyCode.E) || (Input.GetButtonDown("Square"))))
                {
                    open = true;

                    // Disable callout and floating icon.
                    keyPrompt.SetActive(false);
                    dialogueCanvas.ToggleCallout(false);
                    dialogueCanvas.ToggleDialogue(true);
                    dialogue.StartDialogue(dialogueCanvas, this);

                    // Prevent player movement and handover control to UI.
                    PlayerController.inst.CanMove = false;
                    //PlayerController.inst.CanBeAttacked = false;
                }

                if (open && Input.GetKeyDown(KeyCode.Q))
                {
                    Close();
                }
            }
        }

        private void OpenCanvas()
        {
            if (dialogueCanvas == null)
            {
                var dialogueCanvasPos = transform.position;
                dialogueCanvasPos.y += 2.2f;

                dialogueCanvas = Instantiate(dialogueCanvasPrefab, dialogueCanvasPos, Quaternion.identity, this.transform).GetComponent<IDialogueCanvas>();
                dialogueCanvas.Dialogue = dialogue;
            }
        }

        public void PerformAction(int actionIndex)
        {
            switch (actionIndex)
            {
                case 1: weaponUpgradeIndex = actionIndex - 1; break;
                case 2: weaponUpgradeIndex = actionIndex - 1; break;
                case 3: break;
                case 10: PlayerController.inst.mainWeapon.Upgrade(weaponUpgradeGroup.Upgrades[weaponUpgradeIndex]); break;
                default:
                    Close();
                    break;
            }
        }

        private void Close()
        {
            open = false;
            PlayerController.inst.CanMove = true;
            keyPrompt.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                OpenCanvas();
                dialogueCanvas.ToggleCallout(true);
                triggered = true;
                keyPrompt.SetActive(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                Close();
                open = false;
                triggered = false;
                dialogueCanvas.ToggleCallout(false);
                keyPrompt.SetActive(false);
            }
        }
    }
}