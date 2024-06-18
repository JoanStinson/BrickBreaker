using UnityEngine;

namespace JGM.Game
{
    public class RookMovementValidator : IMovementValidator
    {
        public bool CellIsValidForPiece(Vector2Int targetCellCoordinates, Vector2Int pieceCellCoordinates, BoardModel boardModel)
        {
            // Check if the move is in a straight line (either horizontally or vertically)
            if (targetCellCoordinates.x != pieceCellCoordinates.x && targetCellCoordinates.y != pieceCellCoordinates.y)
            {
                return false;
            }

            // Check if there are no pieces blocking the path
            if (!IsPathClear(targetCellCoordinates, pieceCellCoordinates, boardModel))
            {
                return false;
            }

            return true;
        }

        private bool IsPathClear(Vector2Int targetCellCoordinates, Vector2Int pieceCellCoordinates, BoardModel boardModel)
        {
            if (targetCellCoordinates.x == pieceCellCoordinates.x)
            {
                // Vertical move
                int startY = Mathf.Min(targetCellCoordinates.y, pieceCellCoordinates.y) + 1;
                int endY = Mathf.Max(targetCellCoordinates.y, pieceCellCoordinates.y);

                for (int y = startY; y < endY; y++)
                {
                    if (boardModel.GetCell(targetCellCoordinates.x, y).PieceModel != null)
                    {
                        return false;
                    }
                }
            }
            else
            {
                // Horizontal move
                int startX = Mathf.Min(targetCellCoordinates.x, pieceCellCoordinates.x) + 1;
                int endX = Mathf.Max(targetCellCoordinates.x, pieceCellCoordinates.x);

                for (int x = startX; x < endX; x++)
                {
                    if (boardModel.GetCell(x, targetCellCoordinates.y).PieceModel != null)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
