using ChessConsole.Models.PiecesChess.DifferentPiece;

namespace ChessConsole.Models.PiecesChess
{
    internal class WhiteKing:King
    {
        public WhiteKing() : base("Data/White/King", TeamEnum.WhiteTeam)
        {
        }
        public override string ToString()
        {
            return "W_Kin";
        }
    }
}
