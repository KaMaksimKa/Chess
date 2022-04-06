using Chess.Models.Pieces.Base;
using Chess.Models.Pieces.PiecesChess.DifferentPiece;


namespace Chess.Models.Pieces.PiecesChess
{
    internal class WhitePawn:Pawn
    {
        public WhitePawn(Direction direction) : base("../../Data/Img/White/Pawn.png", TeamEnum.WhiteTeam, direction)
        {
        }
        public override string ToString()
        {
            return "W_Paw";
        }
    }
}
