using UnityEngine;
using UnityEngine.UI;

namespace JGM.Game
{
    public class LeaderboardsView : ScreenView
    {
        [SerializeField] private Button m_playAgainButton;
        [SerializeField] private Button m_mainMenuButton;
        [SerializeField] private Button m_quitButton;
        [SerializeField] private Transform m_scrollContent;
        [SerializeField] private LeaderboardCellView m_cell;

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

        public override void Show()
        {
            base.Show();

            int userScore = m_gameView.Model.Score * m_gameView.Model.ScoreMultiplier;
            var cell = GameObject.Instantiate(m_cell);
            cell.transform.SetParent(m_scrollContent, false);
            int position = 0;
            string name = "Pepe";
            cell.SetText($"Position {position} - {name} - {userScore}");
        }
    }
}
