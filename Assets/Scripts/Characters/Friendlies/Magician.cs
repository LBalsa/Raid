using UnityEngine;
using UserInterfaces.Dialogues;
using Weapons.WeaponUpgrades;

namespace Characters.Friendlies
{
    public class Magician : Character, IDialogueAction
    {
        [Header("Weapon Upgrades")]
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
            if (isTriggered)
            {
                // Open dialogue.
                if (!isBusy && (Input.GetKeyDown(KeyCode.E) || (Input.GetButtonDown("Square"))))
                {
                    StartInteraction();
                }

                if (isBusy && Input.GetKeyDown(KeyCode.Q))
                {
                    CloseCanvas();
                }
            }
        }

        void StartInteraction()
        {
            isBusy = true;

            // Disable callout and floating icon.
            keyPrompt.SetActive(false);
            dialogueCanvas.ToggleCallout(false);
            dialogueCanvas.ToggleDialogue(true);
            dialogue.StartDialogue(dialogueCanvas, this);

            // Prevent player movement and handover control to UI.
            PlayerController.inst.CanMove = false;
            //PlayerController.inst.CanBeAttacked = false;
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
                    CloseCanvas();
                    break;
            }
        }
    }
}