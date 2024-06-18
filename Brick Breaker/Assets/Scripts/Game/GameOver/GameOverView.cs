using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace JGM.Game
{
    public class GameOverView : ScreenView
    {
        [SerializeField] private LocalizedText m_playerWinsText;
        [SerializeField] private Button m_playAgainButton;
        [SerializeField] private Button m_mainMenuButton;
        [SerializeField] private Button m_quitButton;
        [SerializeField] private float m_winSfxSecondsDelay = 0.7f;
        [Inject] private IAudioService m_audioService;
        [Inject] private IHapticFeedbackService m_hapticFeedbackService;

        private GameView m_gameView;

        public override void Initialize(GameView gameView)
        {
            m_gameView = gameView;
            m_playAgainButton.onClick.AddListener(OnClickPlayAgainButton);
            m_mainMenuButton.onClick.AddListener(OnClickMainMenuButton);
            m_quitButton.onClick.AddListener(OnClickQuitButton);
        }

        private void OnClickPlayAgainButton()
        {
            m_gameView.OnClickPlayAgainButton();
        }

        private void OnClickMainMenuButton()
        {
            m_gameView.OnClickMainMenuButton();
        }

        private void OnClickQuitButton()
        {
            m_gameView.OnClickQuitButton();
        }

        public override async void Show()
        {
            base.Show();
            m_playerWinsText.SetIntegerValue(m_gameView.Model.LastPlayerWinId + 1);
            await Task.Delay(TimeSpan.FromSeconds(m_winSfxSecondsDelay));
            m_audioService.Play(AudioFileNames.WinCreditsSfx);
            m_hapticFeedbackService.TriggerVibration();
        }
    }
}