using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Documents;


namespace Chess.Models.PiecesChess.DifferentPiece
{
    internal static class MovePieces
    {
        public static List<Point> GetStraightTrajectory(Point startPoint, Point endPoint)
        {
            List<Point> straightTrajectory = new List<Point>();

            var xChange = endPoint.X - startPoint.X;
            var yChange = endPoint.Y - startPoint.Y;
            if (xChange == 0 && yChange == 0)
            {
                return straightTrajectory;
            }
            var xChangeStep = xChange / Math.Max(Math.Abs(xChange), Math.Abs(yChange));
            var yChangeStep = yChange / Math.Max(Math.Abs(xChange), Math.Abs(yChange));

            int currentX = startPoint.X;
            int currentY = startPoint.Y;

            while (!((currentX + xChangeStep) == endPoint.X && (currentY + yChangeStep) == endPoint.Y))
            {
                currentX += xChangeStep;
                currentY += yChangeStep;
                straightTrajectory.Add(new Point(currentX, currentY));
            }

            return straightTrajectory;
        }
        
    }
}
