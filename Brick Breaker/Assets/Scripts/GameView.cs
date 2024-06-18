using UnityEngine;
using Zenject;

namespace JGM.Game
{
    public class GameView : MonoBehaviour
    {
        public Canvas Canvas => m_canvas;
        public GameModel Model => m_gameModel;

        [SerializeField] private Canvas m_canvas;
        [SerializeField] private ScreenView m_mainMenuView;
        [SerializeField] private ScreenView m_playView;
        [SerializeField] private ScreenView m_gameOverView;

        [Inject] private GameSettings m_gameSettings;
        [Inject] private IAudioService m_audioService;
        [Inject] private ILocalizationService m_localizationService;

        private GameController m_gameController;
        private GameModel m_gameModel;

        public void Initialize()
        {
            m_gameController = new GameController(m_audioService, m_localizationService);
            m_gameModel = m_gameController.BuildGameModel(m_gameSettings);
            m_gameController.PlayBackgroundMusic();

            m_mainMenuView.Initialize(this);
            m_playView.Initialize(this);
            m_gameOverView.Initialize(this);

            m_mainMenuView.Hide();
            m_playView.Show();
            m_gameOverView.Hide();
        }

        public void OnClickPlayButton()
        {
            m_mainMenuView.Hide();
            m_playView.Show();
            m_gameController.PlayPressButtonSfx();
        }

        public void OnClickQuitButton()
        {
            m_gameController.Quit();
            m_gameController.PlayPressButtonSfx();
        }

        public void OnClickChangeLanguageButton()
        {
            m_gameController.ChangeLanguageToRandom();
            m_gameController.PlayPressButtonSfx();
        }

        public void OnTicTacToeFound(int playerWinId)
        {
            m_gameModel.LastPlayerWinId = playerWinId;
            m_playView.Hide();
            m_gameOverView.Show();
        }

        public void OnClickPlayAgainButton()
        {
            m_gameOverView.Hide();
            m_playView.Show();
            m_gameController.PlayPressButtonSfx();
        }

        public void OnClickMainMenuButton()
        {
            m_gameOverView.Hide();
            m_mainMenuView.Show();
            m_gameController.PlayPressButtonSfx();
        }

        public void OnClickPlayBackButton()
        {
            m_playView.Hide();
            m_mainMenuView.Show();
            m_gameController.PlayPressButtonSfx();
        }
    }
}