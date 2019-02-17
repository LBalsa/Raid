using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PS4UIControllerOptions : MonoBehaviour
{

    public GameObject main;
    public GameObject optionsMenu;
    public GameObject levelSelect;
    public GameObject textScreen;

    public Toggle sound;
    public Toggle music;

    int index = 0;
    public int options = 3;
    public float xOffset = 1f;

    void Update()
    {

        if (Input.GetButtonDown("R1") || Input.GetKeyDown(KeyCode.RightArrow))
        {

            if (index < (options - 1))
            {
                index++;
                Vector2 position = transform.position;
                position.x -= xOffset;
                transform.position = position;
            }

        }

        if (Input.GetButtonDown("L1") || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (index > 0)
            {
                index--;
                Vector2 position = transform.position;
                position.x += xOffset;
                transform.position = position;
            }
        }

        if (Input.GetButtonDown("Square") || Input.GetKeyDown(KeyCode.Return))
        {
            if (index == 0)
            {
                sound.isOn = !sound.isOn;
            }
            else if (index == 1)
            {
                music.isOn = !music.isOn;
            }
            else if (index == 2)
            {
                main.SetActive(true);
                optionsMenu.SetActive(false);
                levelSelect.SetActive(false);
                textScreen.SetActive(false);
            }

        }
    }
}
