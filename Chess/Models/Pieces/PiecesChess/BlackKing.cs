using Chess.Models.Pieces.PiecesChess.DifferentPiece;


namespace Chess.Models.Pieces.PiecesChess
{
    internal class BlackKing:King
    {
        public BlackKing() : base("../../Data/Img/Black/King.png", TeamEnum.BlackTeam)
        {
        }
        public override string ToString()
        {
            return "B_Kin";
        }
    }
}
