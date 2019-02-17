using UnityEngine;
using UnityEngine.Audio;

public class MainMenuController : MonoBehaviour
{
    public static MainMenuController inst;

    [Header("Canvas elements")]
    public GameObject main;
    public GameObject options;
    public GameObject instructions;
    public GameObject highscores;
    public GameObject levelSelect;
    public GameObject textScreen;
    public AudioMixer audioMixer;

    [Header("Settings")]
    public bool soundEffects = true, gameMusic = true;


    public bool level1 = true;
    public bool level2 = false;
    public bool level3 = false;
    public bool level4 = false;
    public bool level5 = false;

    private void Awake()
    {
        inst = this;
        main.SetActive(true);
        options.SetActive(false);
        instructions.SetActive(false);
        highscores.SetActive(false);
        levelSelect.SetActive(false);
        textScreen.SetActive(false);
    }

    public void StartGame()
    {
        main.SetActive(false);
        options.SetActive(false);
        instructions.SetActive(false);
        highscores.SetActive(false);
        levelSelect.SetActive(false);
        textScreen.SetActive(true);
    }

    public void LevelSelect()
    {
        main.SetActive(false);
        options.SetActive(false);
        levelSelect.SetActive(true);
        textScreen.SetActive(false);
    }

    public void HighScore()
    {
        main.SetActive(false);
        highscores.SetActive(false);
    }

    public void Options()
    {
        main.SetActive(false);
        options.SetActive(true);
    }

    public void Instructions()
    {
        main.SetActive(false);
        instructions.SetActive(true);
    }

    #region Options Menu
    public void SetSoundtrackVolume(float volume)
    {
        audioMixer.SetFloat("SoundtrackVolume", volume);
    }

    public void SetEffectsVolume(float volume)
    {
        audioMixer.SetFloat("EffectsVolume", volume);
    }
    #endregion

    public void Back()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        options.SetActive(false);
        instructions.SetActive(false);
        highscores.SetActive(false);
        levelSelect.SetActive(false);
        main.SetActive(true);
    }


    public void Quit()
    {
        Application.Quit();
    }

    public void SoundToggle()
    {
        if (soundEffects)
        {
            soundEffects = false;
        }
        else
        {
            soundEffects = true;
        }
    }

    public void MusicToggle()
    {
        if (gameMusic)
        {
            gameMusic = false;
        }
        else
        {
            gameMusic = true;
        }
    }

    public void RequestOptions()
    {

    }
}
