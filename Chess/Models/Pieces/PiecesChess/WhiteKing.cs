using Chess.Models.Pieces.PiecesChess.DifferentPiece;

namespace Chess.Models.Pieces.PiecesChess
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
