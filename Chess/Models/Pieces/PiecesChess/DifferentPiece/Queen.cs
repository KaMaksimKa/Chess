
using System.Collections.Generic;
using System.Drawing;
using Chess.Models.Boards.Base;
using Chess.Models.Pieces.Base;

namespace Chess.Models.Pieces.PiecesChess.DifferentPiece
{
    internal class Queen:Piece
    {
        public Queen(TeamEnum team) : base(TypePiece.Queen, team,90,
            new[,]{
                {-2.0, -1.0, -1.0, -0.5, -0.5, -1.0, -1.0, -2.0},
                {-1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, -1.0},
                {-1.0, 0.0, 0.5, 0.5, 0.5, 0.5, 0.0, -1.0},
                {-0.5, 0.0, 0.5, 0.5, 0.5, 0.5, 0.0, -0.5},
                {0.0, 0.0, 0.5, 0.5, 0.5, 0.5, 0.0, -0.5},
                {-1.0, 0.5, 0.5, 0.5, 0.5, 0.5, 0.0, -1.0},
                {-1.0, 0.0, 0.5, 0.0, 0.0, 0.0, 0.0, -1.0},
                { -2.0, -1.0, -1.0, -0.5, -0.5, -1.0, -1.0, -2.0}
            })
        {
        }
        public override Dictionary<(Point, Point), MoveInfo> GetMoves(Point startPoint, Board board)
        {
            List<(short, short)> moveVectors = new List<(short, short)>
            {
                (1, 1), (-1, 1), (1, -1), (-1, -1) , (1, 0), (-1, 0), (0, 1), (0, -1)
            };
            return MovePieces.GetStraightMoves(moveVectors, startPoint, board, Team);
        }

    }
}
