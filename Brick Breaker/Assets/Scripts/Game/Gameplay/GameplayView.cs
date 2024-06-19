using UnityEngine;
using UnityEngine.UI;

namespace JGM.Game
{
    public class GameplayView : MonoBehaviour
    {
        public static GameplayView Instance;
        public int HighScore { private set; get; }
        public int Rings { private set; get; }

        [SerializeField] private GameObject m_mainMenuPanel;
        [SerializeField] private GameObject m_gameMenuPanel;
        [SerializeField] private GameObject m_gameOverPanel;
        [SerializeField] private GameObject m_scores;
        [SerializeField] private Text m_gameOverFinalScore;
        [SerializeField] private Text m_highScoreText;
        [SerializeField] private Text m_scoreText;

        public enum GameState { MainMenu, Playable, GameOver, }
        private GameState m_State = GameState.MainMenu;

        public GameState m_GameState
        {
            set
            {
                m_State = value;

                switch (value)
                {
                    case GameState.MainMenu:
                        m_mainMenuPanel.SetActive(true);
                        m_gameMenuPanel.SetActive(false);
                        m_gameOverPanel.SetActive(false);
                        m_scores.SetActive(true);
                        BallLauncherView.Instance.OnMainMenuActions();
                        BrickRowSpawnerView.Instance.HideAllRows();
                        break;

                    case GameState.Playable:
                        m_mainMenuPanel.SetActive(false);
                        m_gameMenuPanel.SetActive(true);
                        m_gameOverPanel.SetActive(false);
                        m_scores.SetActive(true);

                        BallLauncherView.Instance.m_CanPlay = true;
                        BrickRowSpawnerView.Instance.m_LevelOfFinalBrick = 1;  // temporary (after save and load)

                        // reset score (probably by conditions)
                        GameplayView.Instance.m_scoreText.text = BrickRowSpawnerView.Instance.m_LevelOfFinalBrick.ToString();
                        BrickRowSpawnerView.Instance.SpawnNewRows();
                        break;

                    case GameState.GameOver:
                        m_mainMenuPanel.SetActive(false);
                        m_gameMenuPanel.SetActive(false);
                        m_gameOverPanel.SetActive(true);
                        m_scores.SetActive(false);
                        m_gameOverFinalScore.text = "Final Score : " + (BrickRowSpawnerView.Instance.m_LevelOfFinalBrick - 1).ToString();
                        BallLauncherView.Instance.m_CanPlay = false;
                        BallLauncherView.Instance.ResetPositions();
                        break;
                }
            }
            get
            {
                return m_State;
            }
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            HighScore = PlayerPrefs.GetInt("best_score", 0);
            m_highScoreText.text = HighScore.ToString();
            m_scoreText.text = BrickRowSpawnerView.Instance.m_LevelOfFinalBrick.ToString();
        }

        public void Initialize()
        {
            m_GameState = GameState.Playable;
        }

        public void AddRingToInventory(int value)
        {
            if (value > 0)
            {
                Rings += value;
            }

            PlayerPrefs.SetInt("rings", Rings);
        }

        public void UpdateScore()
        {
            if (BrickRowSpawnerView.Instance.m_LevelOfFinalBrick > HighScore)
            {
                HighScore = BrickRowSpawnerView.Instance.m_LevelOfFinalBrick;
                m_highScoreText.text = HighScore.ToString();
                PlayerPrefs.SetInt("best_score", HighScore);
            }

            m_scoreText.text = BrickRowSpawnerView.Instance.m_LevelOfFinalBrick.ToString();
        }
    }
}