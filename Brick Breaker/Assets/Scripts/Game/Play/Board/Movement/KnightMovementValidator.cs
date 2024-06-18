using UnityEngine;

namespace JGM.Game
{
    public class KnightMovementValidator : IMovementValidator
    {
        public bool CellIsValidForPiece(Vector2Int targetCellCoordinates, Vector2Int pieceCellCoordinates, BoardModel boardModel)
        {
            int deltaX = Mathf.Abs(targetCellCoordinates.x - pieceCellCoordinates.x);
            int deltaY = Mathf.Abs(targetCellCoordinates.y - pieceCellCoordinates.y);

            // The move is valid if it forms an L-shape: (2, 1) or (1, 2)
            return (deltaX == 2 && deltaY == 1) || (deltaX == 1 && deltaY == 2);
        }
    }
}
