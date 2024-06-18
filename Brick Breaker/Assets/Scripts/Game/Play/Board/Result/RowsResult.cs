namespace JGM.Game
{
    public class RowsResult : IBoardResultChain
    {
        public IBoardResultChain NextChain { get; set; }

        public bool CheckTicTacToe(BoardModel boardModel)
        {
            for (int i = 0; i < boardModel.Rows; i++)
            {
                bool isTicTacToe = true;
                var startPieceModel = boardModel.GetCell(i, 0).PieceModel;

                // If starting piece is null, it can't be a tic-tac-toe
                if (startPieceModel == null)
                {
                    continue;
                }

                int playerId = startPieceModel.PlayerId;

                // Check all pieces are from the same player
                for (int j = 0; j < boardModel.Columns; j++)
                {
                    var pieceModel = boardModel.GetCell(i, j).PieceModel;
                    if (pieceModel == null || pieceModel.PlayerId != playerId)
                    {
                        isTicTacToe = false;
                        break;
                    }
                }

                if (isTicTacToe)
                {
                    return true;
                }
            }

            return NextChain?.CheckTicTacToe(boardModel) ?? false;
        }
    }
}
