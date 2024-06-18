using UnityEngine;

namespace JGM.Game
{
    public class CellModel
    {
        public PieceModel PieceModel { get; private set; }
        public Color DefaultColor { get; private set; }
        public Color HighlightedColor { get; private set; }
        public Vector2Int Coordinates { get; private set; }
        public bool IsEmpty => (PieceModel == null);

        public CellModel()
        {
            PieceModel = null;
            DefaultColor = Color.white;
            HighlightedColor = Color.red;
            Coordinates = Vector2Int.zero;
        }

        public CellModel(PieceModel pieceModel, Color defaultColor, Color highlightedColor, Vector2Int coordinates)
        {
            PieceModel = pieceModel;
            DefaultColor = defaultColor;
            HighlightedColor = highlightedColor;
            Coordinates = coordinates;
        }

        public void SetPieceModel(PieceModel pieceModel)
        {
            PieceModel = pieceModel;
        }

        public void RemovePieceModel()
        {
            PieceModel = null;
        }
    }
}
