using Characters.Friendlies;
using UnityEngine;

namespace Controllers
{
    public class GameController : MonoBehaviour
    {
        public static GameController inst = null;

        public bool InCutscene { get; private set; } = false;
        private float cutsceneTimer = 0.0f;
        private float gameoverTimer = 0.0f;
        public Animator cutscene;
        public GameObject cutsceneCamera;

        #region Game Logic
        public bool Paused { get; set; } = false;
        public bool IsGameOver { get; private set; } = false;
        public int Level { get; private set; } = 0;
        public int Difficulty { get; private set; } = 1;

        // Game stats
        public float GameTimer { get; private set; } = 0.0f;
        public int EnemyDeathCount { get; set; } = 0;
        public int EnemySpawnCount { get; set; } = 0;
        #endregion

        #region Game Settings
        [Header("Game Debug Settings")]
        public bool spawnEnemies = true;
        public bool soundEffects = true;
        public bool gameMusic = true;
        public int soundVolume, musicVolume;
        #endregion

        #region Barriers
        [Header("Barriers")]
        public GameObject portBarrier;
        public GameObject poorBarrier;
        public GameObject merchantBarrier;
        public GameObject marketBarrier;
        public GameObject townhallBarrier;
        #endregion


        #region SFX
        [Header("Audio")]
        private AudioSource aus;
        public AudioClip sfx_theme;
        public AudioClip sfx_win;
        public AudioClip sfx_lose;
        #endregion

        // Game Credits
        public TextAsset textFile;     // drop your file here in inspector

        #region Game controls
        public void Pause()
        {
            // TODO: Throw event that will close all UI elements
            Paused = true;
            //Shop.inst.CloseShop();
            Time.timeScale = 0.0f;
            MenuController.inst.Pause();
            //Cursor.lockState = CursorLockMode.Locked;

        }

        public void Unpause()
        {
            //Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1.0f;
            Paused = false;
        }
        #endregion

        private void Awake()
        {
            // Singleton pattern.
            if (inst == null) { inst = this; }
            else { Destroy(this); }


            // Get settings.
            aus = GetComponent<AudioSource>();
            Cursor.lockState = CursorLockMode.Locked;


            InCutscene = true;
            cutsceneTimer = 0f;
            cutsceneCamera.SetActive(true);
        }
        private void Start()
        {
            // Prevent player from moving during cutscene.
            PlayerController.inst.CanMove = false;
        }


        private void Update()
        {
            // Intro cutscene timer.
            if (InCutscene && cutsceneTimer < 15) { cutsceneTimer += Time.deltaTime; }
            else if (InCutscene) { SkipIntroCutscene(); }

            // Skip cutscene.
            if (InCutscene && Input.GetKeyDown(KeyCode.E)) { SkipIntroCutscene(); }

            // Gameover timeout will take player to main menu after 5 minutes
            if (IsGameOver)
            {
                if (gameoverTimer < 300) { gameoverTimer += Time.deltaTime; }
                else { MenuController.inst.MainMenu(); }
                return;
            }

            // (Un)Pause game via P. Ignore during cutscenes.
            if (Input.GetKeyDown(KeyCode.P) && !InCutscene)
            {
                if (!Paused)
                {
                    Pause();
                }
                else
                {
                    Unpause(); MenuController.inst.Unpause();
                }
            }

            GameTimer++;
        }

        public void SkipIntroCutscene()
        {
            cutsceneCamera.SetActive(false);
            InCutscene = false;
            PlayerController.inst.CanMove = true;
        }

        public void IncreaseEnemyDeathCount()
        {
            EnemyDeathCount++;
        }

        public void LevelFinished()
        {
            Level++;
            UnlockArea();
        }

        private void UnlockArea()
        {
            switch (Level)
            {
                case 1:
                    Destroy(portBarrier);
                    break;
                case 2:
                    Destroy(poorBarrier);
                    break;
                case 3:
                    Destroy(merchantBarrier);
                    break;
                case 4:
                    Destroy(marketBarrier);
                    break;
                case 5:
                    Destroy(townhallBarrier);
                    // Win Win
                    break;
            }
        }

        public void GameOver()
        {
            IsGameOver = true;
            MenuController.inst.gameOverMenu.SetActive(true);
            MenuController.inst.gameStats.text = "Time played: " + gameoverTimer + "\nGold collected: " + EnemyDeathCount + "\nEnemies beat: " + EnemyDeathCount;

            // Free mouse cursor.
            //Cursor.lockState = CursorLockMode.None;

            // Play game over track.
            if (sfx_lose)
            {
                MenuController.inst.audioMixer.SetFloat("SoundtrackVolume", 0);
                aus.clip = sfx_lose;
                aus.Play();
            }

            // Load game credits and animate them upwards.
            MenuController.inst.gameOverMenu.GetComponentInChildren<UnityEngine.UI.Text>().text = System.IO.File.ReadAllText(Application.dataPath + "/Resources/GameCredits.txt");
            //gameOverScreen.GetComponent<Animator>().SetTrigger("Play");
        }

    }
}
#region OBSOLETE
/*
switch (level)
{
    case 1:
        if (enemySpawnCount < portLevelEnemies && timer == 0)
        {
            //spawn next wave
            timer = 60.0f;
        }
        else if (enemySpawnCount == portLevelEnemies && enemyDeathCount == portLevelEnemies)
        {
            level++;
            portBarrier.SetActive(false);
            enemySpawnCount = 0;
            timer = 60.0f;
        }
        break;
    case 2:
        if (enemySpawnCount < poorLevelEnemies && timer == 0)
        {
            //spawn next wave
            timer = 60.0f;
        }
        else if (enemySpawnCount == poorLevelEnemies && enemyDeathCount == (portLevelEnemies + poorLevelEnemies))
        {
            level++;
            poorBarrier.SetActive(false);
            enemySpawnCount = 0;
            timer = 60.0f;
        }
        break;
    case 3:
        if (enemySpawnCount < merchantLevelEnemies && timer == 0)
        {
            //spawn next wave
            timer = 60.0f;
        }
        else if (enemySpawnCount == merchantLevelEnemies && enemyDeathCount == (portLevelEnemies + poorLevelEnemies + merchantLevelEnemies))
        {
            level++;
            merchantBarrier.SetActive(false);
            enemySpawnCount = 0;
            timer = 60.0f;
        }
        break;
    case 4:
        if (enemySpawnCount < marketLevelEnemies && timer == 0)
        {
            //spawn next wave
            timer = 60.0f;
        }
        else if (enemySpawnCount == marketLevelEnemies && enemyDeathCount == (portLevelEnemies + poorLevelEnemies + merchantLevelEnemies + marketLevelEnemies))
        {
            level++;
            marketBarrier.SetActive(false);
            townhallBarrier.SetActive(false);
            enemySpawnCount = 0;
            timer = 60.0f;
        }
        break;
    case 5:
        if (enemySpawnCount < townhallLevelEnemies && timer == 0)
        {
            //spawn next wave
            timer = 60.0f;
        }
        else if (enemySpawnCount == townhallLevelEnemies && enemyDeathCount == (portLevelEnemies + poorLevelEnemies + merchantLevelEnemies + marketLevelEnemies + townhallLevelEnemies))
        {
            //GAME COMPLETE
        }
        break;
}
*/
#endregion obsolete 
