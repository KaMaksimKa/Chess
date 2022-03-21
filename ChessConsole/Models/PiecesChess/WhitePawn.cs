using ChessConsole.Models.PiecesChess.DifferentPiece;

namespace ChessConsole.Models.PiecesChess
{
    internal class WhitePawn:Pawn
    {
        public WhitePawn(PawnDirection direction) : base("Data/White/Pawn", TeamEnum.WhiteTeam, direction)
        {
        }
        public override string ToString()
        {
            return "W_Paw";
        }
    }
}
