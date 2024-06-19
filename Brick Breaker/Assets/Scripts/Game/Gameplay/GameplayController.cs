using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace JGM.Game
{
    public class GameplayController : MonoBehaviour
    {
        public static GameplayController Instance;

        public GameObject m_MainMenuPanel;
        public GameObject m_GameMenuPanel;
        public GameObject m_GameOverPanel;
        public GameObject m_Scores;
        public Text m_GameOverFinalScore;

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
                        m_MainMenuPanel.SetActive(true);
                        m_GameMenuPanel.SetActive(false);
                        m_GameOverPanel.SetActive(false);
                        m_Scores.SetActive(true);

                        BallLauncherView.Instance.OnMainMenuActions();
                        BrickRowSpawnerView.Instance.HideAllRows();
                        break;
                    case GameState.Playable:
                        if (Saver.Instance.HasSave())
                        {

                        }
                        else
                        {
                            m_MainMenuPanel.SetActive(false);
                            m_GameMenuPanel.SetActive(true);
                            m_GameOverPanel.SetActive(false);
                            m_Scores.SetActive(true);

                            BallLauncherView.Instance.m_CanPlay = true;
                            BrickRowSpawnerView.Instance.m_LevelOfFinalBrick = 1;  // temporary (after save and load)

                            // reset score (probably by conditions)
                            ScoreManager.Instance.m_ScoreText.text = BrickRowSpawnerView.Instance.m_LevelOfFinalBrick.ToString();

                            BrickRowSpawnerView.Instance.SpawnNewRows();
                        }
                        break;
                    case GameState.GameOver:
                        m_MainMenuPanel.SetActive(false);
                        m_GameMenuPanel.SetActive(false);
                        m_GameOverPanel.SetActive(true);
                        m_Scores.SetActive(false);

                        m_GameOverFinalScore.text = "Final Score : " + (BrickRowSpawnerView.Instance.m_LevelOfFinalBrick - 1).ToString();
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
            m_GameState = GameState.MainMenu;
        }
    }
}