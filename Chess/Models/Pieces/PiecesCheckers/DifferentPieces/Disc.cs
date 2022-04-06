using System;
using System.Collections.Generic;
using System.Drawing;
using Chess.Models.Boards.Base;
using Chess.Models.Pieces.Base;


namespace Chess.Models.Pieces.PiecesCheckers.DifferentPieces
{
    internal class Disc:Piece
    {
        public Disc(TeamEnum team,Direction direction) : base(TypePiece.Disc, team, 10)
        {
            Direction = direction;
        }

        public override Dictionary<(Point, Point), MoveInfo> GetMoves(Point startPoint, Board board)
        {
            throw new NotImplementedException();
        }
    }
}
