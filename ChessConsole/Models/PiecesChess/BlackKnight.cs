
using ChessConsole.Models.PiecesChess.DifferentPiece;

namespace ChessConsole.Models.PiecesChess
{
    internal class BlackKnight:Knight
    {
        public BlackKnight() : base("Data/Black/Knight", TeamEnum.BlackTeam)
        {
        }
        public override string ToString()
        {
            return "B_Kni";
        }
    }
}
