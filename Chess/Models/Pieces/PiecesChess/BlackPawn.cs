using Chess.Models.Pieces.Base;
using Chess.Models.Pieces.PiecesChess.DifferentPiece;

namespace Chess.Models.Pieces.PiecesChess
{
    internal class BlackPawn:Pawn
    {
        public BlackPawn(Direction direction) : base("../../Data/Img/Black/Pawn.png", TeamEnum.BlackTeam, direction)
        {
        }
        public override string ToString()
        {
            return "B_Paw";
        }
    }
}
