using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace JGM.Game
{
    public class BoardView : MonoBehaviour
    {
        public Action OnPiecePlaced { get; set; }
        public int PiecesOnBoard => m_boardModel.PiecesOnBoard;

        [SerializeField] private CellView[] m_cells;
        [Inject] private IAudioService m_audioService;

        private BoardModel m_boardModel;
        private BoardController m_boardController;

        public void Initialize(GameModel gameModel, BoardModel boardModel)
        {
            m_boardModel = boardModel;
            InitializeBoardCells(gameModel);
            m_boardController = new BoardController(boardModel);
        }

        private void InitializeBoardCells(GameModel gameModel)
        {
            int currentRow = 0;
            int currentColumn = -1;

            for (int i = 0; i < m_cells.Length; i++)
            {
                if (++currentColumn > 2)
                {
                    currentColumn = 0;
                    currentRow++;
                }

                var boardCellColor = (i % 2 == 0) ? gameModel.BoardCellDarkBrownColor : gameModel.BoardCellLightBrownColor;
                var boardCell = new CellModel(null, boardCellColor, gameModel.BoardCellHighlightedColor, new Vector2Int(currentRow, currentColumn));
                m_cells[i].Initialize(boardCell, currentRow, currentColumn);
                m_boardModel.SetCell(currentRow, currentColumn, boardCell);
            }
        }

        public void HighlightCellIfValidForPiece(PieceView pieceView, PointerEventData eventData)
        {
            foreach (var cell in m_cells)
            {
                var cellTransform = (RectTransform)cell.transform;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(cellTransform, eventData.position, eventData.pressEventCamera, out var cellLocalPoint);

                if (cellTransform.rect.Contains(cellLocalPoint) && m_boardController.CellIsValidForPiece(cell.Model, pieceView))
                {
                    cell.SetHighlightedColor();
                }
                else
                {
                    cell.SetDefaultColor();
                }
            }
        }

        public bool PlacePieceOnBoard(PieceView pieceView, PointerEventData eventData)
        {
            bool placedPiece = false;

            foreach (var cell in m_cells)
            {
                var cellTransform = (RectTransform)cell.transform;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(cellTransform, eventData.position, eventData.pressEventCamera, out var cellLocalPoint);

                if (cellTransform.rect.Contains(cellLocalPoint) && m_boardController.CellIsValidForPiece(cell.Model, pieceView))
                {
                    pieceView.CellView?.RemovePiece();
                    cell.SetPiece(pieceView);
                    placedPiece = true;
                }

                cell.SetDefaultColor();
            }

            if (placedPiece)
            {
                OnPiecePlaced?.Invoke();
                m_audioService.Play(AudioFileNames.PlacePieceSfx);
            }

            return placedPiece;
        }

        public bool CheckTicTacToe()
        {
            return m_boardController.CheckTicTacToe();
        }

        public void ClearBoard()
        {
            foreach (var cell in m_cells)
            {
                cell.RemovePiece();
                cell.DestroyPiece();
                cell.SetDefaultColor();
            }
        }

        public void HighlightPlayerWinCells(int playerWinId)
        {
            foreach (var cell in m_cells)
            {
                if (cell.Model.PieceModel != null && cell.Model.PieceModel.PlayerId == playerWinId)
                {
                    cell.SetHighlightedColor();
                }
            }
        }

        public int CalculatePiecesAmount()
        {
            return m_boardModel.CalculatePiecesOnBoard();
        }

        public bool AnyPieceFromPlayerCanMove(IReadOnlyList<PieceView> pieces, BoardModel boardModel)
        {
            return m_boardController.AnyPieceFromPlayerCanMove(pieces, boardModel);
        }
    }
}
