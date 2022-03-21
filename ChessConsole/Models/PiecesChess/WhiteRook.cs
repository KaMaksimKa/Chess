
using ChessConsole.Models.PiecesChess.DifferentPiece;

namespace ChessConsole.Models.PiecesChess
{
    internal class WhiteRook:Rook
    {
        public WhiteRook() : base("Data/White/Rook", TeamEnum.WhiteTeam)
        {
        }
        public override string ToString()
        {
            return "W_Roo";
        }
    }
}
