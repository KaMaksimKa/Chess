using System.Drawing;
using ChessConsole.Models.PiecesChess.Base;

namespace ChessConsole.Models.PiecesChess.DifferentPiece
{
    internal class Bishop:Piece
    {
        public Bishop(string icon, TeamEnum team) : base(icon, team)
        {
        }

        private bool IsMove(Point pos, Point newPos)
        {
            Point changePos = new(newPos.X - pos.X, newPos.Y - pos.Y);
            if (Math.Abs(changePos.X) ==Math.Abs(changePos.Y))
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
            {
                return MovePieces.GetStraightTrajectory(pos, newPos);
            }
            else
            {
                throw new ApplicationException("Сюда нельзя походить");
            }
        }

        public override IEnumerable<Point> GetTrajectoryForKill(Point pos, Point newPos)
        {
            if (IsKill(pos, newPos))
            {
                return MovePieces.GetStraightTrajectory(pos, newPos);
            }
            else
            {
                throw new ApplicationException("Сюда нельзя походить");
            }
        }
    }
}
