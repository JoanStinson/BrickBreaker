namespace JGM.Game
{
    public class BoardResultController
    {
        private readonly IBoardResultChain m_firstResultChain;

        public BoardResultController()
        {
            var rowsResult = new RowsResult();
            var columnsResult = new ColumnsResult();
            var diagonalsResult = new DiagonalsResult();

            m_firstResultChain = rowsResult;
            rowsResult.NextChain = columnsResult;
            columnsResult.NextChain = diagonalsResult;
        }

        public bool CheckTicTactoe(BoardModel boardModel)
        {
            return m_firstResultChain.CheckTicTacToe(boardModel);
        }
    }
}
