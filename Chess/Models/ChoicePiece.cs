using System.Collections.Generic;
using System.Drawing;
using Chess.Models.Pieces.Base;


namespace Chess.Models
{
    public class ChoicePiece
    {
        public List<Piece>? PiecesList { get; set; }
        public Point? WhereReplace { get; set; }
        public int? IndexReplacementPiece { get; set; } = null;

        public ChoicePiece(List<Piece> piecesList, Point whereReplace)
        {
            PiecesList = piecesList;
            WhereReplace = whereReplace;
        }

        public ChoicePiece()
        {
            
        }
    }
}
