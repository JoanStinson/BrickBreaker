using System;
using UnityEngine;

namespace JGM.Game
{
    [CreateAssetMenu(fileName = "New Game Settings", menuName = "Game Settings", order = 0)]
    public class GameSettings : ScriptableObject
    {
        public int BoardRows => 3;
        public int BoardColumns => 3;
        public Color BoardCellLightBrownColor => m_boardCellLightBrownColor;
        public Color BoardCellDarkBrownColor => m_boardCellDarkBrownColor;
        public Color BoardCellHighlightedColor => m_boardCellHighlightedColor;
        public PieceConfig[] Player1PieceConfigs => m_player1PieceConfigs;
        public PieceConfig[] Player2PieceConfigs => m_player2PieceConfigs;
        public float PieceEnabledColorAlpha => m_pieceEnabledColorAlpha;
        public float PieceDisabledColorAlpha => m_pieceDisabledColorAlpha;
        public bool ShowTutorialAlways => m_showTutorialAlways;

        [Header("Board")]
        [SerializeField] private Color m_boardCellLightBrownColor;
        [SerializeField] private Color m_boardCellDarkBrownColor;
        [SerializeField] private Color m_boardCellHighlightedColor;

        [Header("Pieces")]
        [SerializeField] private PieceConfig[] m_player1PieceConfigs;
        [SerializeField] private PieceConfig[] m_player2PieceConfigs;
        [SerializeField, Range(0, 1)] private float m_pieceEnabledColorAlpha = 1.0f;
        [SerializeField, Range(0, 1)] private float m_pieceDisabledColorAlpha = 0.5f;

        [Header("Tutorial")]
        [SerializeField] private bool m_showTutorialAlways;

        [Serializable]
        public class PieceConfig
        {
            public PieceType PieceType;
            public Sprite Sprite;
        }

        public enum PieceType
        {
            Knight,
            Rook,
            Bishop
        }
    }
}