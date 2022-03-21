
using Chess.Models.PiecesChess.DifferentPiece;

namespace Chess.Models.PiecesChess
{
    internal class BlackKnight:Knight
    {
        public BlackKnight() : base("../../Data/Img/Black/Knight.png", TeamEnum.BlackTeam)
        {
        }
        public override string ToString()
        {
            return "B_Kni";
        }
    }
}
