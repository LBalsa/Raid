using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public static MenuController inst;
    [Header("Canvas")]
    public GameObject gameOverMenu;
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject instructionsMenu;

    public Text gameStats;
    [Header("Sound")]
    public AudioMixer audioMixer;
    private Resolution[] availableResolutions;
    public Dropdown resolutionDropdown;
    public Dropdown qualityDropdown;
    public Toggle fullSreenToggle;
    private void Awake()
    {
        // Singleton pattern.
        if (inst == null) { inst = this; }
        else { Destroy(this); }

        // Check and deactivate canvas elements.
        if (pauseMenu) { pauseMenu.SetActive(false); }
        else { Debug.LogError("Missing reference: pause menu."); }
        if (optionsMenu) { optionsMenu.SetActive(false); }
        else { Debug.LogError("Missing reference: options menu."); }
        if (instructionsMenu) { instructionsMenu.SetActive(false); }
        else { Debug.LogError("Missing reference: instructions menu."); }
        if (gameOverMenu) { gameOverMenu.SetActive(false); }
        else { Debug.LogError("Missing reference: game over menu."); }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        // Clear resolution options.
        resolutionDropdown.ClearOptions();

        // Get available resolutions for current screen.
        availableResolutions = Screen.resolutions;

        // Convert Resolution array into a string list.
        List<string> resolutionsList = new List<string>();
        for (int i = 0; i < availableResolutions.Length; i++)
        {
            string resolution = availableResolutions[i].width + " * " + availableResolutions[i].width;
            resolutionsList.Add(resolution);

            // Check if this is the current resolution and set it in the dropdown.
            if (availableResolutions[i].width == Screen.currentResolution.width && availableResolutions[i].height == Screen.currentResolution.height)
            {
                resolutionDropdown.value = i;
            }
        }

        // Add resolutions to dropdown and update shown value;
        resolutionDropdown.AddOptions(resolutionsList);
        resolutionDropdown.RefreshShownValue();

        // Select correct quality settings.
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        qualityDropdown.RefreshShownValue();

        // Set fullscreen toggle.
        if (fullSreenToggle)
        {
        fullSreenToggle.isOn = Screen.fullScreen;
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    #region Menu

    public void Continue()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        instructionsMenu.SetActive(false);

        // Unpause game if continue button pressed.
        if (Time.timeScale == 0) { GameController.inst.Unpause(); }
    }

    public void Options()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void Instructions()
    {
        pauseMenu.SetActive(false);
        instructionsMenu.SetActive(true);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
        //Time.timeScale = 1.0f;
    }

    public void Quit()
    {
        Time.timeScale = 1.0f;
        Application.Quit();
    }

    private void SelectButton(GameObject menu)
    {
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(menu.GetComponentInChildren<UnityEngine.UI.Button>().gameObject);

    }
    #endregion

    #region Gameover Menu

    public void StartOver()
    {
        SceneManager.LoadScene("Game Map");
        Time.timeScale = 1.0f;
    }

    #endregion

    #region Options Menu

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution =availableResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        if (isFullScreen)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
        }
    }

    public void SetSoundtrackVolume(float volume)
    {
        audioMixer.SetFloat("SoundtrackVolume", volume);
    }

    public void SetEffectsVolume(float volume)
    {
        audioMixer.SetFloat("EffectsVolume", volume);
    }

    public void Back()
    {
        optionsMenu.SetActive(false);
        instructionsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    #endregion



    /*
     Everyone knows WASD moves, space defies gravity and mice bite...Holding shift will put you in mean pirate mode.
Then yee click the mice to fight off them pesky villagers, either swordlike or kick 'em. If you switch back and forth it's a mean combo move.
If the sword gets heavy, just drop it with R and use these mighty beaters! Pick it back up with E.
The numbers above call the men back on the ship for backup, skillz as them gamers call it.
Saavy? Figure the rest out me ladie, and if you ever need help, well don't count on me, as I can't count! And forget about the dev, he don't like instructions.
Now, let's go get me gold while I work on proper instructions!"
     */

}
