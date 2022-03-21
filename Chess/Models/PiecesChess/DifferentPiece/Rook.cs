using System;
using System.Collections.Generic;
using System.Drawing;
using Chess.Models.PiecesChess.Base;

namespace Chess.Models.PiecesChess.DifferentPiece
{
    internal class Rook:Piece
    {
        public Rook(string icon, TeamEnum team) : base(icon, team)
        {
        }

        private bool IsMove(Point pos, Point newPos)
        {
            Point changePos = new(newPos.X - pos.X, newPos.Y - pos.Y);
            if ((changePos.X == 0 && changePos.Y != 0 ) || (changePos.X != 0 && changePos.Y == 0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsKill(Point pos, Point newPos)
        {
            return IsMove(pos, newPos);
        }

        public override IEnumerable<Point> GetTrajectoryForMove(Point pos, Point newPos)
        {
            if (IsMove(pos, newPos))
                return MovePieces.GetStraightTrajectory(pos, newPos);
            else
                throw new ApplicationException("Сюда нельзя походить");
        }

        public override IEnumerable<Point> GetTrajectoryForKill(Point pos, Point newPos)
        {
            if (IsKill(pos, newPos))
                return MovePieces.GetStraightTrajectory(pos, newPos);
            else
                throw new ApplicationException("Сюда нельзя походить");
        }
    }
}
