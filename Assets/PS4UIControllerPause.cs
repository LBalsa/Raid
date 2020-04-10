using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Characters.Friendlies;

public class PS4UIControllerPause : MonoBehaviour {
    public GameObject pauseScreen;

    int index = 0;
    public int options = 3;
    public float xOffset = 1f;

    void Start()
    {
        Time.timeScale = 0.0f;
        pauseScreen.SetActive(true);
    }

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
                pauseScreen.SetActive(false);
                Time.timeScale = 1.0f;
            }
            else if (index == 1)
            {
                SceneManager.LoadScene("Main Menu");
            }
            else if (index == 2)
            {
                Application.Quit();
            }
        }
    }
}
