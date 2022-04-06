


using Chess.Models.Pieces.PiecesChess.DifferentPiece;

namespace Chess.Models.Pieces.PiecesChess
{
    internal class BlackQueen:Queen
    {
        public BlackQueen() : base("../../Data/Img/Black/Queen.png", TeamEnum.BlackTeam)
        {
        }
        public override string ToString()
        {
            return "B_Que";
        }
    }
}
