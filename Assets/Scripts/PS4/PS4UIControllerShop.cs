using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controllers;
using Characters.Friendlies;

public class PS4UIControllerShop : MonoBehaviour
{
    public GameObject pauseScreen;

    int index = 0;
    public int options = 4;
    public float yOffset = 1f;

    void Update()
    {

        if (Input.GetButtonDown("R1") || Input.GetKeyDown(KeyCode.RightArrow))
        {

            if (index < (options - 1))
            {
                index++;
                Vector2 position = transform.position;
                position.y -= yOffset;
                transform.position = position;
            }

        }

        if (Input.GetButtonDown("L1") || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (index > 0)
            {
                index--;
                Vector2 position = transform.position;
                position.y += yOffset;
                transform.position = position;
            }
        }

        if (Input.GetButtonDown("Square") || Input.GetKeyDown(KeyCode.Return))
        {
            if (index == 0)
            {
                if (HealthManager.inst.Money > 1)
                {
                    HealthManager.inst.Money--;
                    HealthManager.inst.Heal(1);
                }
            }
            else if (index == 1)
            {
                if (HealthManager.inst.Money > 1)
                {
                    HealthManager.inst.Money--;
                    HealthManager.inst.BuyBread();
                }
            }
            else if (index == 2)
            {
                if (HealthManager.inst.Money > 5)
                {
                    HealthManager.inst.Money -= 5;
                    HealthManager.inst.HealthUpgrade();
                }
            }
            else if (index == 3)
            {
                GetComponent<Shop>().CloseShop();
                pauseScreen.SetActive(false);
                Time.timeScale = 1.0f;
            }
        }
        if (Input.GetButtonDown("Circle") || Input.GetKeyDown(KeyCode.Return))
        {
            pauseScreen.SetActive(false);
            GetComponent<Shop>().CloseShop();

        }

    }
}