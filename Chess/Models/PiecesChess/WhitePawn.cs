using Chess.Models.PiecesChess.DifferentPiece;

namespace Chess.Models.PiecesChess
{
    internal class WhitePawn:Pawn
    {
        public WhitePawn(PawnDirection direction) : base("../../Data/Img/White/Pawn.png", TeamEnum.WhiteTeam, direction)
        {
        }
        public override string ToString()
        {
            return "W_Paw";
        }
        public override object Clone()
        {
            return new WhitePawn(Direction) { IsFirstMove = IsFirstMove };
        }
    }
}
