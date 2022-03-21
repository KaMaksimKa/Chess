using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessConsole.Models.PiecesChess.DifferentPiece
{
    internal static class MovePieces
    {
        public static IEnumerable<Point> GetStraightTrajectory(Point pos, Point newPos)
        {
            Point changePos = new(newPos.X - pos.X, newPos.Y - pos.Y);
            Point stepChangePos = new(changePos.X/Math.Max(Math.Abs(changePos.X), Math.Abs(changePos.Y)),
                                        changePos.Y / Math.Max(Math.Abs(changePos.X), Math.Abs(changePos.Y)));
            Point currentPos = new(pos.X, pos.Y);
            List<Point> trajectory = new List<Point>();
            while (!((currentPos.X + stepChangePos.X) == newPos.X && (currentPos.Y + stepChangePos.Y) == newPos.Y))
            {
                currentPos.X += stepChangePos.X;
                currentPos.Y += stepChangePos.Y;
                trajectory.Add(new Point(currentPos.X,currentPos.Y));
            }

            return trajectory;
        }
        
    }
}
