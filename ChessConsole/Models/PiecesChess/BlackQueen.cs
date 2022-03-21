
using ChessConsole.Models.PiecesChess.DifferentPiece;

namespace ChessConsole.Models.PiecesChess
{
    internal class BlackQueen:Queen
    {
        public BlackQueen() : base("Data/Black/Queen", TeamEnum.BlackTeam)
        {
        }
        public override string ToString()
        {
            return "B_Que";
        }
    }
}
