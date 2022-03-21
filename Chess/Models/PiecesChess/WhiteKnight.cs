
using Chess.Models.PiecesChess.DifferentPiece;

namespace Chess.Models.PiecesChess
{
    internal class WhiteKnight:Knight
    {
        public WhiteKnight() : base("../../Data/Img/White/Knight.png", TeamEnum.WhiteTeam)
        {
        }
        public override string ToString()
        {
            return "W_Kni";
        }
    }
}
