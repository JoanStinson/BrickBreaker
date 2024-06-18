using UnityEngine;

namespace JGM.Game
{
    public class BishopMovementValidator : IMovementValidator
    {
        public bool CellIsValidForPiece(Vector2Int targetCellCoordinates, Vector2Int pieceCellCoordinates, BoardModel boardModel)
        {
            // Check if the move is along a diagonal
            if (Mathf.Abs(targetCellCoordinates.x - pieceCellCoordinates.x) != Mathf.Abs(targetCellCoordinates.y - pieceCellCoordinates.y))
            {
                return false;
            }

            // Check if the path is clear
            if (!IsPathClear(targetCellCoordinates, pieceCellCoordinates, boardModel))
            {
                return false;
            }

            return true;
        }

        private bool IsPathClear(Vector2Int targetCellCoordinates, Vector2Int pieceCellCoordinates, BoardModel boardModel)
        {
            int xDirection = (targetCellCoordinates.x > pieceCellCoordinates.x) ? 1 : -1;
            int yDirection = (targetCellCoordinates.y > pieceCellCoordinates.y) ? 1 : -1;

            int x = pieceCellCoordinates.x + xDirection;
            int y = pieceCellCoordinates.y + yDirection;

            while (x != targetCellCoordinates.x && y != targetCellCoordinates.y)
            {
                if (boardModel.GetCell(x, y).PieceModel != null)
                {
                    return false;
                }

                x += xDirection;
                y += yDirection;
            }

            return true;
        }
    }
}
