namespace JGM.Game
{
    public class DiagonalsResult : IBoardResultChain
    {
        public IBoardResultChain NextChain { get; set; }

        public bool CheckTicTacToe(BoardModel boardModel)
        {
            // Check the main diagonal (top left to bottom right)
            if (CheckDiagonal(boardModel, true))
            {
                return true;
            }

            // Check the anti-diagonal (top right to bottom left)
            if (CheckDiagonal(boardModel, false))
            {
                return true;
            }

            return NextChain?.CheckTicTacToe(boardModel) ?? false;
        }

        private bool CheckDiagonal(BoardModel boardModel, bool isMainDiagonal)
        {
            int startRow = 0;
            int startCol = isMainDiagonal ? 0 : boardModel.Columns - 1;
            PieceModel startPieceModel = boardModel.GetCell(startRow, startCol).PieceModel;

            if (startPieceModel == null)
            {
                return false;
            }

            int playerId = startPieceModel.PlayerId;
            bool isTicTacToe = true;

            for (int i = 1; i < boardModel.Rows; i++)
            {
                int j = isMainDiagonal ? i : boardModel.Columns - i - 1;
                var pieceModel = boardModel.GetCell(i, j).PieceModel;

                if (pieceModel == null || pieceModel.PlayerId != playerId)
                {
                    isTicTacToe = false;
                    break;
                }
            }

            return isTicTacToe;
        }
    }
}