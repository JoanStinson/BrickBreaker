using System.Collections.Generic;
using UnityEngine;

namespace JGM.Game
{
    public class PlayController
    {
        private const int m_minBoardPiecesForTicTacToe = 5;
        private readonly BoardView m_boardView;
        private BoardModel m_boardModel;
        private int m_playerTurn;

        public PlayController(BoardView boardView)
        {
            m_boardView = boardView;
        }

        public BoardModel BuildBoardModel(GameModel model)
        {
            m_boardModel = new BoardModel(model.BoardRows, model.BoardColumns);
            return m_boardModel;
        }

        public void GenerateFirstTurnRandomly()
        {
            m_playerTurn = Random.Range(0, 2);
        }

        public void ChangePlayerTurn()
        {
            m_playerTurn ^= 1;
        }

        public int GetPlayerTurn()
        {
            return m_playerTurn;
        }

        public int GetNonPlayerTurn()
        {
            return m_playerTurn ^ 1;
        }

        public bool TicTacToeFound()
        {
            bool piecesAmountRequired = (m_boardView.CalculatePiecesAmount() >= m_minBoardPiecesForTicTacToe);
            if (!piecesAmountRequired)
            {
                return false;
            }

            bool ticTacToe = m_boardView.CheckTicTacToe();
            return ticTacToe;
        }

        public int GetMinBoardPiecesForTicTacToe()
        {
            return m_minBoardPiecesForTicTacToe;
        }

        public bool CanPlayerMove(IReadOnlyList<PieceView> playerPieces)
        {
            bool allPiecesArePlacedOnBoard = (m_boardView.PiecesOnBoard == m_boardModel.Rows + m_boardModel.Columns);
            if (!allPiecesArePlacedOnBoard)
            {
                return true;
            }

            bool canPlayerMove = m_boardView.AnyPieceFromPlayerCanMove(playerPieces, m_boardModel);
            return canPlayerMove;
        }
    }
}
