using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Magician : MonoBehaviour
{
    [Header("Canvas")]
    public GameObject callOut;
    public GameObject canvasDialogue;
    public GameObject canvasOption1;
    public GameObject canvasOption2;

    public GameObject canvas1question;
    public GameObject canvas2upgrade;
    public GameObject canvas3fire;
    public GameObject canvas4freeze;
    public GameObject icon;

    bool questionAnswered = false;
    bool upgradeChosen = false;
    bool fireChosen = false;
    bool freezeChosen = false;

    bool fireAccepted = false;
    bool freezeAccepted = false;

    public bool hasBread = true;

    private bool triggered = false;
    private bool open = false;
    public float degreesPerSecond = -75.0f;

    class Dialogue
    {
        public string dialogue;
        public string option1;
        public string option2;

        public Dialogue(string dialogue, string option1, string option2)
        {
            this.dialogue = dialogue;
            this.option1 = option1;
            this.option2 = option2;
        }
    }

    // Magician dialogue.
    Dialogue dialogue1 = new Dialogue("I can upgrade your weapon if you wish.All I ask for in return is a loaf of bread...", "Sure thing", "Bugger off");
    Dialogue dialogue2 = new Dialogue("Excellent.What kind of upgrade would you like?", "Fire!", "Freee-zzz");
    Dialogue dialogue3 = new Dialogue("Fire will schorch your enemies over time, mind your clothes though. Are you sure this is what you want?", "Aye!", "Actually...");
    Dialogue dialogue4 = new Dialogue("This will freeza enemies half to death. Are you sure this is what you want?", "Ayeyeyeye!", "Ac-tually...");

    [Header("Upgrade VFX")]
    public GameObject vfx_fire_attack;
    public GameObject vfx_fire_upgrade;
    public GameObject vfx_ice_attack;
    public GameObject vfx_ice_upgrade;

    void Start()
    {
        callOut.SetActive(true);
        canvas1question.SetActive(false);
        canvas2upgrade.SetActive(false);
        canvas3fire.SetActive(false);
        canvas4freeze.SetActive(false);
        icon.SetActive(false);
    }

    public void Sure()
    {
        // If player accepts and has bread that npc requested.
        if (HealthManager.inst.hasBread)
        {
            questionAnswered = true;
            UpdateDialogueBox(dialogue1);


            //canvas1question.SetActive(false);
            //canvas2upgrade.SetActive(true);

            callOut.SetActive(false);
            icon.SetActive(false);
        }

        //Debug
        //PlayerController.inst.mainWeapon.Upgrade(MainWeapon.UpgradeType.fire, vfx_fire_attack, vfx_fire_upgrade);

    }

    public void MaybeLater()
    {
        canvas1question.SetActive(false);
        callOut.SetActive(false);
        icon.SetActive(false);
    }

    public void Fire()
    {
        fireChosen = true;
        canvas2upgrade.SetActive(false);
        canvas3fire.SetActive(true);
        callOut.SetActive(false);
        icon.SetActive(false);

        PlayerController.inst.mainWeapon.Upgrade(MainWeapon.UpgradeType.fire, vfx_fire_attack, vfx_fire_upgrade);
    }

    public void Freeze()
    {
        freezeChosen = true;
        canvas2upgrade.SetActive(false);
        canvas4freeze.SetActive(true);
        callOut.SetActive(false);
        icon.SetActive(false);

        PlayerController.inst.mainWeapon.Upgrade(MainWeapon.UpgradeType.fire, vfx_ice_attack, vfx_ice_upgrade);
    }

    public void Actually()
    {
        fireChosen = false;
        freezeChosen = false;
        canvas3fire.SetActive(false);
        canvas4freeze.SetActive(false);
        canvas2upgrade.SetActive(true);
        callOut.SetActive(false);
        icon.SetActive(false);
    }

    public void FireConfirm()
    {
        fireAccepted = true;
        canvas3fire.SetActive(false);
        callOut.SetActive(false);
        icon.SetActive(false);
    }

    public void FreezeConfirm()
    {
        freezeAccepted = true;
        canvas4freeze.SetActive(false);
        callOut.SetActive(false);
        icon.SetActive(false);
    }

    public void CloseInteraction()
    {
        open = false;
        icon.SetActive(true);
    }

    void Update()
    {
        // Make button float above NPC when player is close.
        if (triggered)
        {
            if (!open)
            {
                icon.transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);
            }
            else if (open) CloseInteraction();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            open = false;
            triggered = true;


            icon.SetActive(true);

            // Open Request
            if (questionAnswered == false && upgradeChosen == false && fireAccepted == false && freezeAccepted == false && (Input.GetKeyDown(KeyCode.E) || (Input.GetButtonDown("Square"))))
            {

                print("1");
                //Sure();

                open = true;

                // Open dialogue box and update text.
                canvas1question.SetActive(true);
                UpdateDialogueBox(dialogue1);
                // Set button focus.
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(canvasOption1);


                // Disable callout and floating icon.
                callOut.SetActive(false);
                icon.SetActive(false);
            }

            // Make Choise
            else if (questionAnswered == true && upgradeChosen == false && fireAccepted == false && freezeAccepted == false && (Input.GetKeyDown(KeyCode.E) || (Input.GetButtonDown("Square"))))
            {
                print("2");
                Fire();
                open = true;
                canvas1question.SetActive(false);
                canvas2upgrade.SetActive(true);
                canvas3fire.SetActive(false);
                canvas4freeze.SetActive(false);
                callOut.SetActive(false);
                icon.SetActive(false);
            }

            // Choose Fire
            else if (questionAnswered == true && upgradeChosen == true && fireChosen == true && fireAccepted == false && freezeAccepted == false && (Input.GetKeyDown(KeyCode.E) || (Input.GetButtonDown("Square"))))
            {
                print("3");
                FireConfirm();
                open = true;
                canvas1question.SetActive(false);
                canvas2upgrade.SetActive(false);
                canvas3fire.SetActive(true);
                canvas4freeze.SetActive(false);
                callOut.SetActive(false);
                icon.SetActive(false);
            }

            // Chose Ice
            else if (questionAnswered == true && upgradeChosen == true && freezeChosen == true && fireAccepted == false && freezeAccepted == false && (Input.GetKeyDown(KeyCode.E) || (Input.GetButtonDown("Square"))))
            {
                print("4");
                open = true;
                canvas1question.SetActive(false);
                canvas2upgrade.SetActive(false);
                canvas3fire.SetActive(false);
                canvas4freeze.SetActive(true);
                callOut.SetActive(false);
                icon.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        open = false;
        triggered = false;
        canvas1question.SetActive(false);
        canvas2upgrade.SetActive(false);
        canvas3fire.SetActive(false);
        canvas4freeze.SetActive(false);
        callOut.SetActive(true);
        icon.SetActive(false);
    }

    private void UpdateDialogueBox(Dialogue dialogue)
    {
        canvasDialogue.GetComponent<Text>().text = dialogue.dialogue;
        canvasOption1.GetComponent<Text>().text = dialogue.option1;
        canvasOption1.GetComponent<Text>().text = dialogue.option2;
    }

}
