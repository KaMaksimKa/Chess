using Chess.Models.PiecesChess.DifferentPiece;

namespace Chess.Models.PiecesChess
{
    internal class BlackPawn:Pawn
    {
        public BlackPawn(PawnDirection direction) : base("../../Data/Img/Black/Pawn.png", TeamEnum.BlackTeam, direction)
        {
        }
        public override string ToString()
        {
            return "B_Paw";
        }
    }
}
