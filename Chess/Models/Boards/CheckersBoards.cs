using Chess.Models.Boards.Base;
using Chess.Models.Pieces.Base;

namespace Chess.Models.Boards
{
    internal class CheckersBoards:Board
    {
        public CheckersBoards(Piece?[,] arrayBoard) : base(arrayBoard)
        {
        }
    }
}
