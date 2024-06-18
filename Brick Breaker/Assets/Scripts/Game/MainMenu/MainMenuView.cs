using UnityEngine;
using UnityEngine.UI;

namespace JGM.Game
{
    public class MainMenuView : ScreenView
    {
        [SerializeField] private Button m_playButton;
        [SerializeField] private Button m_quitButton;
        [SerializeField] private Button m_changeLanguageButton;

        private GameView m_gameView;

        public override void Initialize(GameView gameView)
        {
            m_gameView = gameView;
            m_playButton.onClick.AddListener(OnClickPlayButton);
            m_quitButton.onClick.AddListener(OnClickQuitButton);
            m_changeLanguageButton.onClick.AddListener(OnClickChangeLanguageButton);
        }

        private void OnClickPlayButton()
        {
            m_gameView.OnClickPlayButton();
        }

        private void OnClickQuitButton()
        {
            m_gameView.OnClickQuitButton();
        }

        private void OnClickChangeLanguageButton()
        {
            m_gameView.OnClickChangeLanguageButton();
        }
    }
}
