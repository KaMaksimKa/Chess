using Chess.Models.PiecesChess.Base;

namespace Chess.Models
{
    public class BoardForDraw
    {
        public IHaveIcon?[,] Icons { get; set; } = new IHaveIcon[8, 8];
        public MoveInfo? LastMoveInfo { get; set; } = null;
    }
}
