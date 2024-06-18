using UnityEngine;

namespace JGM.Game
{
    public class BoardModel
    {
        public int Rows => m_rows;
        public int Columns => m_columns;
        public int PiecesOnBoard { get; private set; }

        private readonly CellModel[,] m_grid;
        private readonly int m_rows;
        private readonly int m_columns;

        public BoardModel(int rows, int columns)
        {
            Debug.Assert(rows > 0 && columns > 0);
            m_rows = rows;
            m_columns = columns;
            m_grid = new CellModel[rows, columns];
        }

        public void SetCell(int row, int column, CellModel cellModel)
        {
            bool insideBounds = (row < m_grid.GetLength(0) && column < m_grid.GetLength(1));
            Debug.Assert(insideBounds);

            if (insideBounds)
            {
                m_grid[row, column] = cellModel;
            }
        }

        public CellModel GetCell(int row, int column)
        {
            bool insideBounds = (row < m_grid.GetLength(0) && column < m_grid.GetLength(1));
            Debug.Assert(insideBounds);

            if (insideBounds)
            {
                return m_grid[row, column];
            }

            return null;
        }

        public int CalculatePiecesOnBoard()
        {
            PiecesOnBoard = 0;

            for (int i = 0; i < m_rows; i++)
            {
                for (int j = 0; j < m_columns; j++)
                {
                    if (m_grid[i, j].PieceModel != null)
                    {
                        PiecesOnBoard++;
                    }
                }
            }

            return PiecesOnBoard;
        }
    }
}
