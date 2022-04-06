
using Chess.Models.Pieces.PiecesChess.DifferentPiece;


namespace Chess.Models.Pieces.PiecesChess
{
    internal class WhiteBishop:Bishop
    {
        public WhiteBishop() : base("../../Data/Img/White/Bishop.png", TeamEnum.WhiteTeam)
        {
        }
        public override string ToString()
        {
            return "W_Bis";
        }
    }
}
