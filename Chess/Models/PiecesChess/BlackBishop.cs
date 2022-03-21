
using Chess.Models.PiecesChess.DifferentPiece;

namespace Chess.Models.PiecesChess
{
    internal class BlackBishop:Bishop
    {
        public BlackBishop() : base("../../Data/Img/Black/Bishop.png", TeamEnum.BlackTeam)
        {
        }

        public override string ToString()
        {
            return "B_Bis";
        }
    }
}
