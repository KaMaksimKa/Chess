
using ChessConsole.Models.PiecesChess.DifferentPiece;

namespace ChessConsole.Models.PiecesChess
{
    internal class BlackRook:Rook
    {
        public BlackRook() : base("Data/Black/Rook", TeamEnum.BlackTeam)
        {
        }
        public override string ToString()
        {
            return "B_Roo";
        }
    }
}
