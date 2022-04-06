
using Chess.Models.Pieces.PiecesChess.DifferentPiece;


namespace Chess.Models.Pieces.PiecesChess
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
    }
}
