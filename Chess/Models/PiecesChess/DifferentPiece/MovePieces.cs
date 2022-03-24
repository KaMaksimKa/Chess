using System;
using System.Collections.Generic;
using System.Drawing;


namespace Chess.Models.PiecesChess.DifferentPiece
{
    internal static class MovePieces
    {
        public static IEnumerable<Point> GetStraightTrajectory(Point startPoint, Point endPoint)
        {
            var xChange = endPoint.X - startPoint.X;
            var yChange = endPoint.Y - startPoint.Y;

            var xChangeStep = xChange / Math.Max(Math.Abs(xChange), Math.Abs(yChange));
            var yChangeStep = yChange / Math.Max(Math.Abs(xChange), Math.Abs(yChange));

            int currentX = startPoint.X;
            int currentY = startPoint.Y;

            while (!((currentX + xChangeStep) == endPoint.X && (currentY + yChangeStep) == endPoint.Y))
            {
                currentX += xChangeStep;
                currentY += yChangeStep;
                yield return new Point(currentX,currentY);
            }
        }
        
    }
}
