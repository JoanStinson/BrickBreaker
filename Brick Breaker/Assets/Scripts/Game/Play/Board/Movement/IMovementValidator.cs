using UnityEngine;

namespace JGM.Game
{
    public interface IMovementValidator
    {
        bool CellIsValidForPiece(Vector2Int targetCellCoordinates, Vector2Int pieceCellCoordinates, BoardModel boardModel);
    }
}
