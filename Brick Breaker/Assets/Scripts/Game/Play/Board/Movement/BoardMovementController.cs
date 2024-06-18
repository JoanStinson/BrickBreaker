using System.Collections.Generic;
using UnityEngine;

namespace JGM.Game
{
    public class BoardMovementController
    {
        private readonly Dictionary<GameSettings.PieceType, IMovementValidator> m_movementValidators;

        public BoardMovementController()
        {
            m_movementValidators = new Dictionary<GameSettings.PieceType, IMovementValidator>()
            {
                { GameSettings.PieceType.Knight, new KnightMovementValidator() },
                { GameSettings.PieceType.Rook,   new RookMovementValidator() },
                { GameSettings.PieceType.Bishop, new BishopMovementValidator() }
            };
        }

        public bool CellIsValidForPiece(CellModel cell, PieceView piece, BoardModel boardModel)
        {
            bool allPiecesAreNotPlaced = boardModel.CalculatePiecesOnBoard() < boardModel.Rows + boardModel.Columns;
            if (allPiecesAreNotPlaced)
            {
                return cell.IsEmpty;
            }

            var validator = m_movementValidators[piece.Model.PieceType];
            return cell.IsEmpty && validator.CellIsValidForPiece(cell.Coordinates, piece.CellView?.Model.Coordinates ?? Vector2Int.zero, boardModel);
        }

        public bool AnyPieceFromPlayerCanMove(IReadOnlyList<PieceView> pieces, BoardModel boardModel)
        {
            foreach (var piece in pieces)
            {
                if (piece.CellView == null)
                {
                    continue;
                }

                var pieceCoordinates = piece.CellView.Model.Coordinates;
                var validator = m_movementValidators[piece.Model.PieceType];

                for (int i = 0; i < boardModel.Rows; i++)
                {
                    for (int j = 0; j < boardModel.Columns; j++)
                    {
                        var targetCell = boardModel.GetCell(i, j);

                        if (targetCell.IsEmpty && validator.CellIsValidForPiece(targetCell.Coordinates, pieceCoordinates, boardModel))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
