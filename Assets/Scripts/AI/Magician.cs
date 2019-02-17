using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public class Magician : MonoBehaviour
{
    private bool triggered = false;
    private bool open = false;
    public WeaponUpgrade weaponUpgrade;

    // Canvas objects.
    [Header("Canvas")]
    public GameObject canvasCallout;
    public GameObject canvasPanel;
    public GameObject canvasButton0;
    public GameObject canvasButton1;
    public GameObject canvasButton2;
    public GameObject canvasButton3;

    // Key prompt.
    public GameObject keyPrompt;

    // Types of upgrades available to the weapons.
    private WeaponUpgrade.UpgradeType chosenTypeUpgrade = WeaponUpgrade.UpgradeType.none;

    // Current state of the dialogue.
    private enum DialogueState { callout, greeting, denial, upgradeChoice, fireUp, iceUp, confirm };
    private DialogueState ds = DialogueState.callout;

    // Dialogue class keeps npc text and player answers on track.
    public class Dialogue
    {
        public string dialogue;
        public string option0;
        public string option1;
        public string option2;
        public string option3;

        public Dialogue(string dialogue, string option1, string option2 = null, string option3 = null, string option4 = null)
        {
            this.dialogue = dialogue;
            this.option0 = option1 ?? null;
            this.option1 = option2 ?? null;
            this.option2 = option3 ?? null;
            this.option3 = option4 ?? null;
        }
    }

    // Magician dialogue.
    private readonly Dialogue greeting = new Dialogue("I can upgrade your weapon if you wish.All I ask for in return is a loaf of bread...", "Sure thing", "Bugger off");
    private readonly Dialogue greetingAlt = new Dialogue("You again... Wanna change your magiks?", "Please.", "Nah");
    private readonly Dialogue upgradeChoice= new Dialogue("Excellent. What kind of upgrade would you like?", "Fire!", "Mystic Aura", "Bye!");
    private readonly Dialogue fireUp = new Dialogue("Fire will schorch your enemies over time, mind your clothes though. Are you sure this is what you want?", "Aye!", "Actually...", "Bye!");
    private readonly Dialogue iceUp = new Dialogue("This will empower you weapon with the gods! Are you sure this is what you want?", "Ayeyeyeye!", "Ac-tually...", "Bye!");
    private readonly Dialogue upgradeChoiceAlt = new Dialogue("Once again. What kind of upgrade would you like?", "Fire!", "My stick arrgh", "Bye!");

    private void Awake()
    {
        // Close everything in case forgotten to do in editor.
        Close();
    }

    public void Button(int option)
    {
        switch (ds)
        {
            case DialogueState.callout:
                break;
            case DialogueState.greeting:
                if (option == 0) { Sure(); }
                else if (option == 1) { MaybeLater(); }
                break;
            case DialogueState.denial:
                if (option == 0) { Sure(); }
                else if (option == 1) { Close(); }
                break;
            case DialogueState.upgradeChoice:
                if (option == 0) { chosenTypeUpgrade = WeaponUpgrade.UpgradeType.fire; UpdateDialogueBox(fireUp); }
                else if (option == 1) { chosenTypeUpgrade = WeaponUpgrade.UpgradeType.ice; UpdateDialogueBox(iceUp); }
                else if (option == 2) { Close(); break; }
                ds = DialogueState.confirm;
                break;
            case DialogueState.confirm:
                if (option == 0) { Confirm(); }
                else if (option == 1) { Actually(); }
                else if (option == 2) { Close(); }
                break;
            default:
                break;
        }
    }

    private void Close()
    {
        open = false;
        PlayerController.inst.CanMove = true;
        //PlayerController.inst.CanBeAttacked = true;
        canvasPanel.SetActive(false);
        canvasCallout.SetActive(false);
        keyPrompt.SetActive(false);
    }

    private void Sure()
    {
        // If player accepts and has bread that npc requested.
        if (true || HealthManager.inst.HasBread)
        {
            UpdateDialogueBox(upgradeChoice);
            ds = DialogueState.upgradeChoice;
        }
        //Debug
        //PlayerController.inst.mainWeapon.Upgrade(MainWeapon.UpgradeType.fire, vfx_fire_attack, vfx_fire_upgrade);
    }

    private void MaybeLater()
    {
        Close();
    }

    public void Actually()
    {
        UpdateDialogueBox(upgradeChoiceAlt);
        ds = DialogueState.upgradeChoice;
    }


    private void Confirm()
    {
        PlayerController.inst.mainWeapon.Upgrade(weaponUpgrade, chosenTypeUpgrade);
        Close();

    }

    private void Update()
    {
        if (triggered)
        {
            // Open dialogue.
            if (!open && (Input.GetKeyDown(KeyCode.E) || (Input.GetButtonDown("Square"))))
            {
                open = true;

                // Disable callout and floating icon.
                canvasCallout.SetActive(false);
                keyPrompt.SetActive(false);

                // Open dialogue panel and update text.
                ds = DialogueState.greeting;
                canvasPanel.SetActive(true);
                UpdateDialogueBox(greeting);
                // Prevent player movement and handover control to UI.
                PlayerController.inst.CanMove = false;
                //PlayerController.inst.CanBeAttacked = false;

                // Set button focus.
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(canvasButton0);
            }

            if (open && Input.GetKeyDown(KeyCode.Q))
            {
                Close();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            canvasCallout.SetActive(true);
            triggered = true;
            keyPrompt.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //if (other.tag == "Player")
        //{
        //    // Activate callout and key prompt.
        //    if (!open && Vector3.Distance(this.transform.position, other.transform.position) <= 2f)
        //    {
        //        triggered = true;
        //        icon.SetActive(true);
        //    }
        //    else
        //    {
        //        triggered = true;
        //        icon.SetActive(true);
        //    }
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Close();
            open = false;
            triggered = false;
            canvasCallout.SetActive(false);
            canvasPanel.SetActive(false);
            keyPrompt.SetActive(false);
        }
    }

    private void UpdateDialogueBox(Dialogue dialogue)
    {
        canvasPanel.GetComponentInChildren<Text>().text = dialogue.dialogue;
        canvasButton0.GetComponentInChildren<Text>().text = dialogue.option0;

        if (dialogue.option1 == null) { canvasButton1.SetActive(false); }
        else
        {
            canvasButton1.GetComponentInChildren<Text>().text = dialogue.option1;
            canvasButton1.SetActive(true);
        }
        if (dialogue.option2 == null) { canvasButton2.SetActive(false); }
        else
        {
            canvasButton2.SetActive(true);
            canvasButton2.GetComponentInChildren<Text>().text = dialogue.option2;
        }
        if (dialogue.option3 == null) { canvasButton3.SetActive(false); }
        else
        {
            canvasButton3.SetActive(true);
            canvasButton3.GetComponentInChildren<Text>().text = dialogue.option3;
        }
    }

}
