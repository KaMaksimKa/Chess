
using ChessConsole.Models.PiecesChess.DifferentPiece;

namespace ChessConsole.Models.PiecesChess
{
    internal class BlackBishop:Bishop
    {
        public BlackBishop() : base("Data/Black/Bishop", TeamEnum.BlackTeam)
        {
        }
        public override string ToString()
        {
            return "B_Bis";
        }
    }
}
