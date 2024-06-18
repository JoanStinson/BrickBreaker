using JGM.Game;
using NUnit.Framework;

namespace JGM.GameTests
{
    public class DiagonalsResultTest
    {
        [Test]
        public void CheckTicTacToe_ShouldReturnTrue_WhenMainDiagonalHasSamePlayerPieces()
        {
            var boardModel = CreateBoardModelWithPieces(new[]
            {
                new[] { 0, -1, -1 },
                new[] { -1, 0, -1 },
                new[] { -1, -1, 0 }
            });

            var diagonalsResult = new DiagonalsResult();
            Assert.IsTrue(diagonalsResult.CheckTicTacToe(boardModel));
        }

        [Test]
        public void CheckTicTacToe_ShouldReturnTrue_WhenAntiDiagonalHasSamePlayerPieces()
        {
            var boardModel = CreateBoardModelWithPieces(new[]
            {
                new[] { -1, -1, 1 },
                new[] { -1, 1, -1 },
                new[] { 1, -1, -1 }
            });

            var diagonalsResult = new DiagonalsResult();
            Assert.IsTrue(diagonalsResult.CheckTicTacToe(boardModel));
        }

        [Test]
        public void CheckTicTacToe_ShouldReturnFalse_WhenNoDiagonalsHaveSamePlayerPieces()
        {
            var boardModel = CreateBoardModelWithPieces(new[]
            {
                new[] { 0, -1, 1 },
                new[] { -1, 0, -1 },
                new[] { 1, -1, -1 }
            });

            var diagonalsResult = new DiagonalsResult();
            Assert.IsFalse(diagonalsResult.CheckTicTacToe(boardModel));
        }

        [Test]
        public void CheckTicTacToe_ShouldReturnFalse_WhenDiagonalsAreEmpty()
        {
            var boardModel = CreateBoardModelWithPieces(new[]
            {
                new[] { -1, -1, -1 },
                new[] { -1, -1, -1 },
                new[] { -1, -1, -1 }
            });

            var diagonalsResult = new DiagonalsResult();
            Assert.IsFalse(diagonalsResult.CheckTicTacToe(boardModel));
        }

        [Test]
        public void CheckTicTacToe_ShouldReturnFalse_WhenDiagonalsHaveMixedPlayerPieces()
        {
            var boardModel = CreateBoardModelWithPieces(new[]
            {
                new[] { 0, -1, 1 },
                new[] { -1, -1, -1 },
                new[] { 1, -1, 0 }
            });

            var diagonalsResult = new DiagonalsResult();
            Assert.IsFalse(diagonalsResult.CheckTicTacToe(boardModel));
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
