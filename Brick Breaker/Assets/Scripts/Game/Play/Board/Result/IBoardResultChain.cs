namespace JGM.Game
{
    public interface IBoardResultChain
    {
        IBoardResultChain NextChain { get; }

        bool CheckTicTacToe(BoardModel boardModel);
    }
}
