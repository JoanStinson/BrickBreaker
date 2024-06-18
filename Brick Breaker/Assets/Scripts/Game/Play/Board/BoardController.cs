using System.Collections.Generic;

namespace JGM.Game
{
    public class BoardController
    {
        private readonly BoardModel m_boardModel;
        private readonly BoardMovementController m_boardMovementController;
        private readonly BoardResultController m_boardResultController;

        public BoardController(BoardModel boardModel)
        {
            m_boardModel = boardModel;
            m_boardMovementController = new BoardMovementController();
            m_boardResultController = new BoardResultController();
        }

        public bool CellIsValidForPiece(CellModel cell, PieceView piece)
        {
            return m_boardMovementController.CellIsValidForPiece(cell, piece, m_boardModel);
        }

        public bool CheckTicTacToe()
        {
            return m_boardResultController.CheckTicTactoe(m_boardModel);
        }

        public bool AnyPieceFromPlayerCanMove(IReadOnlyList<PieceView> pieces, BoardModel boardModel)
        {
            return m_boardMovementController.AnyPieceFromPlayerCanMove(pieces, boardModel);
        }
    }
}
