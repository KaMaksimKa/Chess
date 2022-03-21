using ChessConsole.Models.PiecesChess.DifferentPiece;

namespace ChessConsole.Models.PiecesChess
{
    internal class BlackPawn:Pawn
    {
        public BlackPawn(PawnDirection direction) : base("Data/Black/Pawn", TeamEnum.BlackTeam, direction)
        {
        }
        public override string ToString()
        {
            return "B_Paw";
        }
    }
}
