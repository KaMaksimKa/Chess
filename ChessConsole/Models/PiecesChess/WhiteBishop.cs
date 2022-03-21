
using ChessConsole.Models.PiecesChess.DifferentPiece;

namespace ChessConsole.Models.PiecesChess
{
    internal class WhiteBishop:Bishop
    {
        public WhiteBishop() : base("Data/White/Bishop", TeamEnum.WhiteTeam)
        {
        }
        public override string ToString()
        {
            return "W_Bis";
        }
    }
}
