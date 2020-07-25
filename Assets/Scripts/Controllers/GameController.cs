using Characters.Player;
using UnityEngine;

namespace Controllers
{
    public class GameController : MonoBehaviour
    {
        public static GameController inst = null;
        public event VoidEvent OnPause;
        public event VoidEvent OnUnPause;
        public event GameOverHandler OnGameOver;
        public delegate void VoidEvent();
        public delegate void GameOverHandler(GameOverArgs args);

        public bool InCutscene { get; private set; } = false;
        private float cutsceneTimer = 0.0f;
        public Animator cutscene;
        public GameObject cutsceneCamera;

        #region Game Logic
        public bool Paused { get; set; } = false;
        private bool IsGameOver { get; set; } = false;
        public int Level { get; private set; } = 0;
        public int Difficulty { get; private set; } = 1;

        // Game stats
        public float GameTimer { get; private set; } = 0.0f;
        private int EnemyDeathCount { get; set; } = 0;
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
            if (!Paused)
            {
                Paused = true;
                //Shop.inst.CloseShop();
                Time.timeScale = 0.0f;
                OnPause?.Invoke();
            }

        }

        public void Unpause()
        {
            if (Paused)
            {
                Time.timeScale = 1.0f;
                Paused = false;
                OnUnPause?.Invoke();
            }
        }
        #endregion

        private void Awake()
        {
            // Singleton pattern.
            if (inst == null) { inst = this; }
            else { Destroy(this); }

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

            // (Un)Pause game via P. Ignore during cutscenes.
            if (Input.GetKeyDown(KeyCode.P) && !InCutscene)
            {
                if (!Paused)
                {
                    Pause();
                }
                else
                {
                    Unpause();
                }
            }

            if (!Paused && !IsGameOver)
            {
                GameTimer++;
            }
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
            UnlockNextArea();
        }

        private void UnlockNextArea()
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

            // Play game over track.
            if (sfx_lose)
            {
                MenuController.inst.audioMixer.SetFloat("SoundtrackVolume", 0);
                aus.clip = sfx_lose;
                aus.Play();
            }

            OnGameOver?.Invoke(new GameOverArgs { EnemyDeathCount = EnemyDeathCount, GameTime = GameTimer });
        }
    }
}
