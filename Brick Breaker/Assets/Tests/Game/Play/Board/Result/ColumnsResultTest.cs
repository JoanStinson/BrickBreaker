using JGM.Game;
using NUnit.Framework;

namespace JGM.GameTests
{
    public class ColumnsResultTest
    {
        [Test]
        public void CheckTicTacToe_ShouldReturnTrue_WhenColumnHasSamePlayerPieces()
        {
            var boardModel = CreateBoardModelWithPieces(new[]
            {
                new[] { 0, -1, -1 },
                new[] { 0, -1, -1 },
                new[] { 0, -1, -1 }
            });

            var columnsResult = new ColumnsResult();
            Assert.IsTrue(columnsResult.CheckTicTacToe(boardModel));
        }

        [Test]
        public void CheckTicTacToe_ShouldReturnFalse_WhenNoColumnsHaveSamePlayerPieces()
        {
            var boardModel = CreateBoardModelWithPieces(new[]
            {
                new[] { 0, 1, 0 },
                new[] { 1, 0, 1 },
                new[] { 0, 1, 0 }
            });

            var columnsResult = new ColumnsResult();
            Assert.IsFalse(columnsResult.CheckTicTacToe(boardModel));
        }

        [Test]
        public void CheckTicTacToe_ShouldReturnTrue_WhenMiddleColumnHasSamePlayerPieces()
        {
            var boardModel = CreateBoardModelWithPieces(new[]
            {
                new[] { -1, 1, -1 },
                new[] { -1, 1, -1 },
                new[] { -1, 1, -1 }
            });

            var columnsResult = new ColumnsResult();
            Assert.IsTrue(columnsResult.CheckTicTacToe(boardModel));
        }

        [Test]
        public void CheckTicTacToe_ShouldReturnTrue_WhenLastColumnHasSamePlayerPieces()
        {
            var boardModel = CreateBoardModelWithPieces(new[]
            {
                new[] { -1, -1, 0 },
                new[] { -1, -1, 0 },
                new[] { -1, -1, 0 }
            });

            var columnsResult = new ColumnsResult();
            Assert.IsTrue(columnsResult.CheckTicTacToe(boardModel));
        }

        [Test]
        public void CheckTicTacToe_ShouldReturnFalse_WhenColumnsHaveMixedPlayerPieces()
        {
            var boardModel = CreateBoardModelWithPieces(new[]
            {
                new[] { 0, 1, 0 },
                new[] { 1, 1, 0 },
                new[] { 0, 0, 1 }
            });

            var columnsResult = new ColumnsResult();
            Assert.IsFalse(columnsResult.CheckTicTacToe(boardModel));
        }

        [Test]
        public void CheckTicTacToe_ShouldReturnFalse_WhenColumnIsEmpty()
        {
            var boardModel = CreateBoardModelWithPieces(new[]
            {
                new[] { -1, -1, -1 },
                new[] { -1, -1, -1 },
                new[] { -1, -1, -1 }
            });

            var columnsResult = new ColumnsResult();
            Assert.IsFalse(columnsResult.CheckTicTacToe(boardModel));
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
