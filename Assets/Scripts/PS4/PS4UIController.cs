//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;

//public class PS4UIController : MonoBehaviour
//{
//    public GameObject main;
//    public GameObject optionsMenu;
//    public GameObject levelSelect;
//    public GameObject textScreen;

//    int index = 0;
//    public int options = 4;
//    public float xOffset = 5f;

//    /*
//    void Update()
//    {

//        if (Input.GetButtonDown("R1") || Input.GetKeyDown(KeyCode.RightArrow))
//        {

//            if (index < (options - 1))
//            {
//                index++;
//                Vector2 position = transform.position;
//                position.x -= xOffset;
//                transform.position = main.transform.position;//position;
//            }

//        }

//        if (Input.GetButtonDown("L1") || Input.GetKeyDown(KeyCode.LeftArrow))
//        {
//            if (index > 0)
//            {
//                index--;
//                Vector2 position = transform.position;
//                position.x += xOffset;
//                transform.position = position;
//            }
//        }

//        if (Input.GetButtonDown("Square") || Input.GetKeyDown(KeyCode.Return))
//        {
//            // Start
//            if (index == 0)
//            {
//                main.SetActive(false);
//                optionsMenu.SetActive(false);
//                levelSelect.SetActive(false);
//                textScreen.SetActive(true);
//            }
//            else if (index == 1)
//            {

//            }
//            // Options
//            else if (index == 2)
//            {
//                main.SetActive(false);
//                optionsMenu.SetActive(true);
//                levelSelect.SetActive(false);
//                textScreen.SetActive(false);
//            }
//            // Quit
//            else if (index == 3)
//            {
//                Application.Quit();
//            }
//        }
//    }
//    */

//    public void StartGame()
//    {
//        main.SetActive(false);
//        optionsMenu.SetActive(false);
//        levelSelect.SetActive(false);
//        textScreen.SetActive(true);
//    }
//    public void HighScore()
//    {

//    }
//    public void Options()
//    {
//        main.SetActive(false);
//        optionsMenu.SetActive(true);
//        levelSelect.SetActive(false);
//        textScreen.SetActive(false);
//    }
//    public void Back()
//    {

//    }
//    public void Quit()
//    {
//        Application.Quit();
//    }
//}
