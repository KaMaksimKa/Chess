
using Chess.Models.Pieces.PiecesChess.DifferentPiece;


namespace Chess.Models.Pieces.PiecesChess
{
    internal class WhiteKnight:Knight
    {
        public WhiteKnight() : base("../../Data/Img/White/Knight.png", TeamEnum.WhiteTeam)
        {
        }
        public override string ToString()
        {
            return "W_Kni";
        }
    }
}
