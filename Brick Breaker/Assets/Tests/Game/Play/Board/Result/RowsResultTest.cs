using JGM.Game;
using NUnit.Framework;

namespace JGM.GameTests
{
    public class RowsResultTest
    {
        [Test]
        public void CheckTicTacToe_ShouldReturnTrue_WhenRowHasSamePlayerPieces()
        {
            var boardModel = CreateBoardModelWithPieces(new[]
            {
                new[] { 0, 0, 0 },
                new[] { -1, -1, -1 },
                new[] { -1, -1, -1 }
            });

            var rowsResult = new RowsResult();
            Assert.IsTrue(rowsResult.CheckTicTacToe(boardModel));
        }

        [Test]
        public void CheckTicTacToe_ShouldReturnFalse_WhenNoRowsHaveSamePlayerPieces()
        {
            var boardModel = CreateBoardModelWithPieces(new[]
            {
                new[] { 0, 1, 0 },
                new[] { 1, 0, 1 },
                new[] { 0, 1, 0 }
            });

            var rowsResult = new RowsResult();
            Assert.IsFalse(rowsResult.CheckTicTacToe(boardModel));
        }

        [Test]
        public void CheckTicTacToe_ShouldReturnTrue_WhenMiddleRowHasSamePlayerPieces()
        {
            var boardModel = CreateBoardModelWithPieces(new[]
            {
                new[] { -1, -1, -1 },
                new[] { 1, 1, 1 },
                new[] { -1, -1, -1 }
            });

            var rowsResult = new RowsResult();
            Assert.IsTrue(rowsResult.CheckTicTacToe(boardModel));
        }

        [Test]
        public void CheckTicTacToe_ShouldReturnTrue_WhenLastRowHasSamePlayerPieces()
        {
            var boardModel = CreateBoardModelWithPieces(new[]
            {
                new[] { -1, -1, -1 },
                new[] { -1, -1, -1 },
                new[] { 0, 0, 0 }
            });

            var rowsResult = new RowsResult();
            Assert.IsTrue(rowsResult.CheckTicTacToe(boardModel));
        }

        [Test]
        public void CheckTicTacToe_ShouldReturnFalse_WhenRowsHaveMixedPlayerPieces()
        {
            var boardModel = CreateBoardModelWithPieces(new[]
            {
                new[] { 0, 1, 0 },
                new[] { 1, 1, 0 },
                new[] { 0, 0, 1 }
            });

            var rowsResult = new RowsResult();
            Assert.IsFalse(rowsResult.CheckTicTacToe(boardModel));
        }

        [Test]
        public void CheckTicTacToe_ShouldReturnFalse_WhenRowIsEmpty()
        {
            var boardModel = CreateBoardModelWithPieces(new[]
            {
                new[] { -1, -1, -1 },
                new[] { -1, -1, -1 },
                new[] { -1, -1, -1 }
            });

            var rowsResult = new RowsResult();
            Assert.IsFalse(rowsResult.CheckTicTacToe(boardModel));
        }

        private BoardModel CreateBoardModelWithPieces(int[][] piecePositions)
        {
            int rows = piecePositions.Length;
            int columns = piecePositions[0].Length;
            var boardModel = new BoardModel(rows, columns);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    var cellModel = new CellModel();
                    if (piecePositions[i][j] != -1)
                    {
                        cellModel.SetPieceModel(new PieceModel(piecePositions[i][j], GameSettings.PieceType.Knight, null, 1, 1));
                    }
                    boardModel.SetCell(i, j, cellModel);
                }
            }

            return boardModel;
        }
    }
}
