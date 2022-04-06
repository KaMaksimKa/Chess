
using Chess.Models.Pieces.PiecesChess.DifferentPiece;

namespace Chess.Models.Pieces.PiecesChess
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
