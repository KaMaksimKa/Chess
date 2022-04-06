
using Chess.Models.Pieces.PiecesChess.DifferentPiece;


namespace Chess.Models.Pieces.PiecesChess
{
    internal class BlackKnight:Knight
    {
        public BlackKnight() : base("../../Data/Img/Black/Knight.png", TeamEnum.BlackTeam)
        {
        }
        public override string ToString()
        {
            return "B_Kni";
        }
    }
}
