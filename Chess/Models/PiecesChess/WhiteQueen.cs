
using Chess.Models.PiecesChess.DifferentPiece;

namespace Chess.Models.PiecesChess
{
    internal class WhiteQueen:Queen
    {
        public WhiteQueen() : base("../../Data/Img/White/Queen.png", TeamEnum.WhiteTeam)
        {
        }
        public override string ToString()
        {
            return "W_Que";
        }
        public override object Clone()
        {
            return new WhiteQueen() { IsFirstMove = IsFirstMove };
        }
    }
}
