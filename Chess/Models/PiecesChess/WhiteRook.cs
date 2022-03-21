
using Chess.Models.PiecesChess.DifferentPiece;

namespace Chess.Models.PiecesChess
{
    internal class WhiteRook:Rook
    {
        public WhiteRook() : base("../../Data/Img/White/Rook.png", TeamEnum.WhiteTeam)
        {
        }
        public override string ToString()
        {
            return "W_Roo";
        }
    }
}
