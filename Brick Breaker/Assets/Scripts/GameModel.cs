using UnityEngine;
using static JGM.Game.GameSettings;

namespace JGM.Game
{
    public class GameModel
    {
        public int BoardRows { get; private set; }
        public int BoardColumns { get; private set; }
        public Color BoardCellDarkBrownColor { get; private set; }
        public Color BoardCellLightBrownColor { get; private set; }
        public Color BoardCellHighlightedColor { get; private set; }
        public float PieceEnabledColorAlpha { get; private set; }
        public float PieceDisabledColorAlpha { get; private set; }
        public bool ShowTutorialAlways { get; private set; }
        public bool CompletedTutorial { get; set; }
        public int LastPlayerWinId { get; set; }

        public int HighScore
        {
            get => PlayerPrefs.GetInt("best_score", 0);
            set
            {
                PlayerPrefs.SetInt("best_score", value);
                PlayerPrefs.Save();
            }
        }
        public int Score { get; set; } = 0;
        public int LevelOfFinalBrick { get; set; } = 1;
        public int ScoreMultiplier { get; set; } = 1;

        private readonly PieceConfig[] m_player1PieceConfigs;
        private readonly PieceConfig[] m_player2PieceConfigs;

        public GameModel(GameSettings gameSettings)
        {
            m_player1PieceConfigs = gameSettings.Player1PieceConfigs;
            m_player2PieceConfigs = gameSettings.Player2PieceConfigs;
            BoardRows = gameSettings.BoardRows;
            BoardColumns = gameSettings.BoardColumns;
            BoardCellDarkBrownColor = gameSettings.BoardCellDarkBrownColor;
            BoardCellLightBrownColor = gameSettings.BoardCellLightBrownColor;
            BoardCellHighlightedColor = gameSettings.BoardCellHighlightedColor;
            PieceEnabledColorAlpha = gameSettings.PieceEnabledColorAlpha;
            PieceDisabledColorAlpha = gameSettings.PieceDisabledColorAlpha;
            ShowTutorialAlways = gameSettings.ShowTutorialAlways;
        }

        public PieceConfig[] GetPieceConfigs(int playerIndex)
        {
            if (playerIndex == 0)
            {
                return m_player1PieceConfigs;
            }

            return m_player2PieceConfigs;
        }
    }
}