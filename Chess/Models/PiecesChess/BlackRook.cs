
using Chess.Models.PiecesChess.DifferentPiece;

namespace Chess.Models.PiecesChess
{
    internal class BlackRook:Rook
    {
        public BlackRook() : base("../../Data/Img/Black/Rook.png", TeamEnum.BlackTeam)
        {
        }
        public override string ToString()
        {
            return "B_Roo";
        }
        public override object Clone()
        {
            return new BlackRook() { IsFirstMove = IsFirstMove };
        }
    }
}
