namespace JGM.Game
{
    public class ColumnsResult : IBoardResultChain
    {
        public IBoardResultChain NextChain { get; set; }

        public bool CheckTicTacToe(BoardModel boardModel)
        {
            for (int j = 0; j < boardModel.Columns; j++)
            {
                bool isTicTacToe = true;
                var startPieceModel = boardModel.GetCell(0, j).PieceModel;

                // If starting piece is null, it can't be a tic-tac-toe
                if (startPieceModel == null)
                {
                    continue;
                }

                int playerId = startPieceModel.PlayerId;

                // Check all pieces are from the same player
                for (int i = 1; i < boardModel.Rows; i++)
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