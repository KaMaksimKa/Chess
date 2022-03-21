
using ChessConsole.Models.PiecesChess.DifferentPiece;

namespace ChessConsole.Models.PiecesChess
{
    internal class WhiteQueen:Queen
    {
        public WhiteQueen() : base("Data/White/Queen", TeamEnum.WhiteTeam)
        {
        }
        public override string ToString()
        {
            return "W_Que";
        }
    }
}
