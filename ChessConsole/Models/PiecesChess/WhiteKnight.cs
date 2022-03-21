
using ChessConsole.Models.PiecesChess.DifferentPiece;

namespace ChessConsole.Models.PiecesChess
{
    internal class WhiteKnight:Knight
    {
        public WhiteKnight() : base("Data/White/Knight", TeamEnum.WhiteTeam)
        {
        }
        public override string ToString()
        {
            return "W_Kni";
        }
    }
}
