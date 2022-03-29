using System.Collections.Generic;
using System.Drawing;
using Chess.Models.Boards.Base;
using Chess.Models.PiecesChess.Base;

namespace Chess.Models.PiecesChess.DifferentPiece
{
    internal abstract class Rook:Piece
    {
        protected Rook(string icon, TeamEnum team) : base(icon, team)
        {
        }

        public override Dictionary<(Point, Point), MoveInfo> GetMoves(Point startPoint, Board board)
        {
            List<(short, short)> moveVectors = new List<(short, short)> { (1, 0), (-1, 0), (0, 1), (0, -1) };
            return MovePieces.GetStraightMoves(moveVectors, startPoint, board, Team);
        }
    }
}
