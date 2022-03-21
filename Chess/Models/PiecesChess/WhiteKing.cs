using Chess.Models.PiecesChess.DifferentPiece;

namespace Chess.Models.PiecesChess
{
    internal class WhiteKing:King
    {
        public WhiteKing() : base("../../Data/Img/White/King.png", TeamEnum.WhiteTeam)
        {
        }
        public override string ToString()
        {
            return "W_BKin";
        }
    }
}
