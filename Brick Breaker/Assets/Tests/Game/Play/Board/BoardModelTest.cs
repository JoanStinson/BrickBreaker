using JGM.Game;
using NUnit.Framework;

namespace JGM.GameTests
{
    public class BoardModelTest
    {
        [TestCase(3, 3)]
        [TestCase(5, 5)]
        [TestCase(1, 1)]
        public void BoardModel_Constructor_ShouldInitializeGrid(int rows, int columns)
        {
            var boardModel = new BoardModel(rows, columns);

            Assert.AreEqual(rows, boardModel.Rows);
            Assert.AreEqual(columns, boardModel.Columns);
        }

        [TestCase(3, 3, 1, 1)]
        [TestCase(5, 5, 2, 3)]
        [TestCase(1, 1, 0, 0)]
        public void BoardModel_SetCell_ShouldSetCellCorrectly(int rows, int columns, int cellRow, int cellColumn)
        {
            var boardModel = new BoardModel(rows, columns);
            var cellModel = new CellModel();

            boardModel.SetCell(cellRow, cellColumn, cellModel);
            var retrievedCell = boardModel.GetCell(cellRow, cellColumn);

            Assert.AreEqual(cellModel, retrievedCell);
        }

        [TestCase(3, 3, 1, 1)]
        [TestCase(5, 5, 2, 3)]
        [TestCase(1, 1, 0, 0)]
        public void BoardModel_GetCell_ShouldReturnCorrectCell(int rows, int columns, int cellRow, int cellColumn)
        {
            var boardModel = new BoardModel(rows, columns);
            var cellModel = new CellModel();

            boardModel.SetCell(cellRow, cellColumn, cellModel);
            var retrievedCell = boardModel.GetCell(cellRow, cellColumn);

            Assert.AreEqual(cellModel, retrievedCell);
        }
    }
}
