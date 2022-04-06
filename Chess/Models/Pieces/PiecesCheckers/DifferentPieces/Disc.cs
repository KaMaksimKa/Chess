using System;
using System.Collections.Generic;
using System.Drawing;
using Chess.Models.Boards.Base;
using Chess.Models.Pieces.Base;


namespace Chess.Models.Pieces.PiecesCheckers.DifferentPieces
{
    internal class Disc:Piece
    {
        public Disc(string icon, TeamEnum team, int price) : base(icon, team, price)
        {
        }

        public override Dictionary<(Point, Point), MoveInfo> GetMoves(Point startPoint, Board board)
        {
            throw new NotImplementedException();
        }
    }
}
