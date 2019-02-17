using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magician : MonoBehaviour
{
    [Header("Canvas")]
    public GameObject callOut;
    public GameObject question;
    public GameObject upgrade;
    public GameObject fire;
    public GameObject freeze;
    public GameObject icon;

    bool questionAnswered = false;
    bool upgradeChosen = false;
    bool fireChosen = false;
    bool freezeChosen = false;
    bool fireAccepted = false;
    bool freezeAccepted = false;
    public bool hasBread = true;
    bool fireUpgrade = false;
    bool freezeUpgrade = false;
    private bool trigger = false;
    private bool open = false;
    public float degreesPerSecond = -75.0f;


    [Header("Upgrade VFX")]
    public GameObject vfx_fire_attack;
    public GameObject vfx_fire_upgrade;
    public GameObject vfx_ice_attack;
    public GameObject vfx_ice_upgrade;

    void Start()
    {
        callOut.SetActive(true);
        question.SetActive(false);
        upgrade.SetActive(false);
        fire.SetActive(false);
        freeze.SetActive(false);
        icon.SetActive(false);
    }

    public void Sure()
    {
        if (HealthManager.inst.hasBread)
        {
            questionAnswered = true;
            question.SetActive(false);
            upgrade.SetActive(true);
            callOut.SetActive(false);
            icon.SetActive(false);
        }
        PlayerController.inst.mainWeapon.Upgrade(MainWeapon.UpgradeType.fire, vfx_fire_attack, vfx_fire_upgrade);

    }

    public void MaybeLater()
    {
        question.SetActive(false);
        callOut.SetActive(false);
        icon.SetActive(false);
    }

    public void Fire()
    {
        fireChosen = true;
        upgrade.SetActive(false);
        fire.SetActive(true);
        callOut.SetActive(false);
        icon.SetActive(false);

        PlayerController.inst.mainWeapon.Upgrade(MainWeapon.UpgradeType.fire, vfx_fire_attack, vfx_fire_upgrade);
    }

    public void Freeze()
    {
        freezeChosen = true;
        upgrade.SetActive(false);
        freeze.SetActive(true);
        callOut.SetActive(false);
        icon.SetActive(false);

        PlayerController.inst.mainWeapon.Upgrade(MainWeapon.UpgradeType.fire, vfx_ice_attack, vfx_ice_upgrade);
    }

    public void Actually()
    {
        fireChosen = false;
        freezeChosen = false;
        fire.SetActive(false);
        freeze.SetActive(false);
        upgrade.SetActive(true);
        callOut.SetActive(false);
        icon.SetActive(false);
    }

    public void FireConfirm()
    {
        fireAccepted = true;
        fireUpgrade = true;
        fire.SetActive(false);
        callOut.SetActive(false);
        icon.SetActive(false);
    }

    public void FreezeConfirm()
    {
        freezeAccepted = true;
        freezeUpgrade = true;
        freeze.SetActive(false);
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
        if (trigger)
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
        //PlayerController.inst.mainWeapon.Upgrade(MainWeapon.UpgradeType.fire, vfx_fire_attack, vfx_fire_upgrade);

        if (other.tag == "Player")
        {
            open = false;
            trigger = true;
            icon.SetActive(true);
            if (questionAnswered == false && upgradeChosen == false && fireAccepted == false && freezeAccepted == false && (Input.GetKeyDown(KeyCode.E) || (Input.GetButtonDown("Square"))))
            {
                Sure();

                open = true;
                question.SetActive(true);
                upgrade.SetActive(false);
                fire.SetActive(false);
                freeze.SetActive(false);
                callOut.SetActive(false);
                icon.SetActive(false);
            }
            else if (questionAnswered == true && upgradeChosen == false && fireAccepted == false && freezeAccepted == false && (Input.GetKeyDown(KeyCode.E) || (Input.GetButtonDown("Square"))))
            {
                Fire();
                open = true;
                question.SetActive(false);
                upgrade.SetActive(true);
                fire.SetActive(false);
                freeze.SetActive(false);
                callOut.SetActive(false);
                icon.SetActive(false);
            }
            else if (questionAnswered == true && upgradeChosen == true && fireChosen == true && fireAccepted == false && freezeAccepted == false && (Input.GetKeyDown(KeyCode.E) || (Input.GetButtonDown("Square"))))
            {
                FireConfirm();
                open = true;
                question.SetActive(false);
                upgrade.SetActive(false);
                fire.SetActive(true);
                freeze.SetActive(false);
                callOut.SetActive(false);
                icon.SetActive(false);
            }
            else if (questionAnswered == true && upgradeChosen == true && freezeChosen == true && fireAccepted == false && freezeAccepted == false && (Input.GetKeyDown(KeyCode.E) || (Input.GetButtonDown("Square"))))
            {
                open = true;
                question.SetActive(false);
                upgrade.SetActive(false);
                fire.SetActive(false);
                freeze.SetActive(true);
                callOut.SetActive(false);
                icon.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        open = false;
        trigger = false;
        question.SetActive(false);
        upgrade.SetActive(false);
        fire.SetActive(false);
        freeze.SetActive(false);
        callOut.SetActive(true);
        icon.SetActive(false);
    }
}
